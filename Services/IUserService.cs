
using System;
using System.Threading.Tasks;
using AppoinmentScheduler.ViewModels;
using Models;

namespace AppoinmentScheduler.Services
{
    public interface IUserService
    {
        Task AddUser(User user, OAuthToken oAuthToken);
        Task<bool> VerifyUser(string inputPassword, string inputEmail, string inputUserName);
        User getUser();
        
    }
}