using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AppoinmentScheduler.ObjMessages;
using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.Messaging;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class SessionService : ISessionService
{
    private const string SessionFilePath = "/home/karl/AppoinmentScheduler/log/session.json";

    private readonly AppDbContext _context;

  
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
    public async Task<bool> SessionLogin(IUserService userService)
    {
        if (!File.Exists(SessionFilePath)) return false;
        OAuthToken oAuthToken = await LoadSessionAsync();
        
        User _user = await _context.Users.FromSqlRaw("SELECT * FROM Users WHERE id = {0}", oAuthToken.id).FirstOrDefaultAsync();

        // Check if the token has expired
        DateTime issuedAt = oAuthToken.IssuedAt;
        DateTime expiryTime = issuedAt.AddSeconds(oAuthToken.ExpiresIn);

        // Token is expired
        if (DateTime.UtcNow >= expiryTime) return false;
        
        if(!BCrypt.Net.BCrypt.Verify(oAuthToken.AccessToken, _user.token)){
            return false;
        }else{
            userService.SendData(_user);
        }
        
        return true;
    }

    
    
}

