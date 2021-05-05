using Microsoft.IdentityModel.Tokens;
using Raique.JWT.Protocols;
using Raique.Library;
using Raique.Microservices.Authenticate.Domain;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Raique.JWT
{
    public class TokenInfoExtractorImpl : Microservices.Authenticate.Protocols.ITokenInfoExtractor
    {
        private readonly IJWTConfig _jWTConfig;

        public TokenInfoExtractorImpl(Protocols.IJWTConfig jWTConfig)
        {
            _jWTConfig = jWTConfig;
        }
        public (User, string, string) Extract(string token)
        {
            User user = User.Invalid<User>();
            string device = String.Empty;
            string app = String.Empty;
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                {
                    byte[] key = Convert.FromBase64String(_jWTConfig.Secret);
                    TokenValidationParameters parameters = new TokenValidationParameters()
                    {
                        RequireExpirationTime = _jWTConfig.Expires > 0,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                    SecurityToken securityToken;
                    ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                          parameters, out securityToken);
                    foreach(var curentClaim in principal.Claims)
                    {
                        if (curentClaim.Type.CompareTo("MyUser") == 0)
                        {
                            user = curentClaim.Value.ParseJson<User>();
                        }
                        else if (curentClaim.Type.CompareTo("Device") == 0)
                        {
                            device = curentClaim.Value;
                        }
                        else if (curentClaim.Type.CompareTo("App") == 0)
                        {
                            app = curentClaim.Value;
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                
            }
            return (user, device, app);
        }
    }
}
