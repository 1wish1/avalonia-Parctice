
using System;
using System.Collections.ObjectModel;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels.BusinessViewModels
{
    public partial class ManagementViewModel : ViewModelBase
    {
        [ObservableProperty] private User? _user;

        [ObservableProperty] private ObservableCollection<BusinessSubcriber> _items = new();

        [ObservableProperty] private BusinessSubcriber? _selectedListItem;

        [ObservableProperty] private DateTime? _selectedDate;
        [ObservableProperty] private string? _error;
        private readonly IClientService _clientService;

        public ManagementViewModel(IMessenger messenger,IClientService clientService)
        {
            _clientService = clientService;
             messenger.Register<ManagementViewModel, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
               
            });     
        }
        
        partial void OnSelectedDateChanged(DateTime? value)
        {
            LoadItem(value);
        }

        public void LoadItem(DateTime? value){

            var filteredItems = _clientService.SearchByDateBusinessSubcriber(value.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),_user.id);

            Items.Clear();
            foreach (var item in filteredItems)
            {
                Items.Add(item);
            }
        }
        
    }
}

