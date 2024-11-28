using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        

    }
}