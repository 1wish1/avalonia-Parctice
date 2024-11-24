using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;
using AppoinmentScheduler.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels
{
    public partial class SignUpPageViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        
              
        public SignUpPageViewModel(IUserService userService, MainWindowViewModel mainWindowViewModel, ISessionService SessionService)
        {   
            _mainWindowViewModel = mainWindowViewModel;
            _userService = userService;
            _sessionService = SessionService;       
        }

        [ObservableProperty] private string? _username;
        [ObservableProperty] private string? role;
        [ObservableProperty] private string? _email;
        [ObservableProperty] private string? _password;
        [ObservableProperty] private string? _error;
        [ObservableProperty] private bool? _isBusiness;
        [ObservableProperty] private bool? _isClient;

        [RelayCommand] 
        private async Task OnsubmitAsync()
        {
            // Clear previous error
            Error = string.Empty;

            // Validate inputs
            if (!ValidateInputs())
            {
                return; // Stop submission if validation fails
            }
            
            OAuthToken oAuthToken = new OAuthToken
            {
                AccessToken = Guid.NewGuid().ToString(),
                ExpiresIn = 3600, // one hour
                IssuedAt = DateTime.UtcNow,
            };

            User user = new User 
            { 
                user_name = Username, 
                email = Email, 
                password = Password,
                token = BCrypt.Net.BCrypt.HashPassword(oAuthToken.AccessToken),
                role = UserRole(),
            };
            try{
                string validation = _userService.AddUser(user, oAuthToken);
                if(validation == "done"){
                    Console.WriteLine("asdasd");
                    _mainWindowViewModel.SetView();
                    _ = LoadSplashAsync();
                }else{
                    _ = LoadSplashAsync();
                    Error = validation;
                }
            }catch(Exception e){
                Console.WriteLine(e);
                _ = LoadSplashAsync();
                Error = "Connection denied" ;
                return;
            }
    
            
        }

        private async Task LoadSplashAsync(){

            //remove page
            // load main viewmodel

            var splashScreenVm = new SplashScreenViewModel();
            var splashScreen = new SplashScreenView
            {
                DataContext = splashScreenVm
            };
            
            splashScreen.Show();
            try
            {
                splashScreenVm.StartupMessage = "Initailaizing Account...";
                await Task.Delay(5000);
                splashScreen.Close();
            }
            catch (TaskCanceledException)
            {
                //remove account 
                // go back
                splashScreen.Close();
                
            }
        }

        private bool ValidateInputs()
        {
            // Validate Username
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3)
            {
                Error = "Username must be at least 3 characters long." ;
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
        private int UserRole(){
            if(_isBusiness is true) return 1;
            return 0;
        }


   
        


    }
}
