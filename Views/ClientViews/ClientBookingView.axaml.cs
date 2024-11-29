using AppoinmentScheduler.ViewModels.ClientViewModels;
using Avalonia.Controls;

namespace AppoinmentScheduler.Views.ClientViews
{
    public partial class ClientBookingView : UserControl
    {
        public ClientBookingView()
        {
            InitializeComponent();
        }
    private async void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;

            if (scrollViewer != null)
            {
                // Get the current vertical scroll position and the total height
                double verticalOffset = scrollViewer.Offset.Y;
                double extentHeight = scrollViewer.Extent.Height;

                // Check if the user has scrolled to the bottom
                if (verticalOffset >= extentHeight - scrollViewer.Viewport.Height)
                {
                    // Call the ViewModel to load more items
                    var viewModel = (ClientBookingViewModel)DataContext;
                    await viewModel.LoadItems();
                }
            }
        }

    }
}
