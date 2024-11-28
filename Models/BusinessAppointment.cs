using System;
using System.Collections.Generic;
using AppoinmentScheduler.Services;
using Models;
namespace Models
{
public class BusinessAppointment
{
    public int BusinessAppointment_ID { get; set; }  
 
    public required string Business_name { get; set; }
    public required string Address { get; set; }
    public required string contact_information { get; set; }
    public required string Organization_Office { get; set; }
    public required string Description { get; set; }
    public required string Schdule { get; set; }
    public required string Time_Slots { get; set; }
    public required int Max_appointment { get; set; }
    public required string Cancellation_Policy { get; set; }
    public int Business_Account { get; set; }

    public User User { get; set; }
    public ICollection<BusinessService> BusinessService { get; set; }


}
}