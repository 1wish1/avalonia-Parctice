using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppoinmentScheduler.ViewModels;

public partial class SplashScreenViewModel: ViewModelBase
{
      [ObservableProperty]
    private string _startupMessage = "Starting application...";

    public void Cancel()
    {
        StartupMessage = "Cancelling...";
        _cts.Cancel();
    }

    private readonly CancellationTokenSource _cts = new();

    public CancellationToken CancellationToken => _cts.Token;

}