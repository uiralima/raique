using Raique.Microservices.Authenticate.Domain;

namespace Raique.Common.Controller
{
    public abstract class BaseController : IController
    {
        public virtual bool DeviceRequired => true;
        public virtual bool AppRequired => true;
        public virtual bool UserRequired => true;
        public string AppKey { get; set; }
        public string Device { get; set; }
        public string Token { get; set; }
        public User CurrentUser { get; set; }
        public virtual void Log(string messageToLog)
        {
            Common.Log.FileLog.InternalLog.Log(messageToLog);
        }
    }
}
