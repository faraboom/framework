using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Faraboom.Framework.DataAnnotation;
using Faraboom.Framework.Resources;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;
using draw = DocumentFormat.OpenXml.Drawing;
using oDraw = DocumentFormat.OpenXml.Office2010.Drawing;

namespace Faraboom.Framework.Core.Utils
{
    public class Export
    {
        public static (IActionResult content, long size) CreateExcelDocumentInHttpResponseMessage(IList lst, DataTable dataTable = null, string responseFileName = null)
        {
            var data = CreateData(lst, dataTable, responseFileName);

            return (new ObjectResult(data.Item1) { StatusCode = (int)System.Net.HttpStatusCode.OK }, data.Item1.Length);

            //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            //{
            //    FileName = data.Item2
            //};
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");
            //response.Content.Headers.ContentLength = data.Item1.Length;

            //return response;
        }

        private static Tuple<byte[], string> CreateData(IList lst, DataTable dataTable = null, string responseFileName = null)
        {
            if (string.IsNullOrWhiteSpace(responseFileName))
                responseFileName = "FaraboomReport";

            responseFileName += ".xlsx";

            var ds = new DataSet();
            var dt = new DataTable();
            if (lst?.Count > 0)
            {
                var localizedColumnNames = new Dictionary<string, string>();
                bool dataIsDictionaryBase = lst[0] as Dictionary<string, string> != null;
                IEnumerable<PropertyInfo> properties = null;
                if (dataTable != null)
                {
                    dataIsDictionaryBase = true;

                    foreach (DataColumn item in dataTable.Columns)
                    {
                        localizedColumnNames.Add(item.ColumnName, item.Caption);
                        dt.Columns.Add(new DataColumn(item.Caption, typeof(string)));
                    }
                }
                else
                {
                    var captions = lst[0] as Dictionary<string, string>;
                    if (captions != null)
                    {
                        foreach (string key in captions.Keys)
                        {
                            localizedColumnNames.Add(key, key);
                            dt.Columns.Add(new DataColumn(key, typeof(string)));
                        }
                    }
                    else
                    {
                        var type = lst[0].GetType();
                        properties = type.GetProperties().Where(t => t.GetCustomAttribute<JsonIgnoreAttribute>(false) == null && (t.GetCustomAttribute<ExportInfoAttribute>() == null || !t.GetCustomAttribute<ExportInfoAttribute>().Ignore));
                        var i = 0;
                        foreach (var info in properties)
                        {
                            var description = Globals.GetLocalizedDescription(info);
                            var name = description != info.Name ? description : Globals.GetLocalizedDisplayName(info);
                            if (dt.Columns.Contains(name))
                                name += i;
                            localizedColumnNames.Add(info.Name, name);
                            var columnType = GetNullableType(info.PropertyType);

                            if (columnType == typeof(bool))
                            {
                                var exportInfo = info.GetCustomAttribute<ExportInfoAttribute>();
                                if (exportInfo != null && !string.IsNullOrWhiteSpace(exportInfo.TrueResourceKey))
                                    columnType = typeof(string);
                            }

                            dt.Columns.Add(new DataColumn(name, columnType));
                            i++;
                        }
                    }
                }

                foreach (var t in lst)
                {
                    var row = dt.NewRow();

                    if (dataIsDictionaryBase)
                    {
                        var dictionaryValues = t as Dictionary<string, string>;
                        foreach (var key in dictionaryValues.Keys)
                        {
                            var value = dictionaryValues[key];
                            if (dataTable != null)
                            {
                                var exportInfo = dataTable.Columns[key].ExtendedProperties[nameof(ExportInfoAttribute)] as ExportInfoAttribute;
                                if (exportInfo != null && !string.IsNullOrWhiteSpace(exportInfo.TrueResourceKey))
                                {
                                    if (exportInfo.ResourceType == null)
                                        exportInfo.ResourceType = typeof(GlobalResource);
                                    var resourceManager = new System.Resources.ResourceManager(exportInfo.ResourceType);
                                    value = value.ValueOf<bool>() ? resourceManager.GetString(exportInfo.TrueResourceKey) : resourceManager.GetString(exportInfo.FalseResourceKey);
                                }
                            }

                            row[localizedColumnNames[key]] = value;
                        }
                    }
                    else
                    {
                        foreach (var info in properties)
                        {
                            var pureValue = info.GetValue(t, null);

                            if (pureValue != null)
                            {
                                var typeValue = GetNullableTypeValue(info.PropertyType);
                                if (typeValue.IsEnum)
                                    pureValue = EnumHelper.LocalizeEnum(pureValue);
                                else if (typeValue == typeof(bool))
                                {
                                    var exportInfo = info.GetCustomAttribute<ExportInfoAttribute>();
                                    if (exportInfo != null && !string.IsNullOrWhiteSpace(exportInfo.TrueResourceKey))
                                    {
                                        if (exportInfo.ResourceType == null)
                                            exportInfo.ResourceType = typeof(GlobalResource);
                                        var resourceManager = new System.Resources.ResourceManager(exportInfo.ResourceType);
                                        pureValue = (bool)pureValue ? resourceManager.GetString(exportInfo.TrueResourceKey) : resourceManager.GetString(exportInfo.FalseResourceKey);
                                    }
                                }
                            }

                            var columnName = localizedColumnNames[info.Name];
                            if (!IsNullableType(info.PropertyType))
                                row[columnName] = pureValue;
                            else
                                row[columnName] = pureValue ?? DBNull.Value;
                        }
                    }

                    dt.Rows.Add(row);
                }
            }
            ds.Tables.Add(dt);

            var stream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
            {
                WriteExcelFile(ds, document);
            }
            stream.Flush();
            stream.Position = 0;

            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();

            return new Tuple<byte[], string>(data, responseFileName);
        }

