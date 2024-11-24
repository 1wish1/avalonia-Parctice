
using System;
using System.Threading.Tasks;
using AppoinmentScheduler.ObjMessages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels.BusinessViewModels
{
    public partial class ServiceViewModel : ViewModelBase
    {
        [ObservableProperty] private string? _greeting = "ManagementViewModel";
        [ObservableProperty] private string? _name;
        [ObservableProperty] private string? _description;
        [ObservableProperty] private string? _price;
        [ObservableProperty] private string? _duration;
        [ObservableProperty] private string? _availability;

 
        public ServiceViewModel()
        {

        }
        [RelayCommand] private async Task OnsubmitAsync()
        {}
      
        
    }
}