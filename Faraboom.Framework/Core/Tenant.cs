using System.Collections.Generic;

namespace Faraboom.Framework.Core
{
    public class Tenant
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Schema { get; set; }

        public string ArchiveSchema { get; set; }

        public List<string> Domains { get; set; }
    }
}
