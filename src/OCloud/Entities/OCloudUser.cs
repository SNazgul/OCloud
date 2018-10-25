using Microsoft.AspNetCore.Identity;


namespace OCloud.Entities
{
    // Add profile data for application users by adding properties to the OCloudUser class
    public class OCloudUser : IdentityUser
    {
        public OCloudUser(string userName) :base(userName)
        {
        }
    }
}
