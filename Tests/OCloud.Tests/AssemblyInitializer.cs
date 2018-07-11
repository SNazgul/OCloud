using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace OCloud.Tests
{
    [TestClass]
    public class AssemblyInitializer
    {
        static TestServer _testServer;
        static HttpClient _httpClient;


        public static HttpClient Client => _httpClient;

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            var rootPath = GetContentRootPath();
            var builder = new WebHostBuilder()
                    .UseContentRoot(rootPath)
                    .UseEnvironment("Development")
                    .UseConfiguration(new ConfigurationBuilder()
                        .SetBasePath(rootPath)
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile("appsettings.Development.json")
                        .Build()
                        )
                    .UseStartup<Startup>() // Uses Start up class from your API Host project to configure the test server
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    })
                    .UseNLog();  // NLog: setup NLog for Dependency injection;  

            _testServer = new TestServer(builder);
            _httpClient = _testServer.CreateClient();
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
        

        static private string GetContentRootPath()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"..\..\..\..\..\OCloud\";
            return Path.Combine(testProjectPath, relativePathToHostProject);
        }        
    }
}
