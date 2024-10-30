using CommunityToolkit.Mvvm.ComponentModel;

namespace AppoinmentScheduler.ViewModels;

public partial class MessageViewModel: ViewModelBase
{   
     [ObservableProperty] private string? _message;
    public MessageViewModel(string message)
        {
            _message = message;
        }
         
}