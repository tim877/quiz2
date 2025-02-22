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

            User newUser = new User(username, password);
            users.Add(newUser);

            Console.WriteLine("Registration successful!");
            return newUser;
        }

        public User Login()
        {
            Console.Clear();
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            foreach (var user in users)
            {
                if (user.Username == username && user.Password == password)
                {
                    Console.WriteLine("Login successful!");
                    return user;
                }
            }

            Console.WriteLine("Invalid credentials. Please try again.");
            return null;
        }
    }
}
