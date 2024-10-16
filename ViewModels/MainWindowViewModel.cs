using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace AppoinmentScheduler.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [ObservableProperty] 
        private bool _isPaneOpen = true;

        [ObservableProperty] 
        private ViewModelBase _currentPage = new HomePageViewModel();

        [ObservableProperty] 
        private ListItemTemplate? _selectedListItem;

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null) return;
            
            // Use the service provider to create an instance of the ViewModel
            var instance = _serviceProvider.GetRequiredService(value.ModelType) as ViewModelBase;
            if (instance is null) return;

            CurrentPage = instance;
        }

        [RelayCommand] 
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        public ObservableCollection<ListItemTemplate> Items { get; } = new()
        {
            new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular"),
            new ListItemTemplate(typeof(LoginPageViewModel), "ArrowRightRegular"),
            new ListItemTemplate(typeof(SignUpPageViewModel), "HomeRegular"),
        };
        public void SetCurrentPage(ViewModelBase viewModelBase){
            CurrentPage = viewModelBase;
        }
    }

    public class ListItemTemplate
    {
        public ListItemTemplate(Type type, string iconKey)
        {
            ModelType = type;
            Label = type.Name.Replace("PageViewModel", "");
            App.Current!.TryFindResource(iconKey, out var res);
            ListItemIcon = (StreamGeometry)res!;
        }

        public Type ModelType { get; }
        public string Label { get; }
        public StreamGeometry ListItemIcon { get; }
    }
    
}
