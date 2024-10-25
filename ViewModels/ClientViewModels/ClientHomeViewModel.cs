using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;

namespace AppoinmentScheduler.ViewModels.ClientViewModels
{
    public partial class ClientHomeViewModel : ViewModelBase
    {
        private User _user;
        [ObservableProperty] private string? _username;
        public ClientHomeViewModel(User user)
        {
            this._user = user;
            _username = user.user_name;
        }
    }
}
