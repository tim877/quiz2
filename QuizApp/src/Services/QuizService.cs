using ConsoleQuizApp;
public class QuizService
{
    // Private field to hold the database context for interacting with the database
    private readonly AppDbContext _context;

    // Constructor for initializing the QuizService with an AppDbContext
    public QuizService(AppDbContext context)
    {
        // Assigning the injected context to the private field
        _context = context;
    }
}
