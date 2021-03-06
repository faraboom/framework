namespace Faraboom.Framework.Data
{
    using System.Collections.Generic;

    public abstract class DbProviderFactory
    {
        protected abstract IReadOnlyDictionary<DataType, string> Mapper { get; }

        public string GetObjectName(string name, string prefix = null, bool pluralize = true)
        {
            if (pluralize)
            {
                name = PluralizeService.Core.PluralizationProvider.Pluralize(name);
            }

            return GetObjectNameInternal(name, prefix);
        }

        public string GetColumnTypeName(DataType dataType)
        {
            return Mapper[dataType];
        }

        protected abstract string GetObjectNameInternal(string name, string prefix = null);
    }
}
