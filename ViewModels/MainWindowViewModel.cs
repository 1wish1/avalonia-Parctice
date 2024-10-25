﻿using System;
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




namespace AppoinmentScheduler.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        
        

        public MainWindowViewModel(IServiceProvider serviceProvider, IUserService userService)
        {
            _serviceProvider = serviceProvider;
            _userService = userService;
            
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

       public ObservableCollection<ListItemTemplate> Items { get; set; } = new()
        {
            new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular"),
            new ListItemTemplate(typeof(LoginPageViewModel), "ArrowRightRegular"),
            new ListItemTemplate(typeof(SignUpPageViewModel), "HomeRegular")
           // new ListItemTemplate(typeof(DashBoardViewModel), "HomeRegular")
        };
        
        
        // set the current page base on token
        public void SetCurrentPage(ViewModelBase viewModelBase){
            CurrentPage = viewModelBase;
        }

        public void SetViewAsync(User user){
            Console.WriteLine("SetViewAsync name");
            Console.WriteLine(user.user_name);
            
            if(user.role == 1){//bussines
                Items = new(){
                    new ListItemTemplate(typeof(BusinessHomeViewModel), "HomeRegular"),
                };

                SetCurrentPage(new BusinessHomeViewModel( user));

            }else if(user.role == 0){
                Items = new(){
                    new ListItemTemplate(typeof(ClientHomeViewModel), "HomeRegular"),
                };
                SetCurrentPage(new ClientHomeViewModel(user));
            }
         
        
            
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
