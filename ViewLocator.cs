using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AppoinmentScheduler.ViewModels;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using AppoinmentScheduler.Views;

namespace AppoinmentScheduler;

public class ViewLocator : IDataTemplate 
{
    private readonly Dictionary<Type, Func<Control?>> _locator = new();
    public ViewLocator(){
        RegisterViewFactory<MainWindowViewModel , MainWindow>();

    }

    public Control? Build(object? data)
    {
        if (data is null)
            return null;
        
        var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            var control = (Control)Activator.CreateInstance(type)!;
            control.DataContext = data;
            return control;
        }
        
        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
    private void RegisterViewFactory<TViewModel, TView>()
        where TViewModel : class
        where TView : Control
        => _locator.Add(
            typeof(TViewModel),
            Design.IsDesignMode
                ? Activator.CreateInstance<TView>
                : Ioc.Default.GetService<TView>);
}
