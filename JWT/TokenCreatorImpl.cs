using Microsoft.IdentityModel.Tokens;
using Raique.JWT.Protocols;
using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Protocols;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Raique.JWT
{
    public class TokenCreatorImpl : ITokenCreator
    {
        private readonly IJWTConfig _jWTConfig;

        public TokenCreatorImpl(Protocols.IJWTConfig jWTConfig)
        {
            _jWTConfig = jWTConfig;
        }
        public string Create(User user)
        {
            byte[] key = Convert.FromBase64String(_jWTConfig.Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            ClaimsIdentity identity;
            identity = new ClaimsIdentity(new GenericIdentity(user.Key, "Login"));
            //Dictionary<string, object> otherClaims = new Dictionary<string, object>();
            //otherClaims.Add("MyUser", user);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                //Claims = otherClaims,
                NotBefore = Raique.Library.DateUtilities.Now,
                Subject = identity,
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            if (_jWTConfig.Expires > 0)
            {
                descriptor.Expires = DateTime.UtcNow.AddMinutes(_jWTConfig.Expires);
            }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
