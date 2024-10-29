using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Microsoft.Extensions.DependencyInjection;
using AppoinmentScheduler.Services;
using AppoinmentScheduler.ViewModels.BusinessViewModels;
using AppoinmentScheduler.ViewModels.ClientViewModels;
using System.Threading.Tasks;
using Models;
using CommunityToolkit.Mvvm.Messaging;
using AppoinmentScheduler.ObjMessages;
using System.Collections.Generic;




namespace AppoinmentScheduler.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;

        private readonly ISessionService _sessionService;

        private User _user { get; set; } 

        private readonly IMessenger _messenger;

        private readonly Dictionary<Type, ViewModelBase> _viewModelCache = new();

        


        public MainWindowViewModel(IServiceProvider serviceProvider, IUserService userService,ISessionService sessionService, IMessenger messenger)
        {
            _serviceProvider = serviceProvider;
            _userService = userService;
            _sessionService = sessionService;

            messenger.Register<MainWindowViewModel, UserMessage>(this, (MainWindowViewModel, message) =>
            {
                MainWindowViewModel._user = message.Value;
                Console.WriteLine("MainWindowViewModel" + message.Value.email);
            });
            _messenger = messenger;
            _ = Initialize();
            
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

            var viewModelType = value.ModelType;

            // Check if instance already exists in cache
            if (!_viewModelCache.TryGetValue(viewModelType, out var instance))
            {
                // Create and cache new instance if it doesn't exist
                instance = _serviceProvider.GetRequiredService(viewModelType) as ViewModelBase;
                if (instance is null) return;

                _userService.SendData(_user);
                _viewModelCache[viewModelType] = instance;
            }

            CurrentPage = instance;
        }


        [RelayCommand] 
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

       public ObservableCollection<ListItemTemplate> Items { get; set; } = new()
        {
            new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular"),
            new ListItemTemplate(typeof(LoginPageViewModel), "ArrowRightRegular"),
            new ListItemTemplate(typeof(SignUpPageViewModel), "HomeRegular")
        };
        
        public void SetCurrentPage(ViewModelBase viewModelBase){
            _userService.SendData(_user);
            CurrentPage = viewModelBase;
            
            
        }

        public void SetView()
        {
            Items.Clear();
            
            if (_user.role == 1) // Business user
            {
                Console.WriteLine("Business");
                Items.Add(new ListItemTemplate(typeof(BusinessHomeViewModel), "HomeRegular"));
                Items.Add(new ListItemTemplate(typeof(ManagementViewModel), "HomeRegular"));
                Items.Add(new ListItemTemplate(typeof(ProfileViewModel), "HomeRegular"));
                Items.Add(new ListItemTemplate(typeof(ServiceViewModel), "HomeRegular"));

                

                if (!_viewModelCache.TryGetValue(typeof(BusinessHomeViewModel), out var businessInstance))
                {
                    businessInstance = _serviceProvider.GetRequiredService<BusinessHomeViewModel>();
                    _viewModelCache[typeof(BusinessHomeViewModel)] = businessInstance;
                }
                SetCurrentPage(businessInstance);
            }
            else if (_user.role == 0) // Client user
            {
                Console.WriteLine("ClientHomeViewModel");
                Items.Add(new ListItemTemplate(typeof(ClientHomeViewModel), "HomeRegular"));
                Items.Add(new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular"));

                if (!_viewModelCache.TryGetValue(typeof(ClientHomeViewModel), out var clientInstance))
                {
                   var clientHomeViewModel = _serviceProvider.GetRequiredService<ClientHomeViewModel>();
                    SetCurrentPage(clientHomeViewModel);
                    _viewModelCache[typeof(ClientHomeViewModel)] = clientInstance;
                }
                SetCurrentPage(clientInstance);
            }
            
            
             
           
        }

        private async Task Initialize()
        {
            if (await _sessionService.SessionLogin(_userService))
            {  
                SetView();
            } 
        }
        


    }



    public class ListItemTemplate
    {
        public ListItemTemplate(Type type, string iconKey)
        {
            ModelType = type;
            Label = type.Name.Replace("PageViewModel", "");
            Label = type.Name.Replace("ViewModel", "");
            App.Current!.TryFindResource(iconKey, out var res);
            ListItemIcon = (StreamGeometry)res!;
        }

        public Type ModelType { get; }
        public string Label { get; }
        public StreamGeometry ListItemIcon { get; }
    }

    
    
}
