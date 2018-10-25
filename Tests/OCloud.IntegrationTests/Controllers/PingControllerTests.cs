using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace OCloud.IntegrationTests.Controllers
{
    public class PingControllerTests : IClassFixture<WebApplicationFactory<OCloud.Startup>>
    {
        private readonly WebApplicationFactory<OCloud.Startup> _factory;

        public PingControllerTests(WebApplicationFactory<OCloud.Startup> factory)
        {
            _factory = factory;
            _factory.ClientOptions.AllowAutoRedirect = false;
        }

        [Fact]
        public async Task PingTest()
        {
            // Arrange
            var client = _factory.CreateClient(
            //    new WebApplicationFactoryClientOptions
            //{
            //    AllowAutoRedirect = false
            //}
                );

            // Act
            var response = await client.GetAsync("/api/ping");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Pong", result, StringComparison.OrdinalIgnoreCase);
        }
    }    
}
