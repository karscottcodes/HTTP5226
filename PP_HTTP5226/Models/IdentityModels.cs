using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PP_HTTP5226.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.LazyLoadingEnabled = true;
        }

        // Add Team Entity
        public DbSet<Team> Teams { get; set; }

        // Add Player Entity
        public DbSet<Player> Players { get; set; }

        // Add Result Entity
        public DbSet<Result> Results { get; set; }

        // Configure relationships between Result and Team1, Result and Team2
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationship between Result and Team1
            modelBuilder.Entity<Result>()
                .HasRequired(r => r.Team1)
                .WithMany()
                .HasForeignKey(r => r.Team1Id)
                .WillCascadeOnDelete(false);

            // Configure relationship between Result and Team2
            modelBuilder.Entity<Result>()
                .HasRequired(r => r.Team2)
                .WithMany()
                .HasForeignKey(r => r.Team2Id)
                .WillCascadeOnDelete(false);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}