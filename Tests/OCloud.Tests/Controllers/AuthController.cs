using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using static OCloud.Controllers.AuthController;

namespace OCloud.Tests.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        [TestMethod]
        public async Task CreateToken_FialedLogin()
        {
            var response1 = await AssemblyInitializer.Client.GetAsync("api/Ping");

            LoginModel lm = new LoginModel() { Username = "absend_user", Password = "1" };
            var response = await AssemblyInitializer.Client.PostAsJsonAsync<LoginModel>("api/auth/login", lm);
            Assert.AreEqual(401, response.StatusCode);
        }

        [TestMethod]
        public async Task Register_FromLocalHist_CreateUser()
        {
            LoginModel user = new LoginModel() { Username = "User1", Password = "12345" };
            var response = await AssemblyInitializer.Client.PostAsJsonAsync("api/auth/register", user);

            
            Assert.AreEqual(401, response.StatusCode);
        }
    }
}
