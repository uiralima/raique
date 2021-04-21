namespace Raique.Commom.Controller
{
    public interface IController
    {
        bool DeviceRequired { get; }
        bool AppRequired { get; }
        bool UserRequired { get; }
    }
}
