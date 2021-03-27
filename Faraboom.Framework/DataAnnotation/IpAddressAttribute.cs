using System.Collections.Generic;
using System.Linq;

namespace Faraboom.Framework.DataAnnotation
{
    public sealed class IpAddressAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return true;

            var lst = value as List<string>;
            return lst?.All(Validate) ?? Validate(value.ToString());
        }

        private static bool Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;

            var splitValues = value.Split('.');
            if (splitValues.Length != 4)
                return false;

            byte tmp;
            return splitValues.All(r => byte.TryParse(r, out tmp));
        }
    }
}