namespace PharmaCareBlazor.Services.Interfaces;

public interface IAuthService
{
    bool IsAuthenticated { get; }
    string CurrentUser { get; }
    string UserRole { get; }
    event Action? OnAuthStateChanged;
    bool Login(string username, string password);
    void Logout();
}
