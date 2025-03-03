using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleQuizApp
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MinLength(4), MaxLength(12)]
        public required string Username { get; set; }

        [Required, MinLength(4), MaxLength(255)]
        public required string Password { get; set; }

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
