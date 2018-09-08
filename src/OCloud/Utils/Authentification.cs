using Microsoft.AspNetCore.Identity;
using OCloud.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OCloud.Utils
{
    class Authentification
    {
        static public async Task<OCloudUser> AuthenticateUser(UserManager<OCloudUser> userManager, string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<OCloudUser>(null);

            // get the user to verifty
            var userToVerify = await userManager.FindByNameAsync(userName);
            if (userToVerify == null)
            {
                return await Task.FromResult<OCloudUser>(null);
            }

            // check the credentials
            if (await userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(userToVerify);
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<OCloudUser>(null);
        }
    }
}
