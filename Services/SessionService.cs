using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class SessionService : ISessionService
{
    private const string SessionFilePath = "/home/karl/AppoinmentScheduler/log/session.json";

    private readonly AppDbContext _context;
    private User? _user {get ; set;} 
    public SessionService(AppDbContext context){
         _context = context;
    }



    public async Task SaveSessionAsync(OAuthToken token)
    {
        var directory = Path.GetDirectoryName(SessionFilePath);
        // Ensure the directory exists
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        var jsonData = JsonSerializer.Serialize(token);
        await File.WriteAllTextAsync(SessionFilePath, jsonData);
    }

    public async Task<OAuthToken?> LoadSessionAsync()
    {
        if (!File.Exists(SessionFilePath))
            return null;

        var jsonData = await File.ReadAllTextAsync(SessionFilePath);
        return JsonSerializer.Deserialize<OAuthToken>(jsonData);
    }

    public void SessionLogout()
    {
        if (File.Exists(SessionFilePath))
            File.Delete(SessionFilePath);
    }
    public async Task<bool> SessionLogin()
    {
        if (!File.Exists(SessionFilePath)) return false;
        OAuthToken oAuthToken = await LoadSessionAsync();
        
        _user = await _context.Users.FromSqlRaw("SELECT * FROM Users WHERE id = {0}", oAuthToken.id).FirstOrDefaultAsync();
        Console.WriteLine(_user.id);

        if (_user == null) return false;
        
        // Check if the token has expired
        DateTime issuedAt = oAuthToken.IssuedAt;
        DateTime expiryTime = issuedAt.AddSeconds(oAuthToken.ExpiresIn);

        // Token is expired
        if (DateTime.UtcNow >= expiryTime) return false;
        
        if(!BCrypt.Net.BCrypt.Verify(oAuthToken.AccessToken, _user.token)){
            _user =  null;
            return false;
        }
        return true;
    }

    public User GetUser(){
        return _user;
    }

    
}

