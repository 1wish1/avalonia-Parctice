using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models; 

namespace AppoinmentScheduler.ViewModels.ClientViewModels
{
    public partial class ClientBookingViewModel : ViewModelBase
    {

    [ObservableProperty] private ObservableCollection<BusinessService> _items;
    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private BusinessService? _selectedListItem;

   [ObservableProperty] private DateTime _selectedDate;
   
   [ObservableProperty] private string _description;

   [ObservableProperty] private string _error;
    private readonly IClientService _ClientService;

    private User? _user { get; set; }
    private int _pageSize = 20;
    private int _currentPage = 0;

                    

    public ClientBookingViewModel(IClientService clientService,IMessenger messenger)
    {
        messenger.Register<ClientBookingViewModel, UserMessage>(this, (recipient, message) =>
        {
            _user = message.Value;
            SearchAsync();
        });
        
        Items = new ObservableCollection<BusinessService>();
        _ClientService = clientService;
        SelectedDate = DateTime.Today;
    }

    [ObservableProperty] private string _searchText;

    public async Task LoadItems()
    {
        if (IsLoading)
            return;

        IsLoading = true;

        // pagenation 
        var fetchedItems = await _ClientService.GetItemsAsync(_currentPage, _pageSize);

        if (fetchedItems != null && fetchedItems.Any())
        {
            // Item random 
            var random = new Random();
            fetchedItems = fetchedItems.OrderBy(x => random.Next()).ToList();

            // Add item to the collection 
            foreach (var item in fetchedItems)
            {
                Items.Add(item);
            }

            _currentPage++;
        }

        IsLoading = false;
    }

    

    [RelayCommand]  public async Task SearchAsync()
    {
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            // If there is search text, fetch the filtered items
            var filteredItems = await _ClientService.SearchItemsAsync(SearchText);

            // Clear current items and add the filtered results
            Items.Clear();
            foreach (var item in filteredItems)
            {
                Items.Add(item);
            }
        }
        else
        {
            // If search text is empty or null, reload paginated items
            _currentPage = 0; // Reset page number for a fresh load
            await LoadItems();
        }
    }




    private bool CanAdd()
    {
            if(string.IsNullOrWhiteSpace(Description)){
                Error = "Must Have Description";
                return false;
            }
            if(SelectedDate.Date < DateTime.Today){
                Error = "Must be Tomorrow or Today";
                return false;
            }
            if(SelectedListItem == null){
                Error = "Select Service";
                return false;
            }

       return true;
    }








    [RelayCommand]  public async Task AddAsync(){
        if (!CanAdd()){
            return;
        }
        try{

        
            ClientAppointment clientAppointment = new ClientAppointment(){
                Userid = _user.id,
                ServiceID = SelectedListItem.ServiceId,
                Time_Date = SelectedDate,
                Status = "pending",
                Description = Description
            };
            _ClientService.insert(clientAppointment);
        }
        catch(Exception e){
            Error = "Pls add your input";
        }
    }



    
    }
}