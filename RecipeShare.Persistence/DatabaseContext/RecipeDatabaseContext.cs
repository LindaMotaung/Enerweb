using RecipeShare.Domain;
using RecipeShare.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace RecipeShare.Persistence.DatabaseContext
{
    public class RecipeDatabaseContext : DbContext
    {
        public RecipeDatabaseContext() { }

        public RecipeDatabaseContext(DbContextOptions<RecipeDatabaseContext> options)
            : base(options) { }

        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<DietaryTag> DietaryTags { get; set; }
        public virtual DbSet<RecipeDietaryTag> RecipeDietaryTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Replace with your actual connection string
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=db_recipe;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeDatabaseContext).Assembly);

            // Recipe
            modelBuilder.Entity<Recipe>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            // DietaryTag
            modelBuilder.Entity<DietaryTag>()
                .HasKey(dt => dt.Id);

            modelBuilder.Entity<DietaryTag>()
                .Property(dt => dt.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DietaryTag>()
                .Property(dt => dt.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<DietaryTag>().HasData(
                new DietaryTag { Id = 2, Name = "Keto" },
                new DietaryTag { Id = 3, Name = "Vegan" }
            );

            modelBuilder.Entity<RecipeDietaryTag>().HasData(
                new RecipeDietaryTag { RecipeId = 1, DietaryTagId = 2 },
                new RecipeDietaryTag { RecipeId = 1, DietaryTagId = 3 }
            );


            // RecipeDietaryTag (Join table)
            modelBuilder.Entity<RecipeDietaryTag>()
                .HasKey(rdt => new { rdt.RecipeId, rdt.DietaryTagId });

            modelBuilder.Entity<RecipeDietaryTag>()
                .HasOne(rdt => rdt.Recipe)
                .WithMany(r => r.RecipeDietaryTags)
                .HasForeignKey(rdt => rdt.RecipeId);

            modelBuilder.Entity<RecipeDietaryTag>()
                .HasOne(rdt => rdt.DietaryTag)
                .WithMany(dt => dt.RecipeDietaryTags)
                .HasForeignKey(rdt => rdt.DietaryTagId);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                entry.Entity.DateModified = DateTime.UtcNow;
                entry.Entity.ModifiedBy = "Jamie Oliver";
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "Gordon Ramsey";
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
