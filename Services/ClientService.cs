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
                @"INSERT INTO ClientAppointment (Client_Account, ServiceID, Time_Date, Status, Description)
                VALUES ({0}, {1}, {2}, {3}, {4})",
                clientAppointment.Client_Account,
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
    }
}