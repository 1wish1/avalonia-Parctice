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

    private ObservableCollection<BusinessService> _items;
    private bool _isLoading;
    private int _pageSize = 20;
    private int _currentPage = 0;

    [ObservableProperty] private BusinessService? _selectedListItem;

    public ObservableCollection<BusinessService> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private readonly IBSService _BSService;

    public ClientBookingViewModel(IBSService BSService)
    {
        Items = new ObservableCollection<BusinessService>();
        _BSService = BSService;
        LoadItems();
    }

    [ObservableProperty] private string _searchText;

    public async Task LoadItems()
    {
        if (IsLoading)
            return;

        IsLoading = true;

        // Simulate fetching items in chunks from the database
        var fetchedItems = await _BSService.GetItemsAsync(_currentPage, _pageSize);

        if (fetchedItems != null && fetchedItems.Any())
        {
            // Randomize the order of items
            var random = new Random();
            fetchedItems = fetchedItems.OrderBy(x => random.Next()).ToList();

            // Add the fetched items to the existing collection
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
            var filteredItems = await _BSService.SearchItemsAsync(SearchText);

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


    
    }
}