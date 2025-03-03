using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ConsoleQuizApp;

[Index(nameof(UserId))]
public class QuizUser
{
    public int Id { get; set; }

    public int Score { get; set; }

    public int MaxScore { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public int UserId { get; set; }

    [Required]
    public User User { get; set; }

    public int QuizId { get; set; }

    [Required]
    public Quiz Quiz { get; set; }

}