namespace Faraboom.Framework.Core.Utils.Export
{
    using System;

    using Faraboom.Framework.Data;
    using Faraboom.Framework.DataAnnotation;
    using Faraboom.Framework.Service.Factory;

    using Microsoft.AspNetCore.Mvc;

    [Injectable]
    public abstract class ExportBase : IProvider<Constants.ExportType>
    {
        public abstract Constants.ExportType ProviderType { get; }

        public abstract string Extension { get; }

        protected static string Key => "F@raB00m";

        public abstract FileContentResult Export(GridDataSource gridDataSource, ISearch search, string actionName = null);

        protected string GenerateFileName(string actionName)
        {
            return "FaraboomReport-" + actionName + "-" + DateTime.Now + Extension;
        }
    }
}
