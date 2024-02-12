using ElectoralSystem.Models.Common;
using ElectoralSystem.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralSystem.Data
{
    public class ElectionDbContext : IdentityDbContext
    {
        public ElectionDbContext(DbContextOptions<ElectionDbContext> options) : base(options)
        {
        }

        //public DbSet<Seat> Seats { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<PartySymbol> Symbols { get; set; }

        public DbSet<VotingSession> Sessions { get; set; }
        public DbSet<ElectionResult> Results { get; set; }

        public DbSet<ElectionCommissioner> ElectionCommisioner { get; set; }

        public DbSet<Candidate> Candidates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Candidate>()
            //.HasMany(s => s.Seats)
            //.WithMany(c => c.Candidates);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "ec";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "ec";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<ElectoralSystem.Models.Entities.VotingSession> VotingSession { get; set; } = default!;
    }
}
