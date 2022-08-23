using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContractControlCentre.DB.Interfaces;
using ContractControlCentre.Security.Authentication.Service;
using ContractControlCentre.Security.Models;
using Microsoft.IdentityModel.Tokens;

namespace ContractControlCentre.Security.Authentication.Service
{
    public class AuthenticationService : IAuthenticationService   //todo write docs
    {
        /// <summary>
        /// Each password was created by
        /// <see cref="https://github.com/ddoo5/PC">Password Creator</see>
        /// </summary>
        private IReadOnlyDictionary<string, AuthResponse> _sudouser = new Dictionary<string, AuthResponse>() {
            { "admin", new AuthResponse() { Password = "2XETnz5N9h^/0x6BUg$4!-Q8" } },
            { "person", new AuthResponse() { Password = "115P30Wf4a3#^pE/iA" } },
            { "employee", new AuthResponse() { Password = "J%RMhh04z8b1t4$5" } }
        };


        /// <summary>
        /// Created by 
        /// <see cref="https://github.com/ddoo5/PC">PC</see>
        /// </summary>
        public const string _secretWord = "Z7u0R@H69TZ9zZV/Yb-d";



        public TokenResponse Authentication(string username, string password)
        {

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            TokenResponse token = new();
            int i = 0;

            foreach (KeyValuePair<string, AuthResponse> pair in _sudouser)
            {
                i++;

                if (string.CompareOrdinal(pair.Key, username) == 0 && string.CompareOrdinal(pair.Value.Password, password) == 0)
                {
                    token.Token = GenerateJwtToken(i, 15);
                    RefreshToken refreshToken = GenerateRefreshToken(i);
                    pair.Value.LatestRefreshToken = refreshToken;
                    token.RefreshToken = refreshToken.Token;
                    return token;
                }
            }

            return null;
        }


        public string RefreshToken(string token)
        {

            int i = 0;

            foreach (KeyValuePair<string, AuthResponse> pair in _sudouser)
            {
                i++;

                if (string.CompareOrdinal(pair.Value.LatestRefreshToken.Token, token) == 0 && pair.Value.LatestRefreshToken.IsExpired is false)
                {
                    pair.Value.LatestRefreshToken = GenerateRefreshToken(i);
                    return pair.Value.LatestRefreshToken.Token;
                }
            }

            return string.Empty;
        }


        private string GenerateJwtToken(int id, int minutes)
        {

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_secretWord);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(minutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public RefreshToken GenerateRefreshToken(int id)
        {
            RefreshToken refreshToken = new RefreshToken();
            refreshToken.Expires = DateTime.Now.AddMinutes(60);
            refreshToken.Token = GenerateJwtToken(id, 60);
            return refreshToken;
        }

    }
}

