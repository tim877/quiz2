using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleQuizApp
{
    // The Leaderboard class is responsible for fetching and displaying the top quiz users based on their scores.
    class Leaderboard
    {
        private AppDbContext _context; // Instance of the AppDbContext to interact with the database.

        // Constructor to initialize the context.
        public Leaderboard()
        {
            _context = new AppDbContext(); // Creates a new instance of AppDbContext to access the database.
        }

        // Method to get the leaderboard.
        public List<QuizUser> getLeaderboard()
        {
            // Fetches the top users based on their scores in descending order.
            List<QuizUser> quiz = _context.QuizUsers
            .OrderByDescending(q => q.Score)  // Orders the QuizUsers by their score in descending order (highest score first).
            .Include(q => q.Quiz)  // Includes the related Quiz entity for each QuizUser.
            .Include(q => q.User)  // Includes the related User entity for each QuizUser.
            .Take(10)  // Limits the number of results to be displayed on the leaderboard.
            .ToList();  // Executes the query and converts the result into a list.

            return quiz;  // Returns the top quiz users with their associated Quiz and User details.
        }
    }
}
