using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels.ClientViewModels
{
    public partial class ClientAppoinmentViewModel : ViewModelBase
    {
        [ObservableProperty]
        private User? _user;

        [ObservableProperty]
        private ObservableCollection<ClientAppointment> _items = new();

        [ObservableProperty]
        private ClientAppointment? _selectedListItem;

        [ObservableProperty]
        private DateTime? _selectedDate;

        private readonly IClientService _clientService;

        public ClientAppoinmentViewModel(IMessenger messenger, IClientService clientService)
        {
            _clientService = clientService;

            messenger.Register<ClientAppoinmentViewModel, UserMessage>(this, (recipient, message) =>
            {
                User = message.Value;
            });
        }

        partial void OnSelectedDateChanged(DateTime? value)
        {
            // This method is automatically called when SelectedDate changes.
            Console.WriteLine($"Selected Date: {value:MM/dd/yyyy}");
        }

        [RelayCommand]
        public async Task EditAsync()
        {
            // Implement edit logic
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            // Implement delete logic
        }
    }
}
