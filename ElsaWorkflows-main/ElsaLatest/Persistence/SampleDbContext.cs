using Elsa.Persistence.EntityFramework.Core;
using Elsa.Samples.UserRegistration.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Samples.UserRegistration.Web.Persistence
{
    public class SampleDbContext : ElsaContext
    {
        public SampleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
