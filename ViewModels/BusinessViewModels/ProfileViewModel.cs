
using System;
using System.Threading.Tasks;
using AppoinmentScheduler.ObjMessages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels.BusinessViewModels
{
    public partial class ProfileViewModel : ViewModelBase
    {
        [ObservableProperty] private string? _greeting = "ManagementViewModel";
        public ProfileViewModel()
        {

        }
        [ObservableProperty] private string? _business_Name;
        [ObservableProperty] private string? _business_Description;
        [ObservableProperty] private string? _contact_Information;
        [ObservableProperty] private string? _weekly_Schedule;
        [ObservableProperty] private string? _time_Slots;
        [ObservableProperty] private string? _max_appointment;
        [ObservableProperty] private string? _cancellation_Policy;

        [RelayCommand] private async Task OnsubmitAsync()
        {}
        
    }
}