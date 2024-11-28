using Models;

namespace AppoinmentScheduler.Services
{
    public interface IBusinessServices
    {
        void addBusinessService(BusinessAppointment businessAppointment);
        void updatedBusinessService(BusinessAppointment businessAppointment);

        BusinessAppointment Select();
    }
}
