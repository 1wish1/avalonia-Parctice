using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Controls.Converters;


namespace AppoinmentScheduler.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isPaneOpen = true;
    [ObservableProperty] private ViewModelBase _currentPage = new HomePageViewModel();

    [ObservableProperty] private ListItemTemplate? _selectedListItem;


    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance is null) return;
        CurrentPage = (ViewModelBase)instance;
    }
    [RelayCommand] private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    public ObservableCollection<ListItemTemplate> Items {get;} = new()
    {
        new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular"),
        new ListItemTemplate(typeof(LoginPageViewModel), "ArrowRightRegular"),
    };

}

public class ListItemTemplate
{
    public ListItemTemplate(Type type, string iconKey )
    {
        ModelType = type;
        Label = type.Name.Replace("PageViewModel","");
        App.Current!.TryFindResource(iconKey, out var res);
        ListItemIcon = (StreamGeometry)res!;
        
    }
    public Type ModelType{get;} 
    public string Label{get;} 
    public StreamGeometry ListItemIcon{get;}

    
}
