using System;
using System.Linq;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.ViewModels.BusinessViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AppoinmentScheduler.Services
{
    public class BusinessServices : IBusinessServices
    {
        private readonly AppDbContext _context;
        private User? _user { get; set; }

        
        public BusinessServices(AppDbContext context,IMessenger messenger){
            
            messenger.Register<BusinessServices, UserMessage>(this, (recipient, message) =>
            {
                _user = message.Value;
            });
            _context = context;
        }
        
        public void  addBusinessService(BusinessAppointment businessAppointment)
        {   
           var count = _context.BusinessAppointment.FromSqlRaw(
            "SELECT * FROM BusinessAppointment WHERE Business_Account = {0}",
           _user.id
            ).Count();
            
            if (count > 0){
                updatedBusinessService(businessAppointment);
            }else{
                businessAppointment.User = _user;
                _context.BusinessAppointment.Add(businessAppointment);
                _context.SaveChanges();
            }
        }

        public void updatedBusinessService(BusinessAppointment businessAppointment)
        {
            _context.Database
            .ExecuteSqlRaw(
                @"UPDATE BusinessAppointment SET Business_name = {0}, Address = {1}, contact_information = {2}, Organization_Office = {3}, Description = {4}, Schdule = {5}, Time_Slots = {6}, Max_appointment = {7}, Cancellation_Policy = {8} WHERE Business_Account = {9}", 
                    businessAppointment.Business_name,
                    businessAppointment.Address,
                    businessAppointment.contact_information,
                    businessAppointment.Organization_Office,
                    businessAppointment.Description,
                    businessAppointment.Schdule,
                    businessAppointment.Time_Slots,
                    businessAppointment.Max_appointment,
                    businessAppointment.Cancellation_Policy,
                    _user.id
                );
            _context.SaveChanges();
        }

        public BusinessAppointment Select(int id)
        {   
            try
            {
                BusinessAppointment businessAppointment = _context.BusinessAppointment.FromSqlRaw(@"SELECT * FROM BusinessAppointment WHERE Business_Account = {0} ", id).FirstOrDefault();
                return businessAppointment;
            }
            catch (System.Exception)
            {
                
                throw;
            }
                
            
             
        }

    }


}