using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Minify.DAL
{
    public class Utility
    {
        private static IConfigurationRoot _configurationRoot;
        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                if(_configurationRoot == null)
                {
                    SetAppSettings();
                }

                return _configurationRoot;
            }
            set => _configurationRoot = value;
        }

        public static void SetAppSettings()
        {
            IConfigurationBuilder configbuilder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            ConfigurationRoot = configbuilder.Build();
        }
        /// <summary>
        /// Gets the connection string based on the environment setting in appsettings.json
        /// </summary>
        /// <returns>The connection string</returns>
        public static string GetConnectionString()
        {
            string connectionStringName = ConfigurationRoot.GetSection("Environment")["IsDevelopment"] == "false" ? "Production" : "Development";

            return ConfigurationRoot.GetConnectionString(connectionStringName);
        }
    }
}
