using Raique.Microservices.Authenticate.Domain;
using System.Collections.Generic;

namespace Raique.JWT.Protocols
{
    public interface ILoadRoles
    {
        List<string> ToUser(User user);
    }
}
