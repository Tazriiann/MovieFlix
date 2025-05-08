using Microsoft.EntityFrameworkCore;
using MoviePlatform.Models;

public class AppDbContext : DbContext
{
    // Constructor to pass the DbContextOptions to the base DbContext class
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSet for User and Movie models
    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }

    // Configure model relationships and seed initial data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed default users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Password = "admin123", Role = "Admin" },
            new User { Id = 2, Username = "user", Password = "user123", Role = "User" }
        );

        // You can also add additional seeding for Movies or other entities if needed
        // For example:
        // modelBuilder.Entity<Movie>().HasData(
        //     new Movie { Id = 1, Title = "Inception", Genre = "Sci-Fi", ReleaseDate = new DateTime(2010, 7, 16), Description = "A mind-bending thriller" },
        //     new Movie { Id = 2, Title = "The Dark Knight", Genre = "Action", ReleaseDate = new DateTime(2008, 7, 18), Description = "A gritty superhero drama" }
        // );
    }
}
