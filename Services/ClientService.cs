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
        public void delete()
        {
             
        }

        public void insert(ClientAppointment clientAppointment)
        {
            // Using parameterized SQL query to insert a new ClientAppointment into the database
            _context.Database.ExecuteSqlRaw(
                @"INSERT INTO ClientAppointment (Userid, ServiceID, Time_Date, Status, Description)
                VALUES ({0}, {1}, {2}, {3}, {4})",
                clientAppointment.Userid,
                clientAppointment.ServiceID,
                clientAppointment.Time_Date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                clientAppointment.Status,
                clientAppointment.Description
            );

            // Optionally save changes (not necessary since ExecuteSqlRaw directly affects the database)
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
        public async Task<List<ClientAppointment>> SearchBydateAsync(string searchText)
        {
            // Query the database using EF Core with a LINQ query
            var services = await _context.ClientAppointment
                .FromSqlRaw("SELECT * FROM ClientAppointment WHERE Time_Date == {0}", searchText)
                .ToListAsync();
            

            return services;
        }
    }
}