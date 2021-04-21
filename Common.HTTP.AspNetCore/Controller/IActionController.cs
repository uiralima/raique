using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Raique.Common.HTTP.AspNetCore.Controller
{
    public interface IActionController
    {
        Task DoActionExecuting(ActionExecutingContext context);
        Task DoActionExecuted(ActionExecutedContext context);
    }
}
