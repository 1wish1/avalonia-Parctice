
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using System.Linq;
using System;
using AppoinmentScheduler.ViewModels;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using AppoinmentScheduler.ObjMessages;

namespace AppoinmentScheduler.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly ISessionService _sessionService;
        public User? _user { get; private set; }

        private readonly IMessenger _messenger;

        public UserService(AppDbContext context, ISessionService SessionService, IMessenger messenger )
        {
            _sessionService = SessionService;
            _context = context;
            _messenger = messenger;

        }
     
        public void AddUser(User user, OAuthToken oAuthToken)
        {
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            
            _context.Users.Add(user);
            _context.SaveChanges();
            
            User newUser = _context.Users.FromSqlRaw("SELECT * FROM Users WHERE user_name = {0} and password = {1}", user.user_name, user.password).FirstOrDefault();

            if(newUser is null) return;
            SendData(newUser);

            oAuthToken.role = user.role;

            oAuthToken.id = user.id;
            
            _ = _sessionService.SaveSessionAsync(oAuthToken);
        }


        public bool Login(string inputPassword, string inputEmail, string inputUserName)
        { 
            var pulluser = _context.Users
            .FromSqlRaw("SELECT id, user_name, email, password, token, role FROM Users WHERE user_name = {0} AND email = {1}", inputUserName, inputEmail)
            .FirstOrDefault();
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
            
                pulluser.token = BCrypt.Net.BCrypt.HashPassword(oAuthToken.AccessToken);

                SendData(pulluser);
                _ = _sessionService.SaveSessionAsync(oAuthToken); 
            }
                return true;
        }

        public void SendData(User user)
        {
             _messenger.Send(new UserMessage(user));
        }
        public User? getUser()
        {
            return _user;
        }
        
        public void updateUser(){
           
                string name = "johnkarltabuzo@gmail.com";
                _context.Database.ExecuteSqlRaw("UPDATE Users SET user_name = {0} WHERE id = 9011", name);

                // Re-query the user from the database without tracking to get the updated data
                _user = _context.Users
                    .FromSqlRaw("SELECT id, user_name, email, password, token, role FROM Users WHERE id = 9011")
                    .AsNoTracking()
                    .FirstOrDefault();

                Console.WriteLine("updateUser: " + _user.user_name);
                _messenger.Send(new UserMessage(_user));
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
