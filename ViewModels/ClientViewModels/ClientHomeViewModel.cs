using System;
using System.Collections.ObjectModel;
using System.Linq;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;

namespace AppoinmentScheduler.ViewModels.ClientViewModels
{
    public partial class ClientHomeViewModel : ViewModelBase
{
    [ObservableProperty] private string? _username;
    [ObservableProperty] private User? _user;
    [ObservableProperty] private ObservableCollection<ClientSubscribed> _items = new();
    [ObservableProperty] private ClientSubscribed? _selectedListItem;

    private readonly IClientService _clientService;

    [ObservableProperty] private int _count;
    [ObservableProperty] private int _total;

    public ClientHomeViewModel(IMessenger messenger, IClientService clientService)
    {
        _clientService = clientService;

        messenger.Register<ClientHomeViewModel, UserMessage>(this, (recipient, message) =>
        {
            _user = message.Value;
            _username = _user?.email;
            Items = new ObservableCollection<ClientSubscribed>(_clientService.Selectall(_user.id));
             Count = CountItemsForToday();
            Total = Items.Count;
        });
        Items = new ObservableCollection<ClientSubscribed>();
    }
    public int CountItemsForToday()
    {
        DateTime today = DateTime.Today;
        int Count = Items.Count(item => item.CA_Time_Date == today.ToString("yyyy-MM-dd"));
        return Count;
    }
}
}
