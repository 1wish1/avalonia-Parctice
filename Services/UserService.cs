
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
        private User? _user { get; set; }

        private readonly IMessenger _messenger;

        public UserService(AppDbContext context, ISessionService SessionService, IMessenger messenger )
        {
            _sessionService = SessionService;
            _context = context;
            _messenger = messenger;
        }
     
        public String AddUser(User user, OAuthToken oAuthToken)
        {   
                user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
                
                User? userVerfiy = _context.Users.FromSqlRaw(@"SELECT * FROM Users WHERE email = {0} or user_name = {1}", user.email, user.user_name).FirstOrDefault();
                if(userVerfiy != null) return "Email or Name is ready use";
                
                _context.Users.Add(user);
                _context.SaveChanges();
                
                User newUser = _context.Users.FromSqlRaw(@"SELECT * FROM Users WHERE user_name = {0} and password = {1} and email = {2}", user.user_name, user.password,user.email).FirstOrDefault();
                if(newUser is null) return "Fail to Regester";
                SendData(newUser);

                oAuthToken.role = user.role;

                oAuthToken.id = user.id;

                _ = _sessionService.SaveSessionAsync(oAuthToken);
                
                return "done";
                
        }


        public String Login(string inputPassword, string inputEmail, string inputUserName)
        { 
            
            var pulluser = _context.Users
            .FromSqlRaw(@"SELECT id, user_name, email, password, token, role FROM Users WHERE user_name = {0} AND email = {1}", inputUserName, inputEmail)
            .FirstOrDefault();
            if (pulluser == null ){
                return "Fail no user found";
            }else if(!BCrypt.Net.BCrypt.Verify(inputPassword, pulluser.password)){
                return "Incorect password";
            }else{

                OAuthToken oAuthToken = new OAuthToken
                {
                    AccessToken = Guid.NewGuid().ToString(),
                    ExpiresIn = 3600, // one hour
                    IssuedAt = DateTime.UtcNow,
                    role = pulluser.role,
                    id = pulluser.id
                };

                _context.Database.ExecuteSqlRaw(@"UPDATE Users SET token = {0} WHERE user_name = {1} AND email = {2}", BCrypt.Net.BCrypt.HashPassword(oAuthToken.AccessToken), inputUserName, inputEmail);
            
                pulluser.token = BCrypt.Net.BCrypt.HashPassword(oAuthToken.AccessToken);

                SendData(pulluser);
                _ = _sessionService.SaveSessionAsync(oAuthToken); 
            }
               
                return "done";
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
           
                string name = "johnkarltabuzoooo@gmail.com";
                _context.Database.ExecuteSqlRaw("UPDATE Users SET user_name = {0} WHERE id = 9011", name);

                // Re-query the user from the database without tracking to get the updated data
                _user = _context.Users
                    .FromSqlRaw("SELECT id, user_name, email, password, token, role FROM Users WHERE id = 9011")
                    .AsNoTracking()
                    .FirstOrDefault();

                Console.WriteLine("updateUser: " + _user.user_name);
                _messenger.Send(new UserMessage(_user));
                
        }
        public bool CheckConnection(){
            return _context.CheckConnectionStatus();
        }

       
    }
}

