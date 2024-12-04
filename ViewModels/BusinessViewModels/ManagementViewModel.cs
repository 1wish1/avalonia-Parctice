
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
                Error = " ";
            });     
        }
        
        partial void OnSelectedDateChanged(DateTime? value)
        {
            LoadItem(value);
            Error = " ";
        }

        public void LoadItem(DateTime? value){

            var filteredItems = _clientService.SearchByDateBusinessSubcriber(value.Value.ToString("yyyy-MM-dd"),_user.id);

            Items.Clear();
            foreach (var item in filteredItems)
            {
                Items.Add(item);
            }
        }
        
        [RelayCommand] private void Accept()
        {
            try
            {
                _clientService.Accept(SelectedListItem.ClientID,SelectedListItem.ServiceID);
                Error = "Done Change Status to Accept";
               
            }
            catch (Exception e)
            {
                Error = "Select your ClientAppointment";
            }
           
        }
        [RelayCommand] private void Denied()
        {
            try{
                _clientService.Denied(SelectedListItem.ClientID,SelectedListItem.ServiceID);
                Error = "Done Deletion";
                LoadItem(SelectedDate);
            }
            catch (Exception e)
            {
                
                 Error = "Select your ClientAppointment";
            }
            
        }
        [RelayCommand] private void Done(){
            try{
                _clientService.Done(SelectedListItem.ClientID,SelectedListItem.ServiceID);
                Error = "Nice You've Done";
                LoadItem(SelectedDate);
            }
            catch (Exception e)
            {
                
                 Error = "Select your ClientAppointment";
            }
        }

        
    }
}

