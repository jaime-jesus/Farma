using PharmaCareBlazor.Services.Interfaces;

namespace PharmaCareBlazor.Services.Implementations;

public class AuthService : IAuthService
{
    public bool IsAuthenticated { get; private set; } = false;
    public string CurrentUser { get; private set; } = string.Empty;
    public string UserRole { get; private set; } = string.Empty;

    public event Action? OnAuthStateChanged;

    public bool Login(string username, string password)
    {
        var cleanUser = username?.Trim().ToLower() ?? string.Empty;
        var cleanPass = password?.Trim() ?? string.Empty;

        // Credentials for demonstration
        if ((cleanUser == "admin" && cleanPass == "admin123") ||
            (cleanUser == "farmaceutico" && cleanPass == "pharma2026") ||
            (cleanUser == "demo" && cleanPass == "demo123"))
        {
            IsAuthenticated = true;
            CurrentUser = cleanUser == "admin" ? "Dr. Carlos Mendoza" : "Farmacéutico Demo";
            UserRole = cleanUser == "admin" ? "Director Técnico / Farmacéutico" : "Asistente de Farmacia";
            NotifyStateChanged();
            return true;
        }

        return false;
    }

    public void Logout()
    {
        IsAuthenticated = false;
        CurrentUser = string.Empty;
        UserRole = string.Empty;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnAuthStateChanged?.Invoke();
}
