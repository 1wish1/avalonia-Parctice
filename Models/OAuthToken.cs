using System;

public class OAuthToken
{
    public string AccessToken { get; set; }        // The access token issued by the server
    public string RefreshToken { get; set; }       // The refresh token used to get a new access token
    public int ExpiresIn { get; set; }             // Expiration time in seconds (e.g., 3600 for 1 hour)
    public string TokenType { get; set; }          // Typically "Bearer"
    public DateTime IssuedAt { get; set; }         // When the token was issued
    public string UserId { get; set; }             // Optional: User ID or any other relevant information
}
