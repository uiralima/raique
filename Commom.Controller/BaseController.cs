namespace Raique.Commom.Controller
{
    public abstract class BaseController : IController
    {
        public virtual bool DeviceRequired => true;

        public virtual bool AppRequired => true;

        public virtual bool UserRequired => true;
    }
}
