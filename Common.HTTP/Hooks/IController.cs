namespace Raique.Common.HTTP.Hooks
{
    public interface IController
    {
        bool DeviceRequired { get; }
        bool AppRequired { get; }
        bool UserRequired { get; }
    }
}
