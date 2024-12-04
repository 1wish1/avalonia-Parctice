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
        [ObservableProperty] private User? _user;

        [ObservableProperty] private ObservableCollection<ClientAppointment> _items = new();

        [ObservableProperty] private ClientAppointment? _selectedListItem;

        [ObservableProperty] private DateTime? _selectedDate;
        [ObservableProperty] private string? _error;

        private readonly IClientService _clientService;

        public ClientAppoinmentViewModel(IMessenger messenger, IClientService clientService)
        {
            _clientService = clientService;

            messenger.Register<ClientAppoinmentViewModel, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
                Error = "";
            });
          
            
        }

        partial void OnSelectedDateChanged(DateTime? value)
        {
            Error = "";
            LoadItem(value);
            
        }
        public void LoadItem(DateTime? value){
            var filteredItems = _clientService.SearchBydateAsync(value.Value.ToString("yyyy-MM-dd"),_user.id);

            Items.Clear();
            foreach (var item in filteredItems)
            {
                
                Items.Add(item);
            }
            
        }



        [RelayCommand]
        public async Task DeleteAsync()
        {

            try
            {
                _clientService.delete(SelectedListItem.ServiceID,_user.id); 
                LoadItem(SelectedDate);
                Error = "Done Delition"; 
            }
            catch (Exception e )
            {
                Error = e.Message;
            }
            
        }



        
    }
}
