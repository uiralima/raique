using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Raique.Common.HTTP.AspNet.Controller
{
    
    public class AppActionFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        
        public override async Task OnActionExecutingAsync(HttpActionContext context, CancellationToken cancellationToken)
        {
            try
            {
                if (context.ControllerContext.Controller is IActionController)
                {

                   await (context.ControllerContext.Controller as IActionController).DoActionExecuting(context);
                }
            }
            catch (Exception ex)
            {
                context.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                context.Response.Content = new StringContent(ex.Message);
            }
            await base.OnActionExecutingAsync(context, cancellationToken);
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
        {
            try
            {
                if (context.ActionContext.ControllerContext.Controller is IActionController)
                {
                    await(context.ActionContext.ControllerContext.Controller as IActionController).DoActionExecuted(context);
                }
            }
            catch (Exception ex)
            {
                context.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                context.Response.Content = new StringContent(ex.Message);
            }
            await base.OnActionExecutedAsync(context, cancellationToken);
        }
    }
}
