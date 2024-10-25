namespace Models
{
    public class User
    {
        public int id { get; set; } 
        public required string? user_name { get; set; }
        public required string email { get; set; }
        public required string password { get; set; } 
        public required string token { get; set; } 
        public required int role { get; set; }
    }    

    
}

