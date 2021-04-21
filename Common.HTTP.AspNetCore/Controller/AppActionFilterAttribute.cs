using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Raique.Common.HTTP.AspNetCore.Controller
{
    public class AppActionFilterAttribute : ActionFilterAttribute
    {
        public override async System.Threading.Tasks.Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                if (context.Controller is IActionController)
                {
                    await (context.Controller as IActionController).DoActionExecuting(context);
                }
            }
            catch (Exception ex)
            {
                context.Result = new ObjectResult(ex.Message)
                {
                    StatusCode = 500
                };
            }
            if (null == context.Result)
            {
                await base.OnActionExecutionAsync(context, next);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Controller is IActionController)
            {
                var task = (context.Controller as IActionController).DoActionExecuted(context);
                task.Wait();
            }
            base.OnActionExecuted(context);
        }
    }
}
