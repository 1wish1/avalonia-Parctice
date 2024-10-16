using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public interface ISessionManager
{
    Task SaveSessionAsync(OAuthToken token);
    Task<OAuthToken?> LoadSessionAsync();
    void Logout();
}


public class SessionManager : ISessionManager
{
    private const string SessionFilePath = "session.json";

    public async Task SaveSessionAsync(OAuthToken token)
    {
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
}
