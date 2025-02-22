namespace ConsoleQuizApp
{
    class Quiz
    {
        public string Title { get; set; }
        public List<Question> Questions { get; set; }
        public string Creator { get; set; }

        // Constructor to accept both title and creator
        public Quiz(string title, string creator)
        {
            Title = title;
            Creator = creator;
            Questions = new List<Question>();
        }

        // Method to add a new question
        public void AddQuestion(string questionText, string correctAnswer)
        {
            Questions.Add(new Question(questionText, correctAnswer));
        }
    }
}
