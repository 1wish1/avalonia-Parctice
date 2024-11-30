using System;
using Models;

namespace Models
{
    public class ClientAppointment
    {
        public int ClientAppointment_ID { get; set; } 
        public int Client_Account { get; set; } 
        public int ServiceID { get; set; } 
        public required DateTime Time_Date;
        public required string Status { get; set; } 
        public required string Description{ get; set; } 

        public User User { get; set; }

    }
}