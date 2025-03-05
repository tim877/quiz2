using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ConsoleQuizApp
{
    // The Quiz class represents a quiz and its associated data (questions, title, and user).
    public class Quiz
    {
        // Primary key for the quiz.
        public int Id { get; set; }

        // Title of the quiz.
        [Required]
        public string? Title { get; set; }

        // Foreign key linking to the User who created the quiz.
        public int UserId { get; set; }

        // Navigation property to the User entity (who created the quiz).
        [Required]
        public User? User { get; set; }

        [Column(TypeName = "jsonb")]
        public string Data { get; set; } = "{}";  // Default value is an empty JSON object.

        // The 'Question' property is not mapped to the database, but it provides a way to access and set quiz questions in a structured format (List of Question objects).
        [NotMapped]
        public List<Question> Question
        {
            // Deserializes the JSON data from 'Data' and returns it as a List of Question objects.
            // If 'Data' is null, returns an empty list.
            get => JsonConvert.DeserializeObject<List<Question>>(Data ?? "[]") ?? new List<Question>();

            // Serializes a List of Question objects into a JSON string and stores it in 'Data'.
            // If the value is null, stores an empty JSON array.
            set => Data = JsonConvert.SerializeObject(value ?? new List<Question>());
        }
        public ICollection<QuizUser> QuizUsers { get; set; } = new List<QuizUser>();

    }
}
