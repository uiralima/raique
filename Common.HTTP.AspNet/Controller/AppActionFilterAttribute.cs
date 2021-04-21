using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Raique.Common.HTTP.AspNet.Controller
{
    public class AppActionFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute, System.Web.Http.Filters.IActionFilter
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            try
            {
                if (context.ControllerContext.Controller is IActionController)
                {
                    (context.ControllerContext.Controller as IActionController).DoActionExecuting(context);
                }
            }
            catch(Exception ex)
            {

            }
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            if (context.ActionContext.ControllerContext.Controller is IActionController)
            {
                (context.ActionContext.ControllerContext.Controller as IActionController).DoActionExecuted(context);
            }
            base.OnActionExecuted(context);
        }
    }
}
