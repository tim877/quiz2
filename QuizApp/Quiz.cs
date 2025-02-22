using System.Collections.Generic;

namespace ConsoleQuizApp
{
    public class Quiz
    {
        public string Title { get; }
        public List<Question> Questions { get; }

        public Quiz(string title)
        {
            Title = title;
            Questions = new List<Question>();
        }

        public void AddQuestion(string questionText, string correctAnswer)
        {
            Questions.Add(new Question(questionText, correctAnswer));
        }
    }
}
