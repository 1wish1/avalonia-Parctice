using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AppoinmentScheduler.ViewModels;
using AppoinmentScheduler.Views;
using System.Threading.Tasks;

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
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
           

            var splashScreenVm = new SplashScreenViewModel();
            var splashScreen = new SplashScreenView{
                DataContext = splashScreenVm
            };
            desktop.MainWindow = splashScreen;
            splashScreen.Show();
            try{
                splashScreenVm.StartupMessage = "Searching for device...";
                await Task.Delay(1000, splashScreenVm.CancellationToken);
                splashScreenVm.StartupMessage = "Connecting to device #1...";
                await Task.Delay(2000, splashScreenVm.CancellationToken);
                splashScreenVm.StartupMessage = "Configuring device...";
                await Task.Delay(2000, splashScreenVm.CancellationToken);
            }
            catch(TaskCanceledException){
                splashScreen.Close();
                return;
            }
            var mainWin = new MainWindow{
                DataContext = new MainWindowViewModel(),
            };
            desktop.MainWindow = mainWin;
            mainWin.Show();
            splashScreen.Close();
        }

      //base.OnFrameworkInitializationCompleted();
    }
}