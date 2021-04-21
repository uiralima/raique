using Raique.Common.HTTP.Hooks;
using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Protocols;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Raique.Common.HTTP.AspNet.Controller
{
    [AppActionFilter]
    public abstract class Base : System.Web.Http.ApiController, IActionController, IController
    {
        private readonly Common.Controller.IController _logicalController;

        protected Base(Common.Controller.IController logicalController)
        {
            _logicalController = logicalController;
        }
        protected Common.Controller.IController LogicalController => _logicalController;
        public abstract bool DeviceRequired { get; }
        public abstract bool AppRequired { get; }
        public abstract bool UserRequired { get; }
        public string AppKey 
        {
            get => _logicalController.AppKey;
            set => _logicalController.AppKey = value;
        }
        public string Device 
        {
            get => _logicalController.Device;
            set => _logicalController.Device = value;
        }
        public User CurrentUser 
        {
            get => _logicalController.CurrentUser;
            set => _logicalController.CurrentUser = value;
        }
        [NonAction]
        public async Task DoActionExecuted(HttpActionExecutedContext context)
        {
            AfterAction _afterAction = new AfterAction(HttpAfterActionMessage.CreateFromContext(context));
            await _afterAction.Execute();
        }

        [NonAction]
        public async Task DoActionExecuting(HttpActionContext actionContext)
        {
            BeforeAction beforeAction = new BeforeAction(
                this,
                HttpBeforeActionMessage.CreateFromContext(actionContext),
                DependencyInjection.Repository.CreateInstance<ITokenRepository>(), 
                DependencyInjection.Repository.CreateInstance<IUserRepository>());
            await beforeAction.Execute();
        }
    }
}
