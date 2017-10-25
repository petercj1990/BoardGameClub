using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BoardGameClub.Models
{
  // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class ApplicationUser : IdentityUser
  {
    public string Department { get; set; }
    //public virtual List<Boardgame> Collection { get; set;}        
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
    public DbSet<Player> Players { get; set; }
    public DbSet<Medal> Medal { get; set; }
    public DbSet<BoardGame> Boardgames { get; set; }
    public DbSet<PlaySession> PlaySessions { get; set; }
    public DbSet<Library> Libraries { get; set; }

    // ... your custom DbSets
    public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
    {
          Database.SetInitializer<ApplicationDbContext>(null); // Remove default initializer
          //Configuration.LazyLoadingEnabled = false;
          //Configuration.ProxyCreationEnabled = false;
    }
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
      //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
      //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
      modelBuilder.Entity<AspNetUserLogin>().HasKey<string>(l => l.UserId);
      modelBuilder.Entity<AspNetRole>().HasKey<string>(r => r.Id);

    }
    public static ApplicationDbContext Create()
    {
      return new ApplicationDbContext();
    }

  }
}
