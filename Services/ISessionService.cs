using System.Threading.Tasks;

public interface ISessionService
{
    Task SaveSessionAsync(OAuthToken token);
    Task<OAuthToken?> LoadSessionAsync();
    void Logout();
    
}