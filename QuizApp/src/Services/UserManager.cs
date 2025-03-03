using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleQuizApp
{
    class UserManager
    {
        private readonly AppDbContext _context;

        public UserManager()
        {
            _context = new AppDbContext();
        }

        public User Register()
        {
            Console.Clear();
            Console.Write("Enter a username: ");
            string username = Console.ReadLine();
            Console.Write("Enter a password: ");
            string password = ReadPassword();

            User newUser = new User
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
            };

            _context.Users.Add(newUser);
            int CommitStatus = SaveChanges();
            if (CommitStatus != 0)
            {
                Console.WriteLine("Registration successful!");
            }
            else
            {
                Console.WriteLine("Registration failed!");
            }
            Console.ReadLine();
            return newUser;
        }

        private string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true); // Read a key, but don't show it

                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write("*"); // Show '' instead of the character
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1]; // Remove last char
                    Console.Write("\b \b"); // Remove '*' from console
                }

            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine(); // Move to the next line
            return password;
        }

        public User? Login()
        {
            Console.Clear();
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = ReadPassword();

            try
            {
                var user = _context.Users.Single(u => u.Username.ToLower() == username.ToLower());
                if (user == null)
                {
                    return null;
                }
                bool valid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (valid)
                {
                    return user;
                }
                Console.WriteLine("\nInvalid credentials. Please try again.");
                Console.ReadLine();

            }
            catch (System.Exception)
            {
                Console.WriteLine("\nInvalid credentials. Please try again.");
                Console.ReadLine();
            }
            return null;
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
