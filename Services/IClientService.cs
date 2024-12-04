
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
        Collection<ClientSubscribed> Selectall(int user_id);
        Task<List<BusinessAppointmentService>> SearchItemsAsync(string searchText);
        List<ClientSubscribed> SearchBydateAsync(string searchText,int userId);
        public Task<List<BusinessAppointmentService>> GetItemsAsync(int page, int pageSize);
        void update();
        void delete(int ServiceID ,int UserID);

        Collection<BusinessSubcriber> SelectallBS(int user_id);
        List<BusinessSubcriber> SearchByDateBusinessSubcriber(string searchText,int userId);

        void Accept(int ClientID , int ServiceID);
        void Done(int ClientID , int ServiceID);
        void Denied(int ClientID , int ServiceID);
    }
}