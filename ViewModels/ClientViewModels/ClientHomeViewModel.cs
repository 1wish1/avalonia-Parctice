using System;
using System.Collections.ObjectModel;
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
    [ObservableProperty] private ObservableCollection<ClientAppointment> _items = new();
    [ObservableProperty] private ClientAppointment? _selectedListItem;

    private readonly IClientService _clientService;

    public ClientHomeViewModel(IMessenger messenger, IClientService clientService)
    {
        _clientService = clientService;

        messenger.Register<ClientHomeViewModel, UserMessage>(this, (recipient, message) =>
        {
            _user = message.Value;
            _username = _user?.email;
            Items = new ObservableCollection<ClientAppointment>(_clientService.Selectall(_user.id));
            
        });
        Items = new ObservableCollection<ClientAppointment>();
    }
}
}
