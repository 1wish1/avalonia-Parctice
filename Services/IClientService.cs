
using Models;

namespace AppoinmentScheduler.Services
{
    public interface IClientService
    {
        void insert(ClientAppointment clientService);
        void update();
        void delete();
    }
}