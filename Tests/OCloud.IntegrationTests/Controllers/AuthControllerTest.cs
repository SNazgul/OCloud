using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using static OCloud.Controllers.AuthController;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using OCloud.IntegrationTests.Helpers;

namespace OCloud.IntegrationTests.Controllers
{
    public class AuthControllerTest : IClassFixture<WebApplicationFactory<OCloud.Startup>>, IClassFixture<TestConfiguration>
    {
        private readonly WebApplicationFactory<OCloud.Startup> _factory;
        private readonly TestConfiguration _testConfig;

        public AuthControllerTest(WebApplicationFactory<OCloud.Startup> factory, TestConfiguration testConfig)
        {
            _factory = factory;
            //_factory.ClientOptions.AllowAutoRedirect = false;
            _testConfig = testConfig;
        }

        [Fact]
        public async Task CreateToken_ForLocalAdminFromLocalHost_SuccessfullyLogin()
        {
            // Arrange
            var client = _factory.CreateClient();
            
            LoginModel loginModel = new LoginModel { Username = "LocalAdmin", Password = "pwd12345" };

            // Act
            var response = await client.PostAsJsonAsync("/api/auth/login", loginModel);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            var resultDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
            Assert.True(resultDict.ContainsKey("token"));
        }

        [Fact]
        public async Task CreateToken_FromLocalHost_SuccessfullyLogin()
        {
            // Arrange
            var client = _factory.CreateClient();
            LoginModel loginModel = new LoginModel { Username = _testConfig.Configuration["LocalAdmin:username"], Password = _testConfig.Configuration["LocalAdmin:password"] };

            // Act
            var response = await client.PostAsJsonAsync("/api/auth/login", loginModel);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            var resultDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
            Assert.True(resultDict.ContainsKey("token"));
        }
    }
}
