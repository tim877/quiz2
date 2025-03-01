using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleQuizApp
{
    class Leaderboard
    {
        private AppDbContext _context;

        public Leaderboard()
        {
            _context = new AppDbContext();
        }

        public List<QuizUser> getLeaderboard()
        {
            List<QuizUser> quiz = _context.QuizUsers
            .OrderByDescending(q => q.Score)
            .Include(q => q.Quiz)
            .Include(q => q.User)
            .Take(3)
            .ToList();

            return quiz;
        }
    }
}
