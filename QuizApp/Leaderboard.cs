using System;
using System.Collections.Generic;

namespace ConsoleQuizApp
{
    class Leaderboard
    {
        private List<ScoreEntry> scoreEntries = new List<ScoreEntry>();

        public void AddScore(string username, int score, string quizTitle)
        {
            scoreEntries.Add(new ScoreEntry(username, score, quizTitle));
        }

        public void DisplayLeaderboard()
        {
            Console.Clear();
            Console.WriteLine("Leaderboard:");

            foreach (var entry in scoreEntries)
            {
                Console.WriteLine($"{entry.Username} - {entry.Score} - Quiz: {entry.QuizTitle}");
            }

            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }
    }

    class ScoreEntry
    {
        public string Username { get; set; }
        public int Score { get; set; }
        public string QuizTitle { get; set; }

        public ScoreEntry(string username, int score, string quizTitle)
        {
            Username = username;
            Score = score;
            QuizTitle = quizTitle;
        }
    }
}
