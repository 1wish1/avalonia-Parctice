using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

namespace AppoinmentScheduler.ViewModels
{
    public partial class SignUpPageViewModel : ViewModelBase
    {
        private readonly IUserService _userService;

        public SignUpPageViewModel(IUserService userService)
        {
            _userService = userService;
        }

        [ObservableProperty] private string? _username;

        [ObservableProperty] private string? _email;

        [ObservableProperty] private string? _password;

        [RelayCommand] private void Onsubmit()
        {
            var user = new User { user_name = Username, email = Email, password = Password };
            _userService.AddUser(user);
        }
    }
}