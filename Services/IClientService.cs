
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Models;

namespace AppoinmentScheduler.Services
{
    public interface IClientService
    {
        void insert(ClientAppointment clientService);
        Collection<ClientAppointment> Selectall(int user_id);
        Task<List<BusinessService>> SearchItemsAsync(string searchText);
        Task<List<ClientAppointment>> SearchBydateAsync(string searchText);
        public Task<List<BusinessService>> GetItemsAsync(int page, int pageSize);
        void update();
        void delete();
    }
}