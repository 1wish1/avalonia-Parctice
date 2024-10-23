using System;

public class OAuthToken
{
    public required string AccessToken { get; set; }        // by The access token issued the server
    public int ExpiresIn { get; set; }             // Expiration time in seconds (e.g., 3600 for 1 hour)
    public DateTime IssuedAt { get; set; }         // When the token was issued
    public int role { get; set; } 
    
}
