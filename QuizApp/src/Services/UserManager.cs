using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ConsoleQuizApp
{
    // The UserManager class handles user-related operations like registration and login
    class UserManager
    {
        // Private field to hold the AppDbContext, used to interact with the database
        private readonly AppDbContext _context;

        // Constructor that initializes the UserManager and creates a new AppDbContext instance
        public UserManager()
        {
            _context = new AppDbContext();
        }

        // Method for registering a new user
        public User Register()
        {
            Console.Clear();
            string username;
            do
            {
                Console.Write("Enter a username: ");
                username = Console.ReadLine(); // User input for username

                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("Username cannot be empty or null. Please try again.");
                }

            } while (string.IsNullOrWhiteSpace(username)); // Keep asking until a valid username is provided

            string password;
            do
            {
                Console.Write("Enter a password (between 4 and 255 characters): ");
                password = ReadPassword(); // User input for password with masking

                if (password.Length < 4 || password.Length > 255)
                {
                    Console.WriteLine("Password must be between 4 and 255 characters. Please try again.");
                }

            } while (password.Length < 4 || password.Length > 255); // Keep asking until a valid password is provided

            // Create a new User object
            User newUser = new User
            {
                Username = username, // Assigning the username
                Password = BCrypt.Net.BCrypt.HashPassword(password), // Hashing the password before storing it
            };

            // Add the new user to the database context
            _context.Users.Add(newUser);

            // Save the changes to the database and check the result
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

        // Private method for reading the password from the console while hiding the input
        private string ReadPassword()
        {
            string password = ""; // Initialize an empty password string
            ConsoleKeyInfo key; // Variable to hold key information

            // Loop to read each key until Enter is pressed
            do
            {
                key = Console.ReadKey(true); // Read a key without showing it on the console

                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar; // Add the typed character to the password
                    Console.Write("*"); // Show '*' instead of the actual character
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1]; // Remove the last character from the password string
                    Console.Write("\b \b"); // Remove the '*' symbol from the console
                }

            } while (key.Key != ConsoleKey.Enter); // Continue until Enter is pressed

            Console.WriteLine(); // Move to the next line after password input is complete
            return password; // Return the final password string
        }

        // Method for logging in a user with their username and password
        public User? Login()
        {
            Console.Clear();
            string username;
            do
            {
                Console.Write("Enter username: ");
                username = Console.ReadLine(); // User input for username

                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("Username cannot be empty or null. Please try again.");
                }

            } while (string.IsNullOrWhiteSpace(username)); // Keep asking until a valid username is provided

            Console.Write("Enter password: ");
            string password = ReadPassword(); // User input for password with masking

            try
            {
                // Attempt to find the user in the database by comparing the username (case-insensitive)
                var user = _context.Users.Single(u => u.Username.ToLower() == username.ToLower());
                if (user == null)
                {
                    return null; // Return null if user is not found
                }

                // Verify the entered password against the stored hashed password
                bool valid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (valid)
                {
                    return user; // Return the user if credentials are valid
                }

                // If credentials are invalid, display an error message
                Console.WriteLine("\nInvalid credentials. Please try again.");
                Console.ReadLine();
            }
            catch (System.Exception)
            {
                // Catch any exceptions (e.g., if the user is not found in the database)
                Console.WriteLine("\nInvalid credentials. Please try again.");
                Console.ReadLine();
            }

            return null; // Return null if login fails
        }

        // Private method for saving changes to the database and handling exceptions
        private int SaveChanges()
        {
            int ret = 0; // Initialize return value for success (0 means no changes)
            try
            {
                ret = _context.SaveChanges(); // Try to save changes to the database
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update errors
                Console.WriteLine($"Database update error: {dbEx}");
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions
                Console.WriteLine($"General error: {ex.Message}");
            }

            return ret; // Return the result
        }
    }
}
