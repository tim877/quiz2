using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ConsoleQuizApp;

// Index attribute creates a database index on the UserId column.
[Index(nameof(UserId))]
public class QuizUser
{
    // Primary key for the QuizUser entity.
    public int Id { get; set; }

    // The score the user achieved on the quiz.
    public int Score { get; set; }

    // The maximum possible score for the quiz.
    public int MaxScore { get; set; }

    // The date and time when the QuizUser record was created.
    // The default value is set to the current UTC time when a new record is created.
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key linking to the user who took the quiz.
    public int UserId { get; set; }

    // Navigation property to the User entity (the user who completed the quiz).
    [Required]
    public User User { get; set; }

    // Foreign key linking to the quiz that the user took.
    public int QuizId { get; set; }

    // Navigation property to the Quiz entity (the quiz the user participated in).
    [Required]
    public Quiz Quiz { get; set; }
}
