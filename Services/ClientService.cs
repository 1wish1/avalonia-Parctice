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
                clientAppointment.Time_Date,
                clientAppointment.Status,
                clientAppointment.Description
            );

            _context.SaveChanges();
        }


        public void update()
        {
            
        }
        public Collection<ClientSubscribed> Selectall(int user_id)
        {   
            try
            {
                var services = _context.ClientSubscribed
                .FromSqlRaw(@"SELECT 
                CA.[Description] as CA_Description,
                CA.[status] as CA_Status,
                CA.Time_Date as CA_Time_date,
                BS.ServiceId as BS_ServiceID,
                BS.Name as BS_Name,
                BS.[Description] as BS_Description,
                BS.Price as BS_Price,
                BS.Availability as BS_Availability,
                BS.Duration as BS_Duration,
                BA.Address as BA_Address,
                BA.Business_name as BA_name,
                BA.Cancellation_Policy as BA_Cancellation_Policy,
                BA.contact_information as BA_Contact_information,
                BA.[Description] as BA_Description,
                BA.Organization_Office as BS_Organization_Office,
                BA.Time_Slots as BA_Time_Slots,
                BA.Schdule as BA_Schedule,
                BA.Max_appointment as BA_Max_appointment
                from 
                ClientAppointment as CA
                JOIN BusinessService as BS on CA.ServiceID = BS.ServiceID
                JOIN BusinessAppointment as BA on BS.BusinessAppointmentID = BA.BusinessAppointment_ID
                WHERE CA.Userid  = {0};",user_id)
                .ToList();
                
                if (services == null){
                    return new Collection<ClientSubscribed>();
                }else{
                    return new Collection<ClientSubscribed>(services);
                }    
            }
            catch (Exception e)
            {
                return new Collection<ClientSubscribed>();
            }
            
            
        }
        public async Task<List<BusinessAppointmentService>> SearchItemsAsync(string searchText)
        {
            var services = await _context.BusinessAppointmentService
            .FromSqlRaw(
                @"SELECT 
                BS.ServiceId as ServiceId,
                BS.Name as BS_Name,
                BS.Description as  BS_Description,
                BS.Price as BS_Price,
                BS.Availability as BS_Availability,
                BS.Duration as  BS_Duration ,

                BA.Business_name as BA_Business_name,
                BA.Address as BA_Address,
                BA.contact_information as BA_contact_information,
                BA.Organization_Office as  BA_Organization_Office,
                BA.Description as BA_Description,
                BA.Schdule as  BA_Schedule ,
                BA.Time_Slots as BA_Time_Slots ,
                BA.Max_appointment as BA_Max_appointment,
                BA.Cancellation_Policy as BA_Cancellation_Policy
            from
            BusinessService as BS
            JOIN BusinessAppointment as BA on BS.BusinessAppointmentID = BA.BusinessAppointment_ID
            WHERE Name LIKE '%' + {0} + '%';",
                searchText)
            .ToListAsync();

            return services;
        }

        // home Client
         public async Task<List<BusinessAppointmentService>> GetItemsAsync(int page, int pageSize)
        {
           var items = await _context.BusinessAppointmentService.FromSqlRaw(
                        @"SELECT 
            BS.ServiceId as ServiceId,
            BS.Name as BS_Name,
            BS.Description as  BS_Description,
            BS.Price as BS_Price,
            BS.Availability as BS_Availability,
            BS.Duration as  BS_Duration ,

            BA.Business_name as BA_Business_name,
            BA.Address as BA_Address,
            BA.contact_information as BA_contact_information,
            BA.Organization_Office as  BA_Organization_Office,
            BA.Description as BA_Description,
            BA.Schdule as  BA_Schedule ,
            BA.Time_Slots as BA_Time_Slots ,
            BA.Max_appointment as BA_Max_appointment,
            BA.Cancellation_Policy as BA_Cancellation_Policy
            from
            BusinessService as BS
            JOIN BusinessAppointment as BA on BS.BusinessAppointmentID = BA.BusinessAppointment_ID
            ORDER BY ServiceId
            OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY",
                        page * pageSize, pageSize)
                        .ToListAsync();
            return items;
        }
        public  List<ClientSubscribed> SearchBydateAsync(string date,int userId)
        {
           var query = @"SELECT 
                CA.[Description] as CA_Description,
                CA.[status] as CA_Status,
                CA.Time_Date as CA_Time_date,
                BS.ServiceId as BS_ServiceID,
                BS.Name as BS_Name,
                BS.[Description] as BS_Description,
                BS.Price as BS_Price,
                BS.Availability as BS_Availability,
                BS.Duration as BS_Duration,
                BA.Address as BA_Address,
                BA.Business_name as BA_name,
                BA.Cancellation_Policy as BA_Cancellation_Policy,
                BA.contact_information as BA_Contact_information,
                BA.[Description] as BA_Description,
                BA.Organization_Office as BS_Organization_Office,
                BA.Time_Slots as BA_Time_Slots,
                BA.Schdule as BA_Schedule,
                BA.Max_appointment as BA_Max_appointment
                from 
                ClientAppointment as CA
                JOIN BusinessService as BS on CA.ServiceID = BS.ServiceID
                JOIN BusinessAppointment as BA on BS.BusinessAppointmentID = BA.BusinessAppointment_ID
                WHERE CA.Time_Date = {0} AND CA.Userid = {1};";
             List<ClientSubscribed>? services = _context.ClientSubscribed
                .FromSqlRaw(query, date,userId)
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
            ca.ServiceID AS ServiceID,
            ca.Userid  AS ClientID, 
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
            WHERE ba_acc.id = {0} and ca.status != 'Denied' and ca.status != 'Done' ";

                var appointments = _context.BusinessSubcriber
                    .FromSqlRaw(query, user_id)
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
                ca.ServiceID AS ServiceID,
                ca.Userid  AS ClientID, 
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
            WHERE ba_acc.id = {0} and ca.Time_Date = {1} and ca.status != 'Denied' and ca.status != 'Done' ;";

            var appointments = _context.BusinessSubcriber
                    .FromSqlRaw(query, userId,searchText)
                    .ToList();
            return appointments;

        }
        

        public void Accept(int ClientID, int ServiceID)
        {
            string query = @"
                        UPDATE ClientAppointment
                        SET Status = 'Accept'
                        WHERE ClientAppointment.Userid = {0} AND ClientAppointment.ServiceID = {1};";
            _context.Database.ExecuteSqlRaw(query, ClientID,ServiceID);
        
        }

        public void Denied(int ClientID, int ServiceID)
        {

            string query = @"
                        UPDATE ClientAppointment
                        SET Status = 'Denied'
                        WHERE ClientAppointment.Userid = {0} AND ClientAppointment.ServiceID = {1};";
            _context.Database.ExecuteSqlRaw(query, ClientID,ServiceID);
        
        }
        public void Done(int ClientID, int ServiceID)
        {
            string query = @"
                        UPDATE ClientAppointment
                        SET Status = 'Done'
                        WHERE ClientAppointment.Userid = {0} AND ClientAppointment.ServiceID = {1};";
            _context.Database.ExecuteSqlRaw(query, ClientID,ServiceID);
        
        }
    }
}