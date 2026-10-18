using Microsoft.EntityFrameworkCore;
using MovieStore.Models;

namespace MovieStore.Data
{
    public class MovieStoreContext : DbContext
    {
        public MovieStoreContext(DbContextOptions<MovieStoreContext> options)
            : base(options)
        {
        }

        // EF Core uses this context to communicate with the database.
        // Keep this simple: one DbSet for Movies used by the controllers.
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure SQL decimal precision for Price to avoid rounding issues.
            modelBuilder.Entity<Movie>()
                .Property(m => m.Price)
                .HasPrecision(18, 2);
        }
    }
}