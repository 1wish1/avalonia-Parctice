
using System;
using AppoinmentScheduler.ObjMessages;
using CommunityToolkit.Mvvm.ComponentModel;
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
        
    }
}