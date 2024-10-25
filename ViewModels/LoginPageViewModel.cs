using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppoinmentScheduler.ViewModels;

public partial class LoginPageViewModel(IUserService userService, MainWindowViewModel mainWindowViewModel) : ViewModelBase
{
        [ObservableProperty] private string? _username;
        [ObservableProperty] private string? _email;
        [ObservableProperty] private string? _password;
        [ObservableProperty] private string? _error;

        private readonly MainWindowViewModel _mainWindowViewModel = mainWindowViewModel;
        private readonly IUserService _userService = userService;

    [RelayCommand] private async Task OnsubmitAsync()
        {
            Error = string.Empty;

            // Validate inputs
            if (!ValidateInputs())
            {
                return; // Stop submission if validation fails
            }
            if(await _userService.VerifyUser(Password, Email, Username)){
                
                _mainWindowViewModel.SetViewAsync(_userService.getUser());
            }else{
                return;
            }
             
        }






        private bool ValidateInputs()
        {
            // Validate Username
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3)
            {
                Error = "Username must be at least 3 characters long.";
                return false;
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(Email) || !IsValidEmail(Email))
            {
                Error = "Please enter a valid email address.";
                return false;
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            {
                Error = "Password must be at least 6 characters long.";
                return false;
            }

            return true; // All validations passed
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
}