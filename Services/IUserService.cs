
using System;
using System.Threading.Tasks;
using AppoinmentScheduler.ViewModels;
using Models;

namespace AppoinmentScheduler.Services
{
    public interface IUserService
    {
        void AddUser(User user, OAuthToken oAuthToken);
        bool Login(string inputPassword, string inputEmail, string inputUserName);
        void SendData(User user);
        User? getUser();
        void updateUser();
        
        
    }
}