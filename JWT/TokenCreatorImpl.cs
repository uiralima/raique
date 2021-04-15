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
        private readonly ILoadRoles _loadRoles;

        public TokenCreatorImpl(Protocols.IJWTConfig jWTConfig, Protocols.ILoadRoles loadRoles)
        {
            _jWTConfig = jWTConfig;
            _loadRoles = loadRoles;
        }
        public string Create(User user)
        {
            byte[] key = Convert.FromBase64String(_jWTConfig.Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            List<Claim> clains = new List<Claim>();
            _loadRoles.ToUser(user).ForEach(f => clains.Add(new Claim("roles", f)));
            ClaimsIdentity identity;
            identity = new ClaimsIdentity(new GenericIdentity(user.Key, "Login"), clains);
            //Dictionary<string, object> otherClaims = new Dictionary<string, object>();
            //otherClaims.Add("MyUser", user);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                //Claims = otherClaims,
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
