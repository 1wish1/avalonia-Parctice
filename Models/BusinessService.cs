using System;
using Models;

namespace Models
{
public class BusinessService
{
    public int ServiceId { get; set; }  
    public required string Name { get; set; } 
    public required string Description { get; set; } 
    public required int Price { get; set; } 
    public required int Duration { get; set; } 
    public required int Availability { get; set; }

    public int businessAppointmentID { get; set; }  
    public BusinessAppointment businessAppointment { get; set; }

}   
}