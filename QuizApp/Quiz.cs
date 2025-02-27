using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ConsoleQuizApp
{
    public class Quiz
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int UserId { get; set; }
        [Required]
        public User User { get; set; }

        [Column(TypeName = "jsonb")]
        public string Data { get; set; } = "{}";

        [NotMapped]
        public Question Question
        {
            get => JsonConvert.DeserializeObject<Question>(Data);
            set => Data = JsonConvert.SerializeObject(value);
        }
    }
}
