using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
namespace AppoinmentScheduler.ViewModels.BusinessViewModels
{
       public partial class BusinessHomeViewModel : ViewModelBase
    {
        private User _user;

        [ObservableProperty] private string? _username;

        // Constructor
        public BusinessHomeViewModel(User user)
        {
            this._user = user;
            _username = user.user_name;
        }

    }
}
