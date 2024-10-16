
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using System;
using System.Linq;

namespace AppoinmentScheduler.Services
{
    public class SignupService : ISignupService
    {
        private readonly AppDbContext _context;

        public SignupService(AppDbContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

         public string getUserName()
        {
            // Fetch the user based on the provided username
            var user = _context.Users
                .FromSqlRaw("SELECT * FROM Users WHERE id = 2004")
                .FirstOrDefault();

            // Check if the user was found and return the username, or return null if not found
            return user?.user_name; // Using null-conditional operator
        }

        
    }
}