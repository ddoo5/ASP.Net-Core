using System;
namespace ContractControlCentre.Security.Models
{
    internal sealed class AuthResponse
    {
        public string Password { get; set; }
        public RefreshToken LatestRefreshToken { get; set; }
    }
}

