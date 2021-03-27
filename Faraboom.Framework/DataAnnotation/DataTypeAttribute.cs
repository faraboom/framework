using System;

namespace Faraboom.Framework.DataAnnotation
{
    public sealed class DataTypeAttribute : System.ComponentModel.DataAnnotations.DataTypeAttribute
    {
        public ElementDataType ElementDataType { get; }

        public DataTypeAttribute(ElementDataType elementDataType)
            : base(elementDataType.ToString())
        {
            ElementDataType = elementDataType;

            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_DataType);

            switch (elementDataType)
            {
                case ElementDataType.Date:
                    DisplayFormat = new DisplayFormatAttribute
                    {
                        DataFormatString = "{0:d}",
                        ApplyFormatInEditMode = true
                    };
                    break;
                case ElementDataType.Time:
                    DisplayFormat = new DisplayFormatAttribute
                    {
                        DataFormatString = "{0:t}",
                        ApplyFormatInEditMode = true
                    };
                    break;
                case ElementDataType.Currency:
                    DisplayFormat = new DisplayFormatAttribute
                    {
                        DataFormatString = "{0:C}"
                    };
                    break;
            }
        }

        public override string GetDataTypeName()
        {
            return ElementDataType.ToString();
        }

        public new Type ErrorMessageResourceType
        {
            get
            {
                return base.ErrorMessageResourceType;
            }
            private set
            {
                base.ErrorMessageResourceType = value;
            }
        }

        public new string ErrorMessageResourceName
        {
            get
            {
                return base.ErrorMessageResourceName;
            }
            private set
            {
                base.ErrorMessageResourceName = value;
            }
        }

        public new string ErrorMessage
        {
            get
            {
                return base.ErrorMessage;
            }
            private set
            {
                base.ErrorMessage = value;
            }
        }

        private DisplayFormatAttribute displayFormat;
        public new DisplayFormatAttribute DisplayFormat
        {
            get
            {
                return displayFormat;
            }
            private set
            {
                base.DisplayFormat = displayFormat = value;
            }
        }
    }
}