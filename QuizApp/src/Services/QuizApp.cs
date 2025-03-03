using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace ConsoleQuizApp
{
    class QuizApp
    {
        private User loggedInUser; // Keeps track of the logged-in user, initially set to null
        private Leaderboard leaderboard = new Leaderboard(); // Instance of leaderboard for displaying top scores
        private readonly AppDbContext _context; // Database context for interacting with the database

        // Constructor to initialize the AppDbContext
        public QuizApp()
        {
            _context = new AppDbContext();
        }

        // Main function that runs the application
        public void Run()
        {
            UserManager userManager = new UserManager(); // Manages user login and registration

            // Outer loop: Displays login/register menu until a user logs in
            while (true)
            {
                if (loggedInUser == null) // If no user is logged in
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to the Quiz App!");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("3. Exit");
                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine();

                    if (choice == "1") // Login option
                    {
                        loggedInUser = userManager.Login(); // Attempt to login
                        if (loggedInUser != null) // Proceed if login is successful
                        {
                            QuizMenu(); // Show quiz menu
                        }
                    }
                    else if (choice == "2") // Register option
                    {
                        loggedInUser = userManager.Register(); // Attempt to register
                        if (loggedInUser != null) // Proceed if registration is successful
                        {
                            //QuizMenu(quizzes);
                        }
                    }
                    else if (choice == "3") // Exit option
                    {
                        break; // Exit the application
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice, try again.");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                    }
                }
                else
                {
                    QuizMenu(); // Show quiz menu if user is logged in
                }
            }
        }

        // Quiz menu is accessible only when the user is logged in
        void QuizMenu()
        {
            while (loggedInUser != null)  // Continue until the user logs out
            {
                Console.Clear();
                Console.WriteLine($"Welcome, {loggedInUser.Username}!");
                Console.WriteLine("1. Create a new quiz");
                Console.WriteLine("2. Play a quiz");
                Console.WriteLine("3. View Leaderboard");
                Console.WriteLine("4. Logout");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                if (choice == "1") // Create new quiz
                {
                    CreateQuiz();
                }
                else if (choice == "2") // Play a quiz
                {
                    PlayQuiz();
                }
                else if (choice == "3") // View the leaderboard
                {
                    ViewLeaderboard();
                }
                else if (choice == "4") // Logout
                {
                    loggedInUser = null; // Reset logged-in user
                    Console.WriteLine("You have been logged out. Press Enter to return to the login menu...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Invalid choice, try again.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        // View the leaderboard showing top users scores
        private void ViewLeaderboard()
        {
            Console.Clear();

            List<QuizUser> leaderboardList = leaderboard.getLeaderboard(); // Get leaderboard data
            leaderboardList.ForEach(l => Console.WriteLine($"User: {l.User.Username} Quiz: {l.Quiz.Title} Score: {l.Score} Max Score: {l.MaxScore} Date: {l.CreatedAt}"));

            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }

        // Create a new quiz
        private void CreateQuiz()
        {
            Console.Clear();
            Console.Write("Enter quiz title: ");
            string title = Console.ReadLine();
            List<Question> quizList = new List<Question>();

            while (true)
            {
                Console.WriteLine("Write your question? Write 'done' if finished.");
                string question = Console.ReadLine();
                if (question == "done") // Stop creating questions
                {
                    break;
                }

                Console.WriteLine("Enter your answer.");
                string answer = Console.ReadLine();

                Question questionData = new Question
                {
                    QuestionText = question,
                    CorrectAnswer = answer
                };
                quizList.Add(questionData); // Add the question to the quiz list
            }

            // Create the quiz and add it to the database
            Quiz quiz = new Quiz
            {
                Title = title,
                UserId = loggedInUser.Id,
                Question = quizList
            };
            _context.Quizzes.Add(quiz);
            int saveStatus = SaveChanges(); // Save changes to the database

            if (saveStatus > 0)
            {
                Console.WriteLine("Quiz created successfully!");
            }
            else
            {
                Console.WriteLine("Quiz was not created.");
            }
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }

        // Function for the user to play a quiz
        private void PlayQuiz()
        {
            Console.Clear();
            List<Quiz> quizzes = _context.Quizzes.ToList<Quiz>(); // Get all quizzes from the database
            int counter = 1;
            JObject listId = new JObject(); // Holds quiz ID and title pairs
            foreach (var q in quizzes)
            {
                Console.WriteLine($"{counter}. {q.Title}"); // Display quiz options
                listId.Add(counter.ToString(), q.Id);
                counter++;
            }

            Console.WriteLine("Enter quiz number.");
            string input = Console.ReadLine();
            int QuizId = (int)listId.GetValue(input); // Get the selected quiz ID

            Quiz userQuiz = _context.Quizzes.Single(q => q.Id == QuizId); // Fetch quiz details

            Console.Clear();
            Console.WriteLine($"Welcome to quiz: {userQuiz.Title}");

            int correctCounter = 0;
            foreach (var q in userQuiz.Question)
            {
                Console.Write("Question: ");
                Console.WriteLine(q.QuestionText); // Display the question
                Console.Write("Answer: ");
                string answer = Console.ReadLine();
                if (answer == q.CorrectAnswer)
                {
                    correctCounter++; // Track correct answers
                }
            }
            Console.WriteLine($"\nYou got {correctCounter} out of {userQuiz.Question.Count} questions correct.");

            // Save the users results to the database
            QuizUser quizUser = new QuizUser
            {
                Score = correctCounter,
                MaxScore = userQuiz.Question.Count,
                UserId = loggedInUser.Id,
                QuizId = userQuiz.Id
            };
            _context.QuizUsers.Add(quizUser);
            int CommitStatus = SaveChanges();
            if (CommitStatus != 0)
            {
                Console.WriteLine("\nResult saved successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to save result.");
            }

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }

        // Helper function to save changes to the database
        private int SaveChanges()
        {
            int ret = 0;
            try
            {
                ret = _context.SaveChanges(); // Commit changes to the database
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx}"); // Handle database-specific errors
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}"); // Handle other exceptions
            }

            return ret;
        }
    }
}
