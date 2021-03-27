using Faraboom.Framework.Core;

namespace Faraboom.Framework.DataAnnotation
{
    public sealed class NationalCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return true;

            return Globals.ValidateNationalCode(value.ToString());
        }
    }
}