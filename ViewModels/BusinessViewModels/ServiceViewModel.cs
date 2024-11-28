
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels.BusinessViewModels
{
    public partial class ServiceViewModel : ViewModelBase
    {
        [ObservableProperty] private string? _greeting = "ManagementViewModel";
        [ObservableProperty] private string? _name;
        [ObservableProperty] private string? _description;
        [ObservableProperty] private int? _price;
        [ObservableProperty] private int? _duration;
        [ObservableProperty] private int? _availability;
        [ObservableProperty] private string? _error;
        [ObservableProperty] private ObservableCollection<BusinessService>  _Items ;
        [ObservableProperty] private BusinessService? _selectedListItem;

        private readonly IBSService _BSService;
        private User? _user { get; set; }
                    
        public ServiceViewModel(IBSService BSService,IMessenger messenger)
        {
            messenger.Register<ServiceViewModel, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
                Items = new ObservableCollection<BusinessService>(_BSService.Selectall());
            });
            _BSService = BSService;
        }

        [RelayCommand]
        private void Edit()
        {   
            if (!ValidateInputs()){
                return;
            }
             
            SelectedListItem.Name = Name;
            SelectedListItem.Description = Description;
            SelectedListItem.Price = (int)Price;
            SelectedListItem.Duration = (int)Duration;
            SelectedListItem.Availability = (int)Availability;
            int index = Items.IndexOf(SelectedListItem);
            _BSService.updateService(SelectedListItem);
            Items[index] = SelectedListItem;

            
        }
        [RelayCommand]
        private void Delete()
        {
            Console.WriteLine(SelectedListItem.ServiceId);
            _BSService.deleteService(SelectedListItem.ServiceId);
            Items.Remove(SelectedListItem);
        }

        

        [RelayCommand] private async Task OnaddAsync()
        {
            if (!ValidateInputs()){
                return;
            }
            BusinessService businessService = new BusinessService(){
                Name = Name,
                Description = Description,
                Price = (int)Price,
                Duration = (int)Duration,
                Availability = (int)Availability
            };
            
            Items.Add(_BSService.addService(businessService));
        }



        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Length < 3)
            {
                Error = "Invalid Service Name" ;
                return false;
            }
            if (string.IsNullOrWhiteSpace(Description) || Description.Length < 3)
            {
                Error = "Invalid Service Description" ;
                return false;
            }
            if (Price < 0 || Price.GetType() != typeof(int) )
            {
                Error = "Invalid Service Price" ;
                return false;
            }
            if (Duration < 0 || Duration.GetType() != typeof(int) )
            {
                Error = "Invalid Service Duration" ;
                return false;
            }
            if (Availability < 0 || Availability.GetType() != typeof(int) )
            {
                Error = "Invalid Service Availability" ;
                return false;
            }
            
           
            return true;
        }

    }

    
}