
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        [ObservableProperty] private int _count;
        [ObservableProperty] private int _total;
       

        public BusinessHomeViewModel(IMessenger messenger, IClientService clientService)
        {
            _clientService =clientService;
            messenger.Register<BusinessHomeViewModel, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
                Items = new ObservableCollection<BusinessSubcriber>(_clientService.SelectallBS(_user.id));
                Count = CountItemsForToday();
                Total = Items.Count;
            });
            Items = new ObservableCollection<BusinessSubcriber>();
        }

        public int CountItemsForToday()
        {
            DateTime today = DateTime.Today;
            int Count = Items.Count(item => item.TimeDate == today.ToString("yyyy-MM-dd"));
            return Count;
        }
        
        
        
        
    }
    
}

