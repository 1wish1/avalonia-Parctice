using AppoinmentScheduler.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AppoinmentScheduler.ViewModels;
using AppoinmentScheduler.Views;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Data;
using Microsoft.EntityFrameworkCore;

namespace AppoinmentScheduler
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Remove Avalonia data validation to avoid duplicates
                BindingPlugins.DataValidators.RemoveAt(0);

                var services = new ServiceCollection();
                services.AddTransient<IUserService, UserService>();
                services.AddDbContext<AppDbContext>(options =>
                     options.UseSqlServer("Server=localhost;Database=AppoinmentScheduler;User Id=sa;Password=KarlPogi5758;Encrypt=True;TrustServerCertificate=True;"));

                // Register ViewModels
                services.AddTransient<MainWindowViewModel>();
                services.AddTransient<HomePageViewModel>();
                services.AddTransient<LoginPageViewModel>();
                services.AddTransient<SignUpPageViewModel>();

                var serviceProvider = services.BuildServiceProvider();

                var splashScreenVm = new SplashScreenViewModel();
                var splashScreen = new SplashScreenView
                {
                    DataContext = splashScreenVm
                };
                desktop.MainWindow = splashScreen;
                splashScreen.Show();
                try
                {
                    splashScreenVm.StartupMessage = "Searching for device...";
                    await Task.Delay(1000, splashScreenVm.CancellationToken);
                    splashScreenVm.StartupMessage = "Connecting to device #1...";
                    await Task.Delay(2000, splashScreenVm.CancellationToken);
                    splashScreenVm.StartupMessage = "Configuring device...";
                    await Task.Delay(2000, splashScreenVm.CancellationToken);
                }
                catch (TaskCanceledException)
                {
                    splashScreen.Close();
                    return;
                }

                // Resolve the MainWindowViewModel from the service provider
                var mainWin = new MainWindow
                {
                    DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>(),
                };
                desktop.MainWindow = mainWin;
                mainWin.Show();
                splashScreen.Close();
            }

            // Call the base method
            base.OnFrameworkInitializationCompleted();
        }
    }
}
