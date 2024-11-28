using AppoinmentScheduler.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AppoinmentScheduler.ViewModels;

using AppoinmentScheduler.Views;

using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;


using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using AppoinmentScheduler.ViewModels.BusinessViewModels;
using AppoinmentScheduler.ViewModels.ClientViewModels;
using AppoinmentScheduler.Views.BusinessViews;




namespace AppoinmentScheduler;

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

            // Set up dependency injection
            var services = new ServiceCollection();
            ConfigureViewModels(services);
            ConfigureViews(services); // Service registration

            var serviceProvider = services.BuildServiceProvider();
            Ioc.Default.ConfigureServices(serviceProvider); // Configure IoC

             // Resolve the MainWindowViewModel from the service provider
            var mainWin = new MainWindow
            {
                DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>(),
            };
           
        
            // Show splash screen
            await ShowSplashScreen(desktop, mainWin);
        }


        
        base.OnFrameworkInitializationCompleted();
    }






    private async Task ShowSplashScreen(IClassicDesktopStyleApplicationLifetime desktop, MainWindow mainWin)
    {
        var splashScreenVm = new SplashScreenViewModel();
        var splashScreen = new SplashScreenView
        {
            DataContext = splashScreenVm
        };
        desktop.MainWindow = splashScreen;
        splashScreen.Show();
        try
        {
            splashScreenVm.StartupMessage = "Wellcome to";
            await Task.Delay(1000, splashScreenVm.CancellationToken);
            splashScreenVm.StartupMessage = "Appointment Management system";
            await Task.Delay(2000, splashScreenVm.CancellationToken);
            splashScreenVm.StartupMessage = "lunching uplication";
            await Task.Delay(2000, splashScreenVm.CancellationToken);
        }
        catch (TaskCanceledException)
        {
            splashScreen.Close();
            return;
        }
        // Set up the main window
        desktop.MainWindow = mainWin;
        mainWin.Show();
        
        splashScreen.Close();   
    }














    private void ConfigureViewModels(IServiceCollection services)
    {
        // Register services
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IBusinessServices, BusinessServices>();
        services.AddSingleton<IBSService, BSService>();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=localhost;Database=AppoinmentScheduler;User Id=sa;Password=KarlPogi5758;Encrypt=True;TrustServerCertificate=True;"));
        services.AddSingleton<ISessionService, SessionService>();
        

        // Register ViewModels
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<HomePageViewModel>();
        services.AddTransient<LoginPageViewModel>();
        services.AddTransient<SignUpPageViewModel>();
        services.AddTransient<MessageViewModel>();

        // Register ViewModels Business
        services.AddSingleton<BusinessHomeViewModel>();
        services.AddSingleton<ManagementViewModel>();
        services.AddSingleton<ProfileViewModel>();
        services.AddSingleton<ServiceViewModel>();
      
        

        // Register ViewModels Cleint
         services.AddSingleton<ClientHomeViewModel>();
     
        // Register Messenger
        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        
        
    }

    private void ConfigureViews(IServiceCollection services)
    {
        // Register services
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IBusinessServices, BusinessServices>();
         services.AddSingleton<IBSService, BSService>();
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=localhost;Database=AppoinmentScheduler;User Id=sa;Password=KarlPogi5758;Encrypt=True;TrustServerCertificate=True;"));
        services.AddSingleton<ISessionService, SessionService>();
       

        // Register ViewModels
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<HomePageViewModel>();
        services.AddTransient<LoginPageViewModel>();
        services.AddTransient<SignUpPageViewModel>();
        services.AddTransient<MessageViewModel>();

        // Register ViewModels Business
        services.AddSingleton<BusinessHomeViewModel>();
        services.AddSingleton<ManagementViewModel>();
        services.AddSingleton<ProfileViewModel>();
        services.AddSingleton<ServiceViewModel>();
        
        // Register ViewModels Cleint
        services.AddTransient<ClientHomeViewModel>();
      
        // Register Messenger
        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        
    }
}
