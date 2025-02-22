namespace ConsoleQuizApp
{
    public class Question
    {
        public string QuestionText { get; }
        public string CorrectAnswer { get; }

        public Question(string questionText, string correctAnswer)
        {
            QuestionText = questionText;
            CorrectAnswer = correctAnswer;
        }
    }
}
