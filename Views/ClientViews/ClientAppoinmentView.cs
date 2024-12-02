using System;
using Avalonia;
using Avalonia.Controls;

namespace AppoinmentScheduler.Views.ClientViews
{
    public partial class ClientAppoinmentView : UserControl
    {
        public ClientAppoinmentView()
        {
            InitializeComponent();
           
            
        }
        public void click(){
             Console.WriteLine(calendar.SelectedDate);
        }
    }
}
