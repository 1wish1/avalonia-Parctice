using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class SessionService : ISessionService
{
    private const string SessionFilePath = "/home/karl/AppoinmentScheduler/log/session.json";

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

    public void Logout()
    {
        if (File.Exists(SessionFilePath))
            File.Delete(SessionFilePath);
    }
    public bool Login()
    {
        if (File.Exists(SessionFilePath)) return true;
        return false;
    }
    


    
}
