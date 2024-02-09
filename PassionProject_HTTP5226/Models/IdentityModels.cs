using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PassionProject_HTTP5226.Models
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
        }

        //Add Player Entity
        public DbSet<Player> Players {  get; set; }

        //Add Team Entity
        public DbSet<Team> Teams {  get; set; }

        //Add Result Entity
        public DbSet<Result> Results { get; set; }

        //Add ResultTeam Entity
        public DbSet<ResultTeam> ResultTeams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResultTeam>()
                .HasRequired(rt => rt.Result)
                .WithMany(r => r.ResultTeams)
                .HasForeignKey(rt => rt.ResultId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ResultTeam>()
                .HasRequired(rt => rt.Team1)
                .WithMany()
                .HasForeignKey(rt => rt.Team1Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ResultTeam>()
                .HasRequired(rt => rt.Team2)
                .WithMany()
                .HasForeignKey(rt => rt.Team2Id)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}