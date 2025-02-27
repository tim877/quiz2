using ConsoleQuizApp;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizUser> QuizUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=quiz;Username=myser;Password=mypassword");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Quiz -> User (Many-to-One)
        modelBuilder
            .Entity<Quiz>()
            .HasOne(q => q.User)
            .WithMany(u => u.Quizzes)
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Leaderboard -> Quiz (Many-to-One)
        /*modelBuilder.Entity<Leaderboard>()
            .HasOne(l => l.Quiz)
            .WithMany()
            .HasForeignKey(l => l.QuizId)
            .OnDelete(DeleteBehavior.Cascade);*/
    }
}
