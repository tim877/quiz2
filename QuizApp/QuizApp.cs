using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleQuizApp
{
    class QuizApp
    {
        private User loggedInUser; // Initially, no one is logged in
        private Leaderboard leaderboard = new Leaderboard(); // Add leaderboard instance

        private readonly AppDbContext _context;

        public QuizApp()
        {

            _context = new AppDbContext();
        }
        public void Run()
        {
            UserManager userManager = new UserManager(); // Manages user registration and login

            // Outer loop: Show login/register menu until a user logs in
            while (true)
            {
                if (loggedInUser == null)
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to the Quiz App!");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("3. Exit");
                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        loggedInUser = userManager.Login();
                        // Only proceed if login was successful
                        if (loggedInUser != null)
                        {
                            QuizMenu();
                        }
                    }
                    else if (choice == "2")
                    {
                        loggedInUser = userManager.Register();
                        // Only proceed if registration was successful
                        if (loggedInUser != null)
                        {
                            //QuizMenu(quizzes);
                        }
                    }
                    else if (choice == "3")
                    {
                        break; // Exit application
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
                    QuizMenu();
                }
            }
        }

        // Quiz menu is only accessible when logged in
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

                if (choice == "1")
                {
                    CreateQuiz();
                }
                else if (choice == "2")
                {
                    PlayQuiz();
                }
                else if (choice == "3")
                {
                    leaderboard.DisplayLeaderboard(); // Display the leaderboard
                    Console.WriteLine("Press Enter to return to the menu...");
                    Console.ReadLine();
                }
                else if (choice == "4")
                {
                    loggedInUser = null; // Logout resets the user
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

        // Function to create a quiz
        void CreateQuiz()
        {
            Console.Clear();
            Console.Write("Enter quiz title: ");
            string title = Console.ReadLine();
            List<Question> quizList = new List<Question>();

            while (true)
            {
                Console.WriteLine("Write your question? Write 'done' if finished.");
                string question = Console.ReadLine();
                if (question == "done")
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
                quizList.Add(questionData);
            }

            Quiz quiz = new Quiz
            {
                Title = title,
                UserId = loggedInUser.Id,
                Question = quizList
            };
            _context.Quizzes.Add(quiz);
            int saveStatus = SaveChanges();

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

        // Function to play a quiz
        void PlayQuiz()
        {
            Console.Clear();
            List<Quiz> quizzes = _context.Quizzes.ToList<Quiz>();
            int counter = 1;
            foreach (var q in quizzes)
            {
                Console.WriteLine($"{counter}. {q.Title}");
            }





            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }
        private int SaveChanges()
        {
            int ret = 0;
            try
            {
                ret = _context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }

            return ret;
        }
    }
}
