
using System;
using System.Threading.Tasks;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;

namespace AppoinmentScheduler.ViewModels.BusinessViewModels
{
    public partial class ProfileViewModel : ViewModelBase
    {
        [ObservableProperty] private string? _greeting = "ManagementViewModel";

        private readonly IBusinessServices _businessServices;
        [ObservableProperty] private User? _user;
        public ProfileViewModel(IBusinessServices BusinessService,IMessenger messenger)
        {
            _businessServices = BusinessService;
             messenger.Register<ProfileViewModel, UserMessage>(this, (recipient, message) =>
            {
                Error = string.Empty;
                _user = message.Value;
                Select(_user.id);

            }); 
            
            


        }
        [ObservableProperty] private string? _business_Name;
        [ObservableProperty] private string? _address;
        [ObservableProperty] private string? _business_Description;
        [ObservableProperty] private string? _contact_Information;
        [ObservableProperty] private string? _weekly_Schedule;
        [ObservableProperty] private string? _time_Slots;
        [ObservableProperty] private int? _max_appointment;
        [ObservableProperty] private string? _cancellation_Policy;
        [ObservableProperty] private string? _organization_Office;
        [ObservableProperty] private string? _error;


        [RelayCommand]
        private async Task OnsubmitAsync()
        {
            Error = string.Empty;

            if (!ValidateInputs())
            {
                return;
            }
            BusinessAppointment businessAppointment = new BusinessAppointment()
            {
                Business_name = Business_Name,
                Address = Address,
                contact_information = Contact_Information,
                Organization_Office = Organization_Office,
                Description = Business_Description,
                Schdule = Weekly_Schedule,
                Time_Slots = Time_Slots,
                Max_appointment = (int)Max_appointment,
                Cancellation_Policy = Cancellation_Policy

            };

            _businessServices.addBusinessService(businessAppointment);
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Business_Name) || Business_Name.Length < 3)
            {
                Error = "Invalid Business Name";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Address) || Address.Length < 3)
            {
                Error = "Invalid Address Description";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Organization_Office) || Organization_Office.Length < 3)
            {
                Error = "Invalid Address Organition Office ";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Business_Description) || Business_Description.Length < 3)
            {
                Error = "Invalid Business Description";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Contact_Information) || Contact_Information.Length < 3)
            {
                Error = "Invalid Contact Information";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Weekly_Schedule) || Weekly_Schedule.Length < 3)
            {
                Error = "Invalid Weekly Schedule";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Time_Slots) || Time_Slots.Length < 3)
            {
                Error = "Invalid Time Slots";
                return false;
            }
            if (!Max_appointment.HasValue || Max_appointment.Value < 0)
            {
                Error = "Invalid Max Appointments";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Cancellation_Policy) || Cancellation_Policy.Length < 3)
            {
                Error = "Invalid Cancellation Policy";
                return false;
            }
            return true;
        }

        private void Select(int userID){
            try
            {
                BusinessAppointment businessAppointment = _businessServices.Select(userID);
                Business_Name = businessAppointment.Business_name;
                Address = businessAppointment.Address;
                Contact_Information = businessAppointment.contact_information;
                Organization_Office = businessAppointment.Organization_Office;
                Business_Description = businessAppointment.Description;
                Weekly_Schedule = businessAppointment.Schdule;
                Time_Slots = businessAppointment.Time_Slots;
                Max_appointment = businessAppointment.Max_appointment;
                Cancellation_Policy = businessAppointment.Cancellation_Policy;
            }
            catch (Exception e)
            {
                Business_Name = "";
                Address = "";
                Contact_Information = "";
                Organization_Office = "";
                Business_Description = "";
                Weekly_Schedule = "";
                Time_Slots = "";
                Max_appointment = null;
                Cancellation_Policy = "";
            }
        }

    }
}