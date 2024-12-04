using System;
using Models;

namespace Models
{
    public class ClientAppointment
    {
        public int ClientAppointment_ID { get; set; } 
        public int Userid { get; set; } 
        public int ServiceID { get; set; } 
        public required string Time_Date { get; set; } 
        public required string Status { get; set; } 
        public required string Description{ get; set; } 

        public User User { get; set; }

    }
}