using System.Collections.Generic;

namespace Faraboom.Framework.Localization
{
    public static class CultureExtensions
    {
        public static IEnumerable<string> GetAtomicValues()
        {
            yield return nameof(Culture.fa);
            yield return nameof(Culture.en);
        }
    }
}
