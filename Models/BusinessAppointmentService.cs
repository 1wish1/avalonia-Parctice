
namespace Models
{
    public class BusinessAppointmentService
    {
         // Fields from BusinessService (BS)
        public int ServiceId { get; set; }
        public string BS_Name { get; set; }
        public string BS_Description { get; set; }
        public int BS_Price { get; set; }
        public int BS_Availability { get; set; }
        public int BS_Duration { get; set; }

        // Fields from BusinessAppointment (BA)
        public string BA_Business_Name { get; set; }
        public string BA_Address { get; set; }
        public string BA_Contact_Information { get; set; }
        public string BA_Organization_Office { get; set; }
        public string BA_Description { get; set; }
        public string BA_Schedule { get; set; }
        public string BA_Time_Slots { get; set; }
        public int BA_Max_Appointment { get; set; }
        public string BA_Cancellation_Policy { get; set; }
    }
}