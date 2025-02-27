using ConsoleQuizApp;

public class QuizService
{

    private Quiz Quiz ;
    public void CreateQuiz(string title, User creator) {
        Quiz Quiz = new Quiz(title, creator);
    }
    // Method to add a new question
    public void AddQuestion(string questionText, string correctAnswer)
    {
        Question.Add(new Question(questionText, correctAnswer));
    }
}