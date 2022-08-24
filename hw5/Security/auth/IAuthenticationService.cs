using ContractControlCentre.Security.Models;

namespace ContractControlCentre.Security.Authentication.Service
{
    public interface IAuthenticationService
    {
        TokenResponse Authentication(string username, string password);
        string RefreshToken(string token);
    }
}

