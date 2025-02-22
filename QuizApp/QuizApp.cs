using System;
using System.Collections.Generic;

namespace ConsoleQuizApp
{
    class QuizApp
    {
        static User loggedInUser = null; // Initially, no one is logged in
        static Leaderboard leaderboard = new Leaderboard(); // Add leaderboard instance

        public static void Run()
        {
            List<Quiz> quizzes = new List<Quiz>();
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
                            QuizMenu(quizzes);
                        }
                    }
                    else if (choice == "2")
                    {
                        loggedInUser = userManager.Register();
                        // Only proceed if registration was successful
                        if (loggedInUser != null)
                        {
                            QuizMenu(quizzes);
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
                    QuizMenu(quizzes);
                }
            }
        }

        // Quiz menu is only accessible when logged in
        static void QuizMenu(List<Quiz> quizzes)
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
                    CreateQuiz(quizzes);
                }
                else if (choice == "2")
                {
                    PlayQuiz(quizzes);
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
        static void CreateQuiz(List<Quiz> quizzes)
        {
            Console.Clear();
            Console.Write("Enter quiz title: ");
            string title = Console.ReadLine();

            // Pass the logged-in user's name when creating the quiz
            Quiz quiz = new Quiz(title, loggedInUser.Username);  

            while (true)
            {
                Console.WriteLine("Enter question (or type 'done' to finish): ");
                string question = Console.ReadLine();
                if (question.ToLower() == "done") break;

                Console.WriteLine("Enter correct answer: ");
                string correctAnswer = Console.ReadLine();

                quiz.AddQuestion(question, correctAnswer);
            }

            quizzes.Add(quiz);
            Console.WriteLine("Quiz created successfully!");
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }

        // Function to play a quiz
        static void PlayQuiz(List<Quiz> quizzes)
        {
            Console.Clear();
            if (quizzes.Count == 0)
            {
                Console.WriteLine("No quizzes available. Please create a quiz first.");
                Console.WriteLine("Press Enter to return to the menu...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Available quizzes:");
            for (int i = 0; i < quizzes.Count; i++)
            {
                // Display the creator's name next to the quiz title
                Console.WriteLine($"{i + 1}. {quizzes[i].Title} (Created by: {quizzes[i].Creator})");
            }

            Console.Write("Choose a quiz by number: ");
            if (!int.TryParse(Console.ReadLine(), out int quizIndex))
            {
                Console.WriteLine("Invalid input. Returning to menu.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }
            quizIndex--;

            if (quizIndex < 0 || quizIndex >= quizzes.Count)
            {
                Console.WriteLine("Invalid choice, returning to menu.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Quiz selectedQuiz = quizzes[quizIndex];
            int score = 0;

            Console.Clear();
            foreach (var question in selectedQuiz.Questions)
            {
                Console.WriteLine(question.QuestionText);
                Console.Write("Your answer: ");
                string userAnswer = Console.ReadLine();

                if (userAnswer.Equals(question.CorrectAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    score++;
                }
            }

            // Add score to leaderboard
            leaderboard.AddScore(loggedInUser.Username, score, selectedQuiz.Title);

            Console.WriteLine($"Your score: {score} out of {selectedQuiz.Questions.Count}");
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }
    }
}
