using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Models;

namespace AppoinmentScheduler.Services
{
    public interface IBSService
    {
        BusinessService addService(BusinessService businessService);
        void deleteService(int id);
        
        public void updateService(BusinessService businessService);

        Collection<BusinessService> Selectall();

        BusinessService Select(int id);
        public Task<List<BusinessService>> GetItemsAsync(int page, int pageSize);

        public  Task<List<BusinessService>> SearchItemsAsync(string searchText);


        

    }
}