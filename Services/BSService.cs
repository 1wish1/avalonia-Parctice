using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.ViewModels.ClientViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AppoinmentScheduler.Services
{
    public class BSService : IBSService
    {
        private readonly AppDbContext _context;
        private readonly IBusinessServices _businessServices;
        private BusinessAppointment businessAppointment{ get; set; }
        private User? _user { get; set; }
        public BSService(AppDbContext context,IBusinessServices BusinessService,IMessenger messenger){
            
            messenger.Register<BSService, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
            });
            _context = context;
            _businessServices = BusinessService;
        }

        public BusinessService addService(BusinessService businessService, int id)
        {       
                businessAppointment = _businessServices.Select(id);
                businessService.businessAppointment = businessAppointment;

                _context.BusinessService.Add(businessService);
                _context.SaveChanges();
                Console.WriteLine(businessService.ServiceId);
                return Select(businessService.ServiceId);

        }

        public void deleteService(int id)
        {
             _context.Database.ExecuteSqlRaw(
                "DELETE FROM ClientAppointment WHERE ServiceID = {0}",
                id
                );
            var service = _context.BusinessService
                .Include(bs => bs.businessAppointment) // Include the navigation property
                .FirstOrDefault(bs => bs.ServiceId == id);

            if (service != null)
            {
                
                _context.BusinessService.Remove(service);
                
                _context.SaveChanges();
            }
        }

        public void updateService(BusinessService businessService)
        {
           
            var service = _context.BusinessService
                .FirstOrDefault(bs => bs.ServiceId == businessService.ServiceId);

            if (service != null)
            {
                service.Name = businessService.Name;
                service.Description = businessService.Description;
                service.Price = businessService.Price;
                service.Duration = businessService.Duration;
                service.Availability = businessService.Availability;

                _context.SaveChanges();
            }
        }

     
        public Collection<BusinessService> Selectall()
        {   
            try
            {
                var services = _context.BusinessService
                .FromSqlRaw(@"SELECT DISTINCT bs.*
                FROM BusinessService bs
                INNER JOIN BusinessAppointment ba ON bs.BusinessAppointmentID = ba.BusinessAppointment_ID
                INNER JOIN Users u ON ba.Business_Account = u.id
                WHERE u.id = {0};", _user.id)
                .ToList();
                if (services == null){
                    return new Collection<BusinessService>();
                }else{
                    return new Collection<BusinessService>(services);
                }    
            }
            catch (Exception e)
            {

                return new Collection<BusinessService>();
            }
            
            
        }

        public BusinessService Select(int id)
        {
            var service = _context.BusinessService
                .Where(bs => bs.ServiceId == id)
                .FirstOrDefault();
            return service;
        }

       

        

    }
}