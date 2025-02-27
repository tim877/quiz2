using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace ConsoleQuizApp
{
    public class Quiz
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public User User { get; set; }

        [Column(TypeName = "jsonb")]
        public string Data { get; set; } = "{}";
        public JObject DataJson
        {
            get => JObject.Parse(Data);
            set => Data = value.ToString();
        }
    }
}
