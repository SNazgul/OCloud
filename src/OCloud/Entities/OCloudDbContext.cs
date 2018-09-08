using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OCloud.Entities
{
    public class OCloudDbContext : IdentityDbContext<OCloudUser>
    {
        public OCloudDbContext(DbContextOptions<OCloudDbContext> options)
            : base(options)
        {
        }

        public DbSet<FileInfo> FileInfo { get; set; }

        public DbSet<CloudInfo> CloudInfo { get; set; }
    }
}
