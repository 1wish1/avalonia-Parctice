
using System;
using AppoinmentScheduler.ObjMessages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels.BusinessViewModels
{
    public partial class ManagementViewModel : ViewModelBase
    {
        [ObservableProperty] private string? _username;
        [ObservableProperty] private User? _user;

        public ManagementViewModel(IMessenger messenger)
        {
             messenger.Register<ManagementViewModel, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
                _username = _user?.email;
                Console.WriteLine("ManagementViewModel"+_username);
            });
             Console.WriteLine("ManagementViewModel"+_username);      

        }
        
    }
}

