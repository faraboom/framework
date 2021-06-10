namespace Faraboom.Framework.Data
{
    using Faraboom.Framework.Core;

    public class DbProviderFactories
    {
        private static DbProviderFactory factory;

        public static DbProviderFactory GetFactory
        {
            get
            {
                if (factory != null)
                {
                    return factory;
                }

                switch (Globals.ProviderType)
                {
                    case ProviderType.SqlServer:
                        factory = new SqlServerProvider();
                        break;
                    case ProviderType.DevartOracle:
                        factory = new DevartOracleProvider();
                        break;
                    default:
                        throw new System.NotSupportedException(Globals.ProviderType.ToString());
                }

                return factory;
            }
        }
    }
}
