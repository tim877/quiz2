using System;
using System.Collections.Generic;

namespace ConsoleQuizApp
{
    public class Leaderboard
    {
        // Store user scores along with the quiz name
        private List<(string username, int score, string quizName)> leaderboardEntries = new List<(string, int, string)>();

        // Add a score entry along with the quiz name
        public void AddScore(string username, int score, string quizName)
        {
            leaderboardEntries.Add((username, score, quizName));
        }

        // Display the leaderboard
        // Display the leaderboard
public void DisplayLeaderboard()
{
    Console.Clear(); // Clears the console before displaying the leaderboard

    if (leaderboardEntries.Count == 0)
    {
        Console.WriteLine("No scores yet.");
        return;
    }

    // Sort the leaderboard in descending order of score
    leaderboardEntries.Sort((x, y) => y.score.CompareTo(x.score));

    Console.WriteLine("Leaderboard:");
    foreach (var entry in leaderboardEntries)
    {
        Console.WriteLine($"{entry.username}: {entry.score} points in {entry.quizName}");
    }
}

    }
}
