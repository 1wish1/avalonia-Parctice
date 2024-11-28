using System;

public class OAuthToken
{
    public required string AccessToken { get; set; }       
    public int ExpiresIn { get; set; }             
    public DateTime IssuedAt { get; set; }         
    public int role { get; set; } 
    public int id { get; set; }
    
}
