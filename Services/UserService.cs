
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using System.Linq;
using System;

namespace AppoinmentScheduler.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool VerifyUser(string inputPassword, string inputEmail, string inputUserName)
        {
            
            var user = _context.Users
            .FromSqlRaw("SELECT user_name, email, password FROM Users WHERE user_name = {0} AND email = {1}", inputUserName, inputEmail)
            .Select(u => new { u.user_name, u.email, u.password })
            .FirstOrDefault();
             
            if (user == null ){
                return false;
            }else if(!BCrypt.Net.BCrypt.Verify(inputPassword, user.password)){
                return false;
            }
            Console.WriteLine(user.user_name + user.password + user.email);
                return true;
        }

        
    }

}





// public string GetUserName(int userId)
// {
//     // Fetch the user based on the provided user ID
//     var user = _context.Users
//         .FromSqlRaw("SELECT * FROM Users WHERE id = {0}", userId)
//         .FirstOrDefault();

//     // Check if the user was found and return the username, or return null if not found
//     return user?.user_name; // Using null-conditional operator
// }

// // During signup or when saving a password
// public string HashPassword(string password)
// {
//     return BCrypt.Net.BCrypt.HashPassword(password);
// }

// // During login, verify the hashed password
// public bool VerifyPassword(string inputPassword, string storedHash)
// {
//     return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
// }

// public void RateLimitLoginAttempts(string email)
// {
//     // Keep track of failed login attempts and lock the account temporarily after too many failures
//     var failedAttempts = GetFailedAttempts(email);

//     if (failedAttempts >= 5)
//     {
//         throw new UnauthorizedAccessException("Too many failed login attempts. Please try again later.");
//     }
// }
