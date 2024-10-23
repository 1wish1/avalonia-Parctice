
using System;
using Models;

namespace AppoinmentScheduler.Services
{
    public interface IUserService
    {
        void AddUser(User user);
        bool VerifyUser(string inputPassword, string inputEmail, string inputUserName);

         int Role();
        
    }
}