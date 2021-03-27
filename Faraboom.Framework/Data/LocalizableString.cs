using System.Collections.Generic;

namespace Faraboom.Framework.Data
{
    public class LocalizableString
    {
        public string Value { get; set; }

        public List<LocalizedValue> LocalizedValues { get; set; }

        public override string ToString()
        {
            return Value;
        }

        public struct LocalizedValue
        {
            public short Id { get; set; }

            public string Code { get; set; }

            public string Value { get; set; }
        }
    }
}
