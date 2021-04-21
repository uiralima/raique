using Raique.Microservices.Authenticate.Domain;

namespace Raique.Common.HTTP.Hooks
{
    public interface IController
    {
        bool DeviceRequired { get; }
        bool AppRequired { get; }
        bool UserRequired { get; }
        string AppKey { get; set; }
        string Device { get; set; }
        User CurrentUser { get; set; }
    }
}
