
using System;
using AppoinmentScheduler.ObjMessages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels.BusinessViewModels
{
    public partial class BusinessHomeViewModel : ViewModelBase
    {
        [ObservableProperty] private string? _username;
        [ObservableProperty] private User? _user;
        public BusinessHomeViewModel(IMessenger messenger)
        {
            messenger.Register<BusinessHomeViewModel, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
                _username = _user?.email;
                Console.WriteLine("BusinessHomeViewModel"+_username);
            });
             Console.WriteLine("BusinessHomeViewModel"+_username);      
        }
        
    }
}

