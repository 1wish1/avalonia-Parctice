
using System;
using Models;

namespace AppoinmentScheduler.Services
{
    public interface IUserService
    {
        void AddUser(User user);

        String getUserName();
    }
}