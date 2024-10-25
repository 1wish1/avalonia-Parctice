
using System;
using System.Threading.Tasks;
using AppoinmentScheduler.ViewModels;
using Models;

namespace AppoinmentScheduler.Services
{
    public interface IUserService
    {
        Task AddUser(User user, OAuthToken oAuthToken);
        Task<bool> Login(string inputPassword, string inputEmail, string inputUserName);
        User getUser();
        void SetUser(User? user);
    }
}