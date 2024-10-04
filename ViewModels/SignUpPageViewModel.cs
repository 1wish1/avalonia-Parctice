using System.Text.RegularExpressions;
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
        [ObservableProperty] private string? _error;

        [RelayCommand] 
        private void Onsubmit()
        {
            // Clear previous error
            Error = string.Empty;

            // Validate inputs
            if (!ValidateInputs())
            {
                return; // Stop submission if validation fails
            }

            // Create user object and add it
            var user = new User 
            { 
                user_name = Username, 
                email = Email, 
                password = Password 
            };
            _userService.AddUser(user);
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
}
