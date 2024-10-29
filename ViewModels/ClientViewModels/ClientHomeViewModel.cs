using System;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;

namespace AppoinmentScheduler.ViewModels.ClientViewModels
{
    public partial class ClientHomeViewModel : ViewModelBase
    {
        [ObservableProperty] private string? _username;
        [ObservableProperty] private User? _user;
        public ClientHomeViewModel(IMessenger messenger)
        {
            messenger.Register<ClientHomeViewModel, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
                _username = _user?.email;
                Console.WriteLine(_username);
            });
            Console.WriteLine(_username);
         
        }
        
    }
}