        private static Type GetNullableTypeValue(Type t)
        {
            var returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                returnType = Nullable.GetUnderlyingType(t);

            return returnType;
        }

        private static Type GetNullableType(Type t)
        {
            var returnType = GetNullableTypeValue(t);
            if (returnType.IsEnum)
                returnType = typeof(string);

            return returnType;
        }

        private static bool IsNullableType(Type type)
        {
            return type == typeof(string) ||
                   type.IsArray ||
                   (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        private static void WriteExcelFile(DataSet ds, SpreadsheetDocument spreadsheet)
        {
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new Workbook();
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            var workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("IdStyles");
            var stylesheet = new Stylesheet();
            workbookStylesPart.Stylesheet = stylesheet;

            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            foreach (DataTable dt in ds.Tables)
            {
                var newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>("IdSheetPart");
                newWorksheetPart.Worksheet =
                    new Worksheet(new SheetViews(new SheetView { WorkbookViewId = 0, RightToLeft = Globals.IsRtl }),
                        new SheetData());

                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                newWorksheetPart.Worksheet.Save();

                if (worksheetNumber == 1)
                    spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());

                spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = worksheetNumber,
                    Name = dt.TableName,
                });

                string drawingID = "IdDrawingsPart";

                if (newWorksheetPart.DrawingsPart != null
                && newWorksheetPart.DrawingsPart.WorksheetDrawing != null)
                {
                    Drawing drawing1 = new Drawing() { Id = drawingID };
                    newWorksheetPart.Worksheet.Append(drawing1);
                }

                worksheetNumber++;
            }
            spreadsheet.WorkbookPart.Workbook.Save();
        }

        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            var numberOfColumns = dt.Columns.Count;
            var columnRenderType = new CellValues[numberOfColumns];

            var excelColumnNames = new string[numberOfColumns];
            for (var i = 0; i < numberOfColumns; i++)
            {
                excelColumnNames[i] = GetExcelColumnName(i);
            }

            //  Create the Header row in our Excel Worksheet
            uint rowIndex = 1;

            var headerRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(headerRow);

            for (var i = 0; i < numberOfColumns; i++)
            {
                var col = dt.Columns[i];
                AppendCell(excelColumnNames[i] + "1", col.ColumnName, headerRow, CellValues.String, worksheetPart, i, (int)rowIndex);
                switch (col.DataType.FullName)
                {
                    case "System.Decimal":
                    case "System.Int32":
                        columnRenderType[i] = CellValues.Number;
                        break;
                    case "System.Boolean":
                        columnRenderType[i] = CellValues.Boolean;
                        break;
                    default:
                        columnRenderType[i] = CellValues.String;
                        break;
                }
            }

