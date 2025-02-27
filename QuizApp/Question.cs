namespace ConsoleQuizApp
{
    public class Question
    {
        // Properties for Question Text and Correct Answer
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }

        // Constructor with parameters for both properties
        public Question(string questionText, string correctAnswer)
        {
            QuestionText = questionText;
            CorrectAnswer = correctAnswer;
        }
    }
}
