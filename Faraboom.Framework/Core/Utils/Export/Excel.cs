namespace Faraboom.Framework.Core.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;

    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    using Faraboom.Framework.Converter;
    using Faraboom.Framework.Core.Utils.Export;
    using Faraboom.Framework.Data;
    using Faraboom.Framework.DataAnnotation;
    using Faraboom.Framework.Resources;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc;

    using Border = DocumentFormat.OpenXml.Spreadsheet.Border;
    using Color = DocumentFormat.OpenXml.Spreadsheet.Color;
    using draw = DocumentFormat.OpenXml.Drawing;
    using Drawing = DocumentFormat.OpenXml.Spreadsheet.Drawing;
    using Fonts = DocumentFormat.OpenXml.Spreadsheet.Fonts;
    using oDraw = DocumentFormat.OpenXml.Office2010.Drawing;
    using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;

    public class Excel : ExportBase
    {
        public override Constants.ExportType ProviderType => Constants.ExportType.Excel;

        public override string Extension => ".xlsx";

        public override FileContentResult Export(GridDataSource gridDataSource, ISearch search, string actionName = null)
        {
            var hasSearchItem = false;
            var ds = new DataSet();
            var dt = new DataTable();
            if (gridDataSource.Data?.Count > 0)
            {
                var localizedSearchColumnNames = new Dictionary<string, string>();
                var localizedGridColumnNames = new Dictionary<string, string>();
                bool dataIsDictionaryBase = (gridDataSource.Data[0] as Dictionary<string, string>) != null;
                IEnumerable<PropertyInfo> properties = null;
                if (search != null)
                {
                    hasSearchItem = true;
                    var dtSearch = new DataTable();
                    var searchItemType = search.GetType();
                    properties = searchItemType.GetProperties().Where(t => t.GetCustomAttribute<JsonIgnoreAttribute>() == null && (t.GetCustomAttribute<ExportInfoAttribute>() == null || !t.GetCustomAttribute<ExportInfoAttribute>().Ignore));
                    foreach (var info in properties)
                    {
                        var description = Globals.GetLocalizedDescription(info);
                        var name = description != info.Name ? description : Globals.GetLocalizedDisplayName(info);
                        var columnType = GetNullableType(info.PropertyType);
                        if (columnType.IsGenericType)
                        {
                            if (columnType.GenericTypeArguments.Any())
                            {
                                if (columnType.GenericTypeArguments[0].Name == "ParameterDto")
                                {
                                    columnType = typeof(string);
                                }
                            }
                        }

                        if (columnType == typeof(DateTimeOffset) || columnType == typeof(DateTimeOffset?)
                            || columnType == typeof(DateTime) || columnType == typeof(DateTime?))
                        {
                            columnType = typeof(string);
                        }

                        dtSearch.Columns.Add(new DataColumn(name, columnType));

                        localizedSearchColumnNames.Add(info.Name, name);
                    }

                    var row = dtSearch.NewRow();
                    foreach (var info in properties)
                    {
                        var pureValue = info.GetValue(search, null);
                        if (pureValue != null)
                        {
                            var pureValueList = string.Empty;
                            var converter = info.GetCustomAttribute<JsonConverterAttribute>();
                            if (converter != null)
                            {
                                if (converter.CreateConverter(converter.ConverterType) is IJsonConverter instance && !instance.IgnoreOnExport)
                                {
                                    pureValue = instance.Convert(pureValue);
                                }
                            }

                            var columnName = localizedSearchColumnNames[info.Name];

                            if (pureValue is IList)
                            {
                                var pureValueCount = (pureValue as IList).Count;
                                if (pureValue.GetType().IsGenericType)
                                {
                                    if (pureValue.GetType().GenericTypeArguments.Any())
                                    {
                                        if (pureValue.GetType().GenericTypeArguments[0].Name == "ParameterDto")
                                        {
                                            for (int j = 0; j < pureValueCount; j++)
                                            {
                                                pureValueList += ((pureValue as IList)[j] as dynamic).Value + (j != pureValueCount - 1 ? "," : string.Empty);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < pureValueCount; j++)
                                    {
                                        pureValueList += ((pureValue as IList)[j] as dynamic).Value + (j != pureValueCount - 1 ? "," : string.Empty);
                                    }
                                }

                                pureValue = pureValueList;
                            }

                            row[columnName] = pureValue;
                        }
                    }

                    dtSearch.Rows.Add(row);
                    ds.Tables.Add(dtSearch);
                }

                if (gridDataSource.DataTable != null)
                {
                    dataIsDictionaryBase = true;

                    foreach (DataColumn column in gridDataSource.DataTable.Columns)
                    {
                        if (column.DataType == typeof(HtmlString))
                        {
                            break;
                        }

                        localizedGridColumnNames.Add(column.ColumnName, column.Caption);
                        dt.Columns.Add(new DataColumn(column.Caption, typeof(string)));
                    }
                }
                else
                {
                    if (gridDataSource.Data[0] is Dictionary<string, string> captions)
                    {
                        foreach (string key in captions.Keys)
                        {
                            localizedGridColumnNames.Add(key, key);
                            dt.Columns.Add(new DataColumn(key, typeof(string)));
                        }
                    }
                    else
                    {
                        localizedGridColumnNames = new Dictionary<string, string>();
                        var type = gridDataSource.Data[0].GetType();
                        properties = type.GetProperties().Where(t => t.GetCustomAttribute<JsonIgnoreAttribute>(false) == null && (t.GetCustomAttribute<ExportInfoAttribute>() == null || !t.GetCustomAttribute<ExportInfoAttribute>().Ignore));
                        var i = 0;
                        foreach (var info in properties)
                        {
                            var description = Globals.GetLocalizedDescription(info);
                            var name = description != info.Name ? description : Globals.GetLocalizedDisplayName(info);
                            if (dt.Columns.Contains(name))
                            {
                                name += i;
                            }

                            localizedGridColumnNames.Add(info.Name, name);
                            var columnType = GetNullableType(info.PropertyType);

                            if (columnType == typeof(bool))
                            {
                                var exportInfo = info.GetCustomAttribute<ExportInfoAttribute>();
                                if (exportInfo != null && !string.IsNullOrWhiteSpace(exportInfo.TrueResourceKey))
                                {
                                    columnType = typeof(string);
                                }
                            }

                            if (columnType == typeof(DateTimeOffset) || columnType == typeof(DateTimeOffset?)
                                || columnType == typeof(DateTime) || columnType == typeof(DateTime?))
                            {
                                columnType = typeof(string);
                            }

                            dt.Columns.Add(new DataColumn(name, columnType));
                            i++;
                        }
                    }
                }

                foreach (var t in gridDataSource.Data)
                {
                    var row = dt.NewRow();

                    if (dataIsDictionaryBase)
                    {
                        var dictionaryValues = t as Dictionary<string, string>;
                        foreach (var key in dictionaryValues.Keys)
                        {
                            if (gridDataSource.DataTable.Columns[key].DataType == typeof(HtmlString))
                            {
                                break;
                            }

                            var value = dictionaryValues[key];
                            if (gridDataSource.DataTable != null)
                            {
                                if (gridDataSource.DataTable.Columns[key].ExtendedProperties[nameof(ExportInfoAttribute)] is ExportInfoAttribute exportInfo && !string.IsNullOrWhiteSpace(exportInfo.TrueResourceKey))
                                {
                                    if (exportInfo.ResourceType == null)
                                    {
                                        exportInfo.ResourceType = typeof(GlobalResource);
                                    }

                                    var resourceManager = new System.Resources.ResourceManager(exportInfo.ResourceType);
                                    value = value.ValueOf<bool>() ? resourceManager.GetString(exportInfo.TrueResourceKey) : resourceManager.GetString(exportInfo.FalseResourceKey);
                                }
                            }

                            row[localizedGridColumnNames[key]] = value;
                        }
                    }
                    else
                    {
                        foreach (var info in properties)
                        {
                            var pureValue = info.GetValue(t, null);
                            if (pureValue != null)
                            {
                                var converter = info.GetCustomAttribute<JsonConverterAttribute>();
                                if (converter != null)
                                {
                                    if (converter.CreateConverter(converter.ConverterType) is IJsonConverter instance && !instance.IgnoreOnExport)
                                    {
                                        pureValue = instance.Convert(pureValue);
                                    }
                                }

                                var typeValue = GetNullableTypeValue(info.PropertyType);
                                if (typeValue.IsEnum)
                                {
                                    pureValue = EnumHelper.LocalizeEnum(pureValue);
                                }
                                else if (typeValue == typeof(bool))
                                {
                                    var exportInfo = info.GetCustomAttribute<ExportInfoAttribute>();
                                    if (exportInfo != null && !string.IsNullOrWhiteSpace(exportInfo.TrueResourceKey))
                                    {
                                        if (exportInfo.ResourceType == null)
                                        {
                                            exportInfo.ResourceType = typeof(GlobalResource);
                                        }

                                        var resourceManager = new System.Resources.ResourceManager(exportInfo.ResourceType);
                                        pureValue = (bool)pureValue ? resourceManager.GetString(exportInfo.TrueResourceKey) : resourceManager.GetString(exportInfo.FalseResourceKey);
                                    }
                                }
                            }

                            var columnName = localizedGridColumnNames[info.Name];
                            if (!IsNullableType(info.PropertyType))
                            {
                                row[columnName] = pureValue;
                            }
                            else
                            {
                                row[columnName] = pureValue ?? DBNull.Value;
                            }
                        }
                    }

                    dt.Rows.Add(row);
                }
            }

            ds.Tables.Add(dt);

            var stream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
            {
                WriteExcelFile(ds, document, hasSearchItem);
            }

            stream.Flush();
            stream.Position = 0;

            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();

            return new FileContentResult(data, "application/ms-excel") { FileDownloadName = GenerateFileName(actionName) };
        }

        private static DocumentFormat.OpenXml.Spreadsheet.Columns AutoSize(SheetData sheetData)
        {
            var maxColWidth = GetMaxCharacterWidth(sheetData);

            DocumentFormat.OpenXml.Spreadsheet.Columns columns = new();

            // this is the width of my font - yours may be different
            double maxWidth = 7;
            foreach (var item in maxColWidth)
            {
                // width = Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}]/{Maximum Digit Width}*256)/256
                double width = Math.Truncate(((item.Value * maxWidth) + 5) / maxWidth * 256) / 256;

                // pixels=Truncate(((256 * {width} + Truncate(128/{Maximum Digit Width}))/256)*{Maximum Digit Width})
                double pixels = Math.Truncate(((256 * width) + Math.Truncate(128 / maxWidth)) / 256 * maxWidth);

                // character width=Truncate(({pixels}-5)/{Maximum Digit Width} * 100+0.5)/100
                double charWidth = Math.Truncate(((pixels - 5) / maxWidth * 100) + 0.5) / 100;

                DocumentFormat.OpenXml.Spreadsheet.Column col = new() { BestFit = true, Min = (uint)(item.Key + 1), Max = (uint)(item.Key + 1), CustomWidth = true, Width = width };
                columns.Append(col);
            }

            return columns;
        }

        private static Dictionary<int, int> GetMaxCharacterWidth(SheetData sheetData)
        {
            // iterate over all cells getting a max char value for each column
            Dictionary<int, int> maxColWidth = new Dictionary<int, int>();
            var rows = sheetData.Elements<Row>();
            uint[] numberStyles = new uint[] { 5, 6, 7, 8 }; // styles that will add extra chars
            uint[] boldStyles = new uint[] { 1, 2, 3, 4, 6, 7, 8 }; // styles that will bold
            foreach (var r in rows)
            {
                var cells = r.Elements<Cell>().ToArray();

                // using cell index as my column
                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    var cellValue = cell.CellValue == null ? string.Empty : cell.CellValue.InnerText;
                    var cellTextLength = cellValue.Length;

                    if (cell.StyleIndex != null && numberStyles.Contains(cell.StyleIndex))
                    {
                        int thousandCount = (int)Math.Truncate((double)cellTextLength / 4);

                        // add 3 for '.00'
                        cellTextLength += 3 + thousandCount;
                    }

                    if (cell.StyleIndex != null && boldStyles.Contains(cell.StyleIndex))
                    {
                        // add an extra char for bold - not 100% acurate but good enough for what i need.
                        cellTextLength += 1;
                    }

                    if (maxColWidth.ContainsKey(i))
                    {
                        var current = maxColWidth[i];
                        if (cellTextLength > current)
                        {
                            maxColWidth[i] = cellTextLength;
                        }
                    }
                    else
                    {
                        maxColWidth.Add(i, cellTextLength);
                    }
                }
            }

            return maxColWidth;
        }

        private static Type GetNullableTypeValue(Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Nullable.GetUnderlyingType(t);
            }

            return t;
        }

        private static Type GetNullableType(Type t)
        {
            var returnType = GetNullableTypeValue(t);
            if (returnType.IsEnum)
            {
                returnType = typeof(string);
            }

            return returnType;
        }

        private static bool IsNullableType(Type type)
        {
            return type == typeof(string) ||
                         type.IsArray ||
                         (type.IsGenericType &&
                            type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        private static void WriteExcelFile(DataSet ds, SpreadsheetDocument spreadsheet, bool hasSearchItem)
        {
            WorkbookPart workbookPart = spreadsheet.AddWorkbookPart();

            workbookPart.Workbook = new Workbook
            {
                WorkbookProtection = new WorkbookProtection
                {
                    LockStructure = true,
                },
            };
            workbookPart.Workbook.Append(new BookViews(new WorkbookView()));
            uint worksheetNumber = 1;

            WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>("IdSheetPart" + worksheetNumber.ToString());

            newWorksheetPart.Worksheet =
                    new Worksheet(
                        new SheetViews(new SheetView { WorkbookViewId = 0, RightToLeft = Globals.IsRtl }),
                        new SheetData());

            var stylePart = workbookPart.AddNewPart<WorkbookStylesPart>("IdStyles");

            stylePart.Stylesheet = GenerateStylesheet();
            stylePart.Stylesheet.CellFormats.Count = UInt32Value.FromUInt32((uint)stylePart.Stylesheet.CellFormats.ChildElements.Count);
            stylePart.Stylesheet.Save();
            if (worksheetNumber == 1)
            {
                workbookPart.Workbook.AppendChild(new Sheets());
            }

            workbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet
            {
                Id = workbookPart.GetIdOfPart(newWorksheetPart),
                SheetId = worksheetNumber,
                Name = "Excel Report",
            });

            uint rowIndex = 0;
            var isFirstTable = true;

            foreach (DataTable dt in ds.Tables)
            {
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart, hasSearchItem, isFirstTable, ref rowIndex);
                newWorksheetPart.Worksheet.Save();

                string drawingID = "IdDrawingsPart";

                if (newWorksheetPart.DrawingsPart != null
                && newWorksheetPart.DrawingsPart.WorksheetDrawing != null)
                {
                    Drawing drawing1 = new Drawing() { Id = drawingID };
                    newWorksheetPart.Worksheet.Append(drawing1);
                }

                isFirstTable = false;
                worksheetNumber++;
            }

            foreach (var worksheetPart in spreadsheet.WorkbookPart.WorksheetParts)
            {
                string hexConvertedPassword = HashPassword(Key);
                SheetProtection sheetProt = new()
                {
                    Sheet = true,
                    Objects = true,
                    FormatColumns = false,
                    Scenarios = true,
                    Password = hexConvertedPassword,
                };
                worksheetPart.Worksheet.InsertAfter(sheetProt, worksheetPart.Worksheet.Descendants<SheetData>().LastOrDefault());
                worksheetPart.Worksheet.Save();
            }

            // workbookPart.Workbook.Save();
        }

        private static string HashPassword(string password)
        {
            byte[] passwordCharacters = System.Text.Encoding.ASCII.GetBytes(password);
            int hash = 0;
            if (passwordCharacters.Length > 0)
            {
                int charIndex = passwordCharacters.Length;

                while (charIndex-- > 0)
                {
                    hash = ((hash >> 14) & 0x01) | ((hash << 1) & 0x7fff);
                    hash ^= passwordCharacters[charIndex];
                }

                hash = ((hash >> 14) & 0x01) | ((hash << 1) & 0x7fff);
                hash ^= passwordCharacters.Length;
                hash ^= 0x8000 | ('N' << 8) | 'K';
            }

            return Convert.ToString(hash, 16).ToUpperInvariant();
        }

        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart, bool hasSearchItem, bool isFirstTable, ref uint rowIndexTemp)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();
            var numberOfColumns = dt.Columns.Count;
            var columnRenderType = new CellValues[numberOfColumns];

            var rowIndex = hasSearchItem ? 3U : 2;
            var excelColumnNames = new string[numberOfColumns];
            if (rowIndexTemp != 0)
            {
                rowIndex = rowIndexTemp + 2;
            }

            for (var i = 0; i < numberOfColumns; i++)
            {
                excelColumnNames[i] = GetExcelColumnName(rowIndex == 3 && hasSearchItem ? i + 1 : i);
            }

            var headerRow = new Row { RowIndex = rowIndex };
            sheetData.Append(headerRow);
            for (var i = 0; i < numberOfColumns; i++)
            {
                // if (i == 0 && isFirstTable) { }
                // AppendCell("B2", "logo-admin.png", headerRow, true, CellValues.String, worksheetPart, i, 2);
                var col = dt.Columns[i];
                AppendCell(excelColumnNames[i] + rowIndex.ToString(), col.ColumnName, headerRow, true, CellValues.String, worksheetPart, i, (int)rowIndex);
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

            foreach (DataRow dr in dt.Rows)
            {
                var newExcelRow = new Row { RowIndex = ++rowIndex };
                sheetData.Append(newExcelRow);

                for (var colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    var cellValue = dr.ItemArray[colInx].ToString();
                    AppendCell(excelColumnNames[colInx] + rowIndex, cellValue, newExcelRow, false, columnRenderType[colInx],
                            worksheetPart, colInx, (int)rowIndex);
                }
            }

            rowIndexTemp = rowIndex;
        }

        private static void AppendCell(string cellReference, string cellStringValue, Row excelRow, bool isHeaderRow, CellValues dataType, WorksheetPart worksheetPart, int colInx, int rowIndex)
        {
            var isImage = false;
            var appendCell = true;
            switch (dataType)
            {
                case CellValues.Boolean:
                    {
                        if (bool.TryParse(cellStringValue, out bool cellBooleanValue))
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
                        if (double.TryParse(cellStringValue, out double cellNumericValue))
                        {
                            cellStringValue = cellNumericValue.ToString();
                        }
                        else
                        {
                            appendCell = false;
                        }
                    }

                    break;
                case CellValues.String:
                    {
                        if (cellStringValue.StartsWith("data:image/") /*|| cellStringValue.Equals("logo-admin.png")*/)
                        {
                            isImage = true;

                            var contentType = "image/png";
                            var imageFormat = ImageFormat.Png;
                            if (cellStringValue.StartsWith("data:image/jpeg"))
                            {
                                contentType = "image/jpeg";
                                imageFormat = ImageFormat.Jpeg;
                            }

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

                            var imageId = "Id" + rowIndex;

                            Xdr.TwoCellAnchor cellAnchor = AddTwoCellAnchor(rowIndex - 1, colInx, rowIndex, colInx + 1, imageId);

                            worksheetDrawing.Append(cellAnchor);

                            byte[] imageBytes = null;

                            // if (cellStringValue.Equals("logo-admin.png"))
                            // {
                            // var request = System.Web.HttpContext.Current.Request;
                            // var appBaseUrl = string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority);

                            // var imageUrl = string.Format("{0}/images/Tenants/BoomMarket/{1}", appBaseUrl, cellStringValue);

                            // using (WebResponse wrFileResponse = WebRequest.Create(imageUrl).GetResponse())
                            // {
                            // using (Stream objWebStream = wrFileResponse.GetResponseStream())
                            // {
                            // MemoryStream ms = new MemoryStream();
                            // objWebStream.CopyTo(ms, 8192);
                            // imageBytes = ms.ToArray();
                            // }
                            // }
                            // }
                            // else
                            // {
                            cellStringValue = cellStringValue.Split(' ')[1];
                            imageBytes = Convert.FromBase64String(cellStringValue);

                            // }
                            using var ms = new MemoryStream(imageBytes);
                            var image = System.Drawing.Image.FromStream(ms);

                            var stream = new MemoryStream();
                            image.Save(stream, imageFormat);
                            stream.Position = 0;

                            ImagePart imagePart = drawingsPart.AddNewPart<ImagePart>(contentType, imageId);
                            imagePart.FeedData(stream);

                            excelRow.Height = image.Size.Height <= 40 ? image.Size.Height : 40;
                            excelRow.CustomHeight = true;
                        }
                    }

                    break;
            }

            if (appendCell && !isImage)
            {
                var cell = new Cell { CellReference = cellReference, DataType = dataType };
                var cellValue = new CellValue { Text = cellStringValue };
                cell.Append(cellValue);
                if (isHeaderRow)
                {
                    cell.StyleIndex = 2U;
                }
                else
                {
                    cell.StyleIndex = 1U;
                }

                excelRow.Height = 20;
                excelRow.CustomHeight = true;
                excelRow.Append(cell);
            }
        }

        private static Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new(
                    new DocumentFormat.OpenXml.Spreadsheet.Font(// Index 0 - default
                            new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 }),
                    new DocumentFormat.OpenXml.Spreadsheet.Font(// Index 1 - header
                            new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
                            new DocumentFormat.OpenXml.Spreadsheet.Bold(),
                            new Color() { Rgb = "000000" }));

            Fills fills = new Fills(
                            new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                            new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 1 - default
                            new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "CCCCCCCC" } })
                            { PatternType = PatternValues.Solid })); // Index 2 - header

            Borders borders = new Borders(
                            new Border(), // index 0 default
                            new Border(// index 1 black border
                                    new DocumentFormat.OpenXml.Spreadsheet.LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                                    new DocumentFormat.OpenXml.Spreadsheet.RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                                    new DocumentFormat.OpenXml.Spreadsheet.TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                                    new DocumentFormat.OpenXml.Spreadsheet.BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                                    new DiagonalBorder()));

            CellFormats cellFormats = new CellFormats(
                            new CellFormat() { ApplyBorder = true }, // default
                            new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, ApplyProtection = true, Protection = new Protection() { Locked = true } }, // body
                            new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyBorder = true, ApplyFill = true, ApplyProtection = true, Protection = new Protection() { Locked = true } }); // header

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

        private static Xdr.TwoCellAnchor AddTwoCellAnchor(int startRow, int startColumn, int endRow, int endColumn, string imageId)
        {
            Xdr.TwoCellAnchor twoCellAnchor1 = new Xdr.TwoCellAnchor() { EditAs = Xdr.EditAsValues.OneCell };

            Xdr.FromMarker fromMarker1 = new Xdr.FromMarker();
            Xdr.ColumnId columnId1 = new Xdr.ColumnId
            {
                Text = startColumn.ToString(),
            };
            Xdr.ColumnOffset columnOffset1 = new Xdr.ColumnOffset
            {
                Text = "0",
            };
            Xdr.RowId rowId1 = new Xdr.RowId
            {
                Text = startRow.ToString(),
            };
            Xdr.RowOffset rowOffset1 = new Xdr.RowOffset
            {
                Text = "0",
            };

            fromMarker1.Append(columnId1);
            fromMarker1.Append(columnOffset1);
            fromMarker1.Append(rowId1);
            fromMarker1.Append(rowOffset1);

            Xdr.ToMarker toMarker1 = new Xdr.ToMarker();
            Xdr.ColumnId columnId2 = new Xdr.ColumnId
            {
                Text = endColumn.ToString(),
            };
            Xdr.ColumnOffset columnOffset2 = new Xdr.ColumnOffset
            {
                Text = "0", // "152381";
            };
            Xdr.RowId rowId2 = new Xdr.RowId
            {
                Text = endRow.ToString(),
            };
            Xdr.RowOffset rowOffset2 = new Xdr.RowOffset
            {
                Text = "0", // "152381";
            };

            toMarker1.Append(columnId2);
            toMarker1.Append(columnOffset2);
            toMarker1.Append(rowId2);
            toMarker1.Append(rowOffset2);

            Xdr.Picture picture1 = new Xdr.Picture();

            Xdr.NonVisualPictureProperties nonVisualPictureProperties1 = new Xdr.NonVisualPictureProperties();
            Xdr.NonVisualDrawingProperties nonVisualDrawingProperties1 =
                    new Xdr.NonVisualDrawingProperties() { Id = 2U, Name = "Picture 1" };

            Xdr.NonVisualPictureDrawingProperties nonVisualPictureDrawingProperties1 =
                    new Xdr.NonVisualPictureDrawingProperties();
            draw.PictureLocks pictureLocks1 = new draw.PictureLocks() { NoChangeAspect = true };

            nonVisualPictureDrawingProperties1.Append(pictureLocks1);

            nonVisualPictureProperties1.Append(nonVisualDrawingProperties1);
            nonVisualPictureProperties1.Append(nonVisualPictureDrawingProperties1);

            Xdr.BlipFill blipFill1 = new Xdr.BlipFill();

            draw.Blip blip1 = new draw.Blip() { Embed = imageId };
            blip1.AddNamespaceDeclaration(
                "r",
                "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            draw.BlipExtensionList blipExtensionList1 = new draw.BlipExtensionList();

            draw.BlipExtension blipExtension1 = new draw.BlipExtension()
            { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" };

            oDraw.UseLocalDpi useLocalDpi1 = new oDraw.UseLocalDpi() { Val = false };
            useLocalDpi1.AddNamespaceDeclaration(
                "oDraw",
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
            // Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            if (columnIndex < 26)
            {
                return ((char)('A' + columnIndex)).ToString();
            }

            var firstChar = (char)('A' + (columnIndex / 26) - 1);
            var secondChar = (char)('A' + (columnIndex % 26));

            return $"{firstChar}{secondChar}";
        }
    }
}
