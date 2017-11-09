namespace BoardGameClubEntities
{

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data.Entity.Infrastructure;

    public class GameContext : DbContext
    {
        public GameContext()
            : base("Server = tcp:bghq.database.windows.net, 1433; Initial Catalog = bghq; Persist Security Info=False;User ID = petercj; Password=yaboiPeber321;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30; MultipleActiveResultSets=true;")
        {
            Database.SetInitializer<GameContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Entity<AspNetUserLogin>()
                .HasKey(l => new { l.UserId, l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<Player>()
                .HasKey(l => l.Id);
            //modelBuilder.Entity<Player>()
            //    .HasOptional(p => p.AspNetUser)
            //    .WithMany()
            //    .HasForeignKey(a => a.AspNetUser_Id);

        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<BoardGame> BoardGames { get; set; }
        public virtual DbSet<Library> Libraries { get; set; }
        public virtual DbSet<Medal> Medals { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<PlaySession> PlaySessions { get; set; }

    }
}
