using Microsoft.Extensions.Configuration;

using System.IO;

namespace Minify.DAL
{
    public class Configuraion
    {
        private static IConfigurationRoot _root;
        public static IConfigurationRoot Root
        {
            get
            {
                if (_root == null)
                {
                    Add();
                }

                return _root;
            }
            set => _root = value;
        }

        public static void Add()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

#if DEBUG
            builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#else
            builder.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
#endif
            _root = builder.Build();
        }
        /// <summary>
        /// Gets the connection string based on the environment setting in appsettings.json
        /// </summary>
        /// <returns>The connection string</returns>
        public static string GetConnectionString()
        {
            return Root.GetConnectionString("MSSQL");
        }
    }
}
