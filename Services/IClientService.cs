
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
        List<ClientAppointment> SearchBydateAsync(string searchText,int userId);
        public Task<List<BusinessService>> GetItemsAsync(int page, int pageSize);
        void update();
        void delete(int ServiceID ,int UserID);

        Collection<BusinessSubcriber> SelectallBS(int user_id);
        List<BusinessSubcriber> SearchByDateBusinessSubcriber(string searchText,int userId);
    }
}