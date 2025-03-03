using ConsoleQuizApp;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // DbSet properties represent the tables in the database.
    public DbSet<User> Users { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizUser> QuizUsers { get; set; }

    // Configures the database connection string.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=quiz;Username=myser;Password=mypassword");
    }

    // Defines the relationships and model configurations.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuring the relationship between Quiz and User (Many-to-One)

        // A quiz is associated with one user (Many quizzes can belong to one user).
        // Defining how the Quiz table will reference the User table.
        modelBuilder
            .Entity<Quiz>() // Target the Quiz entity (table).
            .HasOne(q => q.User) // A quiz has one user (each quiz is linked to one user).
            .WithMany(u => u.Quizzes) // A user can have many quizzes.
            .HasForeignKey(q => q.UserId) // The foreign key in the Quiz table is UserId.
            .OnDelete(DeleteBehavior.Cascade); // If a user is deleted, all related quizzes will also be deleted.
    }
}
