using Microsoft.EntityFrameworkCore;
using RL.Data.DataModels;
using RL.Data.DataModels.Common;

namespace RL.Data;
public class RLContext : DbContext
{
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlanProcedure> PlanProcedures { get; set; }
    public DbSet<Procedure> Procedures { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PlanProcedureUser> PlanProcedureUsers { get; set; }

    public RLContext() { }
    public RLContext(DbContextOptions<RLContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PlanProcedure>(typeBuilder =>
        {
            typeBuilder.HasKey(pp => pp.Id);
            typeBuilder.HasOne(pp => pp.Plan).WithMany(p => p.PlanProcedures).HasForeignKey(pp => pp.PlanId);
            typeBuilder.HasOne(pp => pp.Procedure).WithMany().HasForeignKey(pp => pp.ProcedureId);
        });

        var seedData = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "ProcedureSeedData.csv"));
        builder.Entity<Procedure>(p =>
        {
            var seedProcedures = seedData.Select((sd, i) => new Procedure
            {
                ProcedureId = i + 1,
                ProcedureTitle = sd
            });
            p.HasData(seedProcedures);
        });

        builder.Entity<User>(u =>
        {
            u.HasData(new List<User> {
                new User { UserId = 1, Name = "Nick Morrison", CreateDate = new DateTime(1999,12,13), UpdateDate = new DateTime(1999,12,13) },
                new User { UserId = 2, Name = "Scott Cassidy", CreateDate = new DateTime(1999,12,13), UpdateDate = new DateTime(1999,12,13) },
                new User { UserId = 3, Name = "Tony Bidner", CreateDate = new DateTime(1999,12,13), UpdateDate = new DateTime(1999,12,13) },
                new User { UserId = 4, Name = "Patryk Skwarko", CreateDate = new DateTime(1999,12,13), UpdateDate = new DateTime(1999,12,13) }
            });
        });

        builder.Entity<PlanProcedureUser>(typeBuilder =>
        {
            typeBuilder.HasKey(ppu => ppu.Id);
            typeBuilder.HasOne(ppu => ppu.PlanProcedure)
                       .WithMany(pp => pp.PlanProcedureUsers)
                       .HasForeignKey(ppu => ppu.PlanProcedureId)
                       .OnDelete(DeleteBehavior.Cascade);
            typeBuilder.HasOne(ppu => ppu.User)
                       .WithMany()
                       .HasForeignKey(ppu => ppu.UserId)
                       .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        AddTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries().Where(x => x.Entity is IChangeTrackable &&
            (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
                ((IChangeTrackable)entity.Entity).CreateDate = DateTime.UtcNow;

            ((IChangeTrackable)entity.Entity).UpdateDate = DateTime.UtcNow;
        }
    }
}
