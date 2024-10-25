using System.Threading.Tasks;
using Models;

public interface ISessionService
{
    Task SaveSessionAsync(OAuthToken token);
    Task<OAuthToken?> LoadSessionAsync();
    void SessionLogout();
    Task<bool> SessionLogin();

    User GetUser();
    
}