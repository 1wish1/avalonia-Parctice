namespace Models
{
    public class BusinessSubcriber
    {

        public required int ServiceID { get; set; } 
         public required int ClientID { get; set; }
        public required string TimeDate { get; set; } 
        public required string Description { get; set; } 
        public required string Status  { get; set; } 
        public required string ClientEmail  { get; set; } 
        public required string ClientUserName { get; set; } 
        public required string BusinessServiceName  { get; set; } 
        public required string BusinessServiceDescription  { get; set; } 
        public required int Price { get; set; } 
        public required int Duration { get; set; } 
        public required int Availability { get; set; } 
        

    }
}
