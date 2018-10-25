using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace OCloud.IntegrationTests.Helpers
{
    public class TestConfiguration
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
#if DEBUG
                .AddJsonFile("appsettings.Development.json", optional: true)
#else
                .AddJsonFile("appsettings.json", optional: true)
#endif
                .AddUserSecrets("6cacd92d-92df-4d8a-a7d3-7139ce794011")
                .AddEnvironmentVariables()
                .Build();
        }

        public static IConfigurationRoot GetApplicationConfiguration(string outputPath)
        {
            var iConfig = GetIConfigurationRoot(outputPath);

            //var configuration = new KavaDocsConfiguration();
            //iConfig
            //    .GetSection("KavaDocs")
            //    .Bind(configuration);
            //return configuration;

            return iConfig;
        }

        public TestConfiguration()
        {
            string path = Assembly.GetExecutingAssembly().CodeBase;
            path = Path.GetDirectoryName(path);
            Uri uri = new Uri(path, UriKind.Absolute);
            path = uri.LocalPath;
            Configuration = GetApplicationConfiguration(path);
        }
        

        public IConfigurationRoot Configuration { get; private set; }
    }
}
