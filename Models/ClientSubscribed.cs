namespace Models
{
    public class ClientSubscribed
    {
        // Fields from ClientAppointment
        public string CA_Description { get; set; }
        public string CA_Status { get; set; }
        public string CA_Time_Date { get; set; }


        // Fields from BusinessService
        public int BS_ServiceID { get; set; } 
        public string BS_Name { get; set; }
        public string BS_Description { get; set; }
        public int BS_Price { get; set; }
        public int BS_Availability { get; set; }
        public int BS_Duration { get; set; }

        // Fields from BusinessAppointment
        public string BA_Address { get; set; }
        public string BA_Name { get; set; }
        public string BA_Cancellation_Policy { get; set; }
        public string BA_Contact_Information { get; set; }
        public string BA_Description { get; set; }
        public string BS_Organization_Office { get; set; }
        public string BA_Time_Slots { get; set; }
        public string BA_Schedule { get; set; }
        public int BA_Max_Appointment { get; set; }
    }
}
