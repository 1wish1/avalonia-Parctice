
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using System.Linq;
using System;
using AppoinmentScheduler.ViewModels;
using System.Threading.Tasks;

namespace AppoinmentScheduler.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly ISessionService _sessionService;

        private User _user {get ; set;} 



        public UserService(AppDbContext context, ISessionService SessionService)
        {
            _sessionService = SessionService;
            _context = context;
        
        }
        
        

        public async Task AddUser(User user, OAuthToken oAuthToken)
        {
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            
            _context.Users.Add(user);
            _context.SaveChanges();
            
            User newUser = _context.Users.FromSqlRaw("SELECT * FROM Users WHERE user_name = {0} and password = {1}", user.user_name, user.password).FirstOrDefault();
            _user =  newUser;

            Console.WriteLine("AddUser name" + user.user_name);

            oAuthToken.role = user.role;
            oAuthToken.id = user.id;

            _ = _sessionService.SaveSessionAsync(oAuthToken);
        
            

        }

        public async Task<bool> VerifyUser(string inputPassword, string inputEmail, string inputUserName)
        { 
            var pulluser = _context.Users
            .FromSqlRaw("SELECT id, user_name, email, password, token, role FROM Users WHERE user_name = {0} AND email = {1}", inputUserName, inputEmail)
            .Select(u => new { u.id ,u.user_name, u.email, u.password, u.token, u.role })
            .FirstOrDefault();
            
            Console.WriteLine($" {pulluser.id} ,{pulluser.user_name}, {pulluser.email}, {pulluser.password}, {pulluser.token}, {pulluser.role}");
            

            if (pulluser == null ){
                return false;
            }else if(!BCrypt.Net.BCrypt.Verify(inputPassword, pulluser.password)){
                return false;
            }else{

                OAuthToken oAuthToken = new OAuthToken
                {
                    AccessToken = Guid.NewGuid().ToString(),
                    ExpiresIn = 3600, // one hour
                    IssuedAt = DateTime.UtcNow,
                    role = pulluser.role,
                    id = pulluser.id
                };

                _context.Database.ExecuteSqlRaw("UPDATE Users SET token = {0} WHERE user_name = {1} AND email = {2}", BCrypt.Net.BCrypt.HashPassword(oAuthToken.AccessToken), inputUserName, inputEmail);
            
                _user = new User 
                {   
                    id = pulluser.id,
                    user_name = pulluser.user_name, 
                    email = pulluser.email, 
                    password = pulluser.password,
                    token = BCrypt.Net.BCrypt.HashPassword(oAuthToken.AccessToken),
                    role = pulluser.role
                };
                
                _ = _sessionService.SaveSessionAsync(oAuthToken);

                
            }
                return true;
        }
       
        public User getUser(){
            
            Console.WriteLine("getUser name" + _user.user_name);
            return _user;
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
