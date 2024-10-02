namespace Models
{
    public class User
    {
        public int Id { get; set; } // Primary Key
        public string user_name { get; set; }
        public string email { get; set; }
        public string password { get; set; } // Consider hashing passwords for security
    }    
}

