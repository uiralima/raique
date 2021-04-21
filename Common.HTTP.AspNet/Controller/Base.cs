using Raique.Common.HTTP.Hooks;
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
        protected Base()
        {
            
        }

        public abstract bool DeviceRequired { get; }
        public abstract bool AppRequired { get; }
        public abstract bool UserRequired { get; }

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
