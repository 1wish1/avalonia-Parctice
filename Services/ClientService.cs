using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AppoinmentScheduler.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;
        public ClientService(AppDbContext context){
            _context = context;

        }
        public void delete(int ServiceID ,int UserID)
        {
            try{
            _context.Database.ExecuteSqlRaw(
                "DELETE FROM ClientAppointment WHERE ServiceID = {0} AND Userid = {1}",
                ServiceID, UserID
                );
            }catch(Exception e){
                throw new InvalidOperationException("Invalid Deletion");
            }
        }

        public void insert(ClientAppointment clientAppointment)
        {

            var serviceExists = _context.ClientAppointment
                                 .FromSqlRaw("SELECT * FROM ClientAppointment WHERE ServiceID = {0}", clientAppointment.ServiceID)
                                 .Any();

            if (serviceExists)
            {
                throw new InvalidOperationException("You are allredy subscribe to the Service");
            }
                _context.Database.ExecuteSqlRaw(
                @"INSERT INTO ClientAppointment (Userid, ServiceID, Time_Date, Status, Description)
                VALUES ({0}, {1}, {2}, {3}, {4})",
                clientAppointment.Userid,
                clientAppointment.ServiceID,
                clientAppointment.Time_Date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                clientAppointment.Status,
                clientAppointment.Description
            );

            _context.SaveChanges();
        }


        public void update()
        {
            
        }
        public Collection<ClientAppointment> Selectall(int user_id)
        {   
            try
            {
                var services = _context.ClientAppointment
                .FromSqlRaw(@"SELECT * from ClientAppointment WHERE Userid = {0};",user_id)
                .ToList();
                
                if (services == null){
                    return new Collection<ClientAppointment>();
                }else{
                    return new Collection<ClientAppointment>(services);
                }    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Collection<ClientAppointment>();
            }
            
            
        }
        public async Task<List<BusinessService>> SearchItemsAsync(string searchText)
        {
            // Query the database using EF Core with a LINQ query
            var services = await _context.BusinessService
                .Where(bs => bs.Name.Contains(searchText))
                .ToListAsync();

            return services;
        }
         public async Task<List<BusinessService>> GetItemsAsync(int page, int pageSize)
        {
            // Fetch items from the database with pagination
            var items = await _context.BusinessService
                .OrderBy(b => b.ServiceId)  // Order by a relevant field, e.g., ServiceId
                .Skip(page * pageSize)      // Skip the number of items based on the page
                .Take(pageSize)             // Take the page size amount of items
                .ToListAsync();             // Execute the query and convert it to a list

            return items;
        }
        public  List<ClientAppointment> SearchBydateAsync(string searchText,int userId)
        {
            // Query the database using EF Core with a LINQ query
             List<ClientAppointment>? services = _context.ClientAppointment
                .FromSqlRaw("SELECT * FROM ClientAppointment WHERE Time_Date = {0} AND Userid = {1};", searchText,userId)
                .ToList();
            return services;
        }

        public Collection<BusinessSubcriber> SelectallBS(int user_id)
        {
            try
            {
                var query = @"
                            SELECT 
                                ca.Time_Date AS TimeDate,
                                ca.Description,
                                ca.status AS Status,
                                ca_acc.email AS ClientEmail,
                                ca_acc.user_name AS ClientUserName,
                                bs.name AS BusinessServiceName,
                                bs.Description AS BusinessServiceDescription,
                                bs.Price,
                                bs.Duration,
                                bs.Availability
                            FROM ClientAppointment ca
                            JOIN BusinessService bs ON ca.ServiceID = bs.ServiceID
                            JOIN BusinessAppointment ba ON bs.BusinessAppointmentID = ba.BusinessAppointment_ID
                            JOIN users ba_acc ON ba.Business_Account = ba_acc.id
                            JOIN users ca_acc ON ca_acc.id = ca.Userid
                            WHERE ba_acc.id = {0} and ca.status != 'Denied' ";

                var appointments = _context.BusinessSubcriber
                    .FromSqlRaw(query, 18012)
                    .ToList();
                if (appointments == null){
                    return new Collection<BusinessSubcriber>();
                }else{
                    return new Collection<BusinessSubcriber>(appointments);
                }    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Collection<BusinessSubcriber>();
            }
        }

        public List<BusinessSubcriber> SearchByDateBusinessSubcriber(string searchText, int userId)
        {
            var query = @"
                            SELECT 
                                ca.Time_Date AS TimeDate,
                                ca.Description,
                                ca.status AS Status,
                                ca_acc.email AS ClientEmail,
                                ca_acc.user_name AS ClientUserName,
                                bs.name AS BusinessServiceName,
                                bs.Description AS BusinessServiceDescription,
                                bs.Price,
                                bs.Duration,
                                bs.Availability
                            FROM ClientAppointment ca
                            JOIN BusinessService bs ON ca.ServiceID = bs.ServiceID
                            JOIN BusinessAppointment ba ON bs.BusinessAppointmentID = ba.BusinessAppointment_ID
                            JOIN users ba_acc ON ba.Business_Account = ba_acc.id
                            JOIN users ca_acc ON ca_acc.id = ca.Userid
                            WHERE ba_acc.id = {0} and ca.Time_Date = {1} and ca.status != 'Denied';

";
            var appointments = _context.BusinessSubcriber
                    .FromSqlRaw(query, userId,searchText)
                    .ToList();
            return appointments;

        }
    }
}