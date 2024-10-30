
using System;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels.BusinessViewModels
{
    public partial class BusinessHomeViewModel : ViewModelBase
    {
        [ObservableProperty] private string? _username;
        [ObservableProperty] private User? _user;
        private IUserService _userService; 
        private readonly MainWindowViewModel _mainWindowViewModel;
        public BusinessHomeViewModel(IMessenger messenger, IUserService userService, MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            messenger.Register<BusinessHomeViewModel, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
                _username = _user?.user_name;
                Console.WriteLine("BusinessHomeViewModel"+_username);
            });
             Console.WriteLine("BusinessHomeViewModel"+_username);      
             _userService = userService;
        }
        [RelayCommand] private void Onsubmit()
        {
            _mainWindowViewModel.UpdateView();
             
        }
        
    }
}

