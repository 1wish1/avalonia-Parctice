
using System;
using Models;

namespace AppoinmentScheduler.Services
{
    public interface ISignupService
    {
        void AddUser(User user);

        String getUserName();
    }
}