            //  Now, step through each row of data in our DataTable...
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);

                for (var colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    var cellValue = dr.ItemArray[colInx].ToString();
                    AppendCell(excelColumnNames[colInx] + rowIndex, cellValue, newExcelRow, columnRenderType[colInx],
                        worksheetPart, colInx, (int)rowIndex);
                }
            }
        }

        private static void AppendCell(string cellReference, string cellStringValue, Row excelRow, CellValues dataType, WorksheetPart worksheetPart, int colInx, int rowIndex)
        {
            var isImage = false;
            var appendCell = true;
            switch (dataType)
            {
                case CellValues.Boolean:
                    {
                        bool cellBooleanValue;
                        if (bool.TryParse(cellStringValue, out cellBooleanValue))
                        {
                            cellStringValue = cellBooleanValue ? GlobalResource.Yes : GlobalResource.No;
                            dataType = CellValues.String;
                        }
                        else
                        {
                            appendCell = false;
                        }
                    }
                    break;
                case CellValues.Number:
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        double cellNumericValue;
                        if (double.TryParse(cellStringValue, out cellNumericValue))
                            cellStringValue = cellNumericValue.ToString();
                        else
                            appendCell = false;
                    }
                    break;
                case CellValues.String:
                    {
                        if (cellStringValue.StartsWith("data:image/"))
                        {
                            isImage = true;

                            //if the data is Image, we need to serailize 
                            //its characteristics information in the drawing part
                            //and then raw image need to be added as Image part
                            DrawingsPart drawingsPart = null;
                            Xdr.WorksheetDrawing worksheetDrawing = new Xdr.WorksheetDrawing();
                            if (worksheetPart.DrawingsPart == null)
                            {
                                drawingsPart = worksheetPart.AddNewPart<DrawingsPart>("IdDrawingsPart");
                                drawingsPart.WorksheetDrawing = worksheetDrawing;
                            }
                            else if (worksheetPart.DrawingsPart != null
                                    && worksheetPart.DrawingsPart.WorksheetDrawing != null)
                            {
                                drawingsPart = worksheetPart.DrawingsPart;
                                worksheetDrawing = worksheetPart.DrawingsPart.WorksheetDrawing;
                            }

                            string imageId = "Id" + rowIndex.ToString();

                            Xdr.TwoCellAnchor cellAnchor = AddTwoCellAnchor
                                (rowIndex - 1, colInx, rowIndex, colInx + 1, imageId);

                            worksheetDrawing.Append(cellAnchor);

                            ImagePart imagePart = drawingsPart.AddNewPart<ImagePart>("image/png", imageId);
                            cellStringValue = cellStringValue.Split(' ')[1];
                            byte[] bytes = Convert.FromBase64String(cellStringValue);

                            Image data;
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                data = Image.FromStream(ms);
                            }

                            GenerateImagePartContent(imagePart, data);

                            //Calculate & sets the column width & Row height based on the image size
                            Size imageSize = data.Size;
                            excelRow.Height = imageSize.Height <= 40 ? imageSize.Height : 40;
                            excelRow.CustomHeight = true;
                        }
                    }
                    break;
            }

            if (appendCell && !isImage)
            {
                //  Add a new Excel Cell to our Row 
                var cell = new Cell { CellReference = cellReference, DataType = dataType };
                var cellValue = new CellValue { Text = cellStringValue };
                cell.Append(cellValue);
                excelRow.Append(cell);
            }
        }

        private static void GenerateImagePartContent(ImagePart imagePart, Image image)
        {
            MemoryStream memStream = new MemoryStream();
            image.Save(memStream, ImageFormat.Png);
            memStream.Position = 0;
            imagePart.FeedData(memStream);
            memStream.Close();
        }

        private static Xdr.TwoCellAnchor AddTwoCellAnchor
        (int startRow, int startColumn, int endRow, int endColumn, string imageId)
        {
            Xdr.TwoCellAnchor twoCellAnchor1 = new Xdr.TwoCellAnchor() { EditAs = Xdr.EditAsValues.OneCell };

            Xdr.FromMarker fromMarker1 = new Xdr.FromMarker();
            Xdr.ColumnId columnId1 = new Xdr.ColumnId();
            columnId1.Text = startColumn.ToString();
            Xdr.ColumnOffset columnOffset1 = new Xdr.ColumnOffset();
            columnOffset1.Text = "0";
            Xdr.RowId rowId1 = new Xdr.RowId();
            rowId1.Text = startRow.ToString();
            Xdr.RowOffset rowOffset1 = new Xdr.RowOffset();
            rowOffset1.Text = "0";

            fromMarker1.Append(columnId1);
            fromMarker1.Append(columnOffset1);
            fromMarker1.Append(rowId1);
            fromMarker1.Append(rowOffset1);

            Xdr.ToMarker toMarker1 = new Xdr.ToMarker();
            Xdr.ColumnId columnId2 = new Xdr.ColumnId();
            columnId2.Text = endColumn.ToString();
            Xdr.ColumnOffset columnOffset2 = new Xdr.ColumnOffset();
            columnOffset2.Text = "0";// "152381";
            Xdr.RowId rowId2 = new Xdr.RowId();
            rowId2.Text = endRow.ToString();
            Xdr.RowOffset rowOffset2 = new Xdr.RowOffset();
            rowOffset2.Text = "0";//"152381";

            toMarker1.Append(columnId2);
            toMarker1.Append(columnOffset2);
            toMarker1.Append(rowId2);
            toMarker1.Append(rowOffset2);

            Xdr.Picture picture1 = new Xdr.Picture();

            Xdr.NonVisualPictureProperties nonVisualPictureProperties1 = new Xdr.NonVisualPictureProperties();
            Xdr.NonVisualDrawingProperties nonVisualDrawingProperties1 =
                new Xdr.NonVisualDrawingProperties() { Id = (UInt32Value)2U, Name = "Picture 1" };

            Xdr.NonVisualPictureDrawingProperties nonVisualPictureDrawingProperties1 =
                new Xdr.NonVisualPictureDrawingProperties();
            draw.PictureLocks pictureLocks1 = new draw.PictureLocks() { NoChangeAspect = true };

            nonVisualPictureDrawingProperties1.Append(pictureLocks1);

            nonVisualPictureProperties1.Append(nonVisualDrawingProperties1);
            nonVisualPictureProperties1.Append(nonVisualPictureDrawingProperties1);

            Xdr.BlipFill blipFill1 = new Xdr.BlipFill();

            draw.Blip blip1 = new draw.Blip() { Embed = imageId };
            blip1.AddNamespaceDeclaration("r",
                "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            draw.BlipExtensionList blipExtensionList1 = new draw.BlipExtensionList();

            draw.BlipExtension blipExtension1 = new draw.BlipExtension()
            { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" };

            oDraw.UseLocalDpi useLocalDpi1 = new oDraw.UseLocalDpi() { Val = false };
            useLocalDpi1.AddNamespaceDeclaration("oDraw",
                "http://schemas.microsoft.com/office/drawing/2010/main");

            blipExtension1.Append(useLocalDpi1);

            blipExtensionList1.Append(blipExtension1);

            blip1.Append(blipExtensionList1);

            draw.Stretch stretch1 = new draw.Stretch();
            draw.FillRectangle fillRectangle1 = new draw.FillRectangle();

            stretch1.Append(fillRectangle1);

            blipFill1.Append(blip1);
            blipFill1.Append(stretch1);

            Xdr.ShapeProperties shapeProperties1 = new Xdr.ShapeProperties();

            draw.Transform2D transform2D1 = new draw.Transform2D();
            draw.Offset offset1 = new draw.Offset() { X = 0L, Y = 0L };
            draw.Extents extents1 = new draw.Extents() { Cx = 152381L, Cy = 152381L };

            transform2D1.Append(offset1);
            transform2D1.Append(extents1);

            draw.PresetGeometry presetGeometry1 = new draw.PresetGeometry() { Preset = draw.ShapeTypeValues.Rectangle };
            draw.AdjustValueList adjustValueList1 = new draw.AdjustValueList();

            presetGeometry1.Append(adjustValueList1);

            shapeProperties1.Append(transform2D1);
            shapeProperties1.Append(presetGeometry1);

            picture1.Append(nonVisualPictureProperties1);
            picture1.Append(blipFill1);
            picture1.Append(shapeProperties1);
            Xdr.ClientData clientData1 = new Xdr.ClientData();

            twoCellAnchor1.Append(fromMarker1);
            twoCellAnchor1.Append(toMarker1);
            twoCellAnchor1.Append(picture1);
            twoCellAnchor1.Append(clientData1);

            return twoCellAnchor1;
        }

        private static string GetExcelColumnName(int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            //
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            var firstChar = (char)('A' + (columnIndex / 26) - 1);
            var secondChar = (char)('A' + (columnIndex % 26));

            return $"{firstChar}{secondChar}";
        }
    }
}
