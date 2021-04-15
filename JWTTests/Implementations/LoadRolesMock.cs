using Raique.JWT.Protocols;
using Raique.Microservices.Authenticate.Domain;
using System.Collections.Generic;

namespace JWTTests.Implementations
{
    public class LoadRolesMock : ILoadRoles
    {
        public List<string> ToUser(User user)
        {
            List<string> result = new List<string>();
            result.Add("User");
            result.Add("NetworkManager");
            return result;
        }
    }
}
