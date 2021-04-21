using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Raique.Common.HTTP.AspNetCore.Controller
{
    public class AppActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (context.Controller is IActionController)
                {
                    var task = (context.Controller as IActionController).DoActionExecuting(context);
                    task.Wait();
                }
            }
            catch(Exception ex)
            { }
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Controller is IActionController)
            {
                (context.Controller as IActionController).DoActionExecuted(context);
            }
            base.OnActionExecuted(context);
        }
    }
}
