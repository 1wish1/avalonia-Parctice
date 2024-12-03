
using System;
using System.Collections.ObjectModel;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels.BusinessViewModels
{
    public partial class BusinessHomeViewModel : ViewModelBase
    {
        [ObservableProperty] private string? _username;
        [ObservableProperty] private User? _user;

        [ObservableProperty] private ObservableCollection<BusinessSubcriber> _items = new();

        [ObservableProperty] private BusinessSubcriber? _selectedListItem;

        private readonly IClientService _clientService;
       

        public BusinessHomeViewModel(IMessenger messenger, IClientService clientService)
        {
            _clientService =clientService;
            messenger.Register<BusinessHomeViewModel, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
                Items = new ObservableCollection<BusinessSubcriber>(_clientService.SelectallBS(_user.id));
            });
            Items = new ObservableCollection<BusinessSubcriber>();
             
        }
        
        
        
        
    }
    
}

