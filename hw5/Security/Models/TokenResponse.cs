using System;
namespace ContractControlCentre.Security.Models
{
    public sealed class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}

