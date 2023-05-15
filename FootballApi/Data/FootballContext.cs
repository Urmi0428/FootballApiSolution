using FootballApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;


namespace FootballApi.Data
{
    public class FootballContext :DbContext
    {
      
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }

        public FootballContext(DbContextOptions<FootballContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            if (_httpContextAccessor.HttpContext != null)
            {
                //We have a HttpContext, but there might not be anyone Authenticated
                UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
                UserName ??= "Unknown";
            }
            else
            {
                //No HttpContext so seeding data
                UserName = "Seed Data";
            }
        }
        public DbSet<Player> Players { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<PlayerTeam> PlayerTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
            .HasIndex(p => p.EMail)
            .IsUnique();

            modelBuilder.Entity<League>()
                .HasKey(p => p.ID);

            modelBuilder.Entity<PlayerTeam>()
           .HasKey(pt => new { pt.TeamId, pt.PlayerId });

            modelBuilder.Entity<League>()
                .HasMany<Team>(p => p.Teams)
                .WithOne(t => t.League)
                .HasForeignKey(t => t.LeagueID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Team>()
                .HasMany<PlayerTeam>(t => t.PlayerTeams)
                .WithOne(t => t.Team)
                .HasForeignKey(pt => pt.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                  .HasMany<PlayerTeam>(t => t.PlayerTeams)
                  .WithOne(t => t.Player)
                  .HasForeignKey(pt => pt.PlayerId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }
    }
    
}
