using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OCloud.Entities
{
    public class OCloudDbContext : IdentityDbContext
    {
        public OCloudDbContext(DbContextOptions<OCloudDbContext> options)
            : base(options)
        {
        }
    }
}
