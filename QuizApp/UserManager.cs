using System;
using System.Collections.Generic;

namespace ConsoleQuizApp
{
    class UserManager
    {
        private List<User> users = new List<User>();

        public User Register()
        {
            Console.Clear();
            Console.Write("Enter a username: ");
            string username = Console.ReadLine();
            Console.Write("Enter a password: ");
            string password = Console.ReadLine();

            User existingUser = users.Find(u => u.Username == username);
            if (existingUser != null)
            {
                Console.WriteLine("Username already exists. Press Enter to return to the menu.");
                Console.ReadLine();
                return null;
            }

            User newUser = new User(username, password);
            users.Add(newUser);
            Console.WriteLine("Registration successful! Press Enter to log in.");
            Console.ReadLine();
            return newUser;
        }

        public User Login()
        {
            Console.Clear();
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            User user = users.Find(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                Console.WriteLine("Login successful! Press Enter to continue.");
                Console.ReadLine();
                return user;
            }
            else
            {
                Console.WriteLine("Invalid username or password. Press Enter to try again.");
                Console.ReadLine();
                return null;
            }
        }
    }
}
