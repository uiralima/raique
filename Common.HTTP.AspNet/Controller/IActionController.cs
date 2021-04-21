using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Raique.Common.HTTP.AspNet.Controller
{
    public interface IActionController
    {
        Task DoActionExecuting(HttpActionContext actionContext);
        Task DoActionExecuted(HttpActionExecutedContext context);
    }
}
