using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Raique.Common.HTTP.Hooks;
using System;

namespace Raique.Common.HTTP.AspNetCore.Controller
{
    public class HttpAfterActionMessage : BaseActionMessage, IAfterActionMessage
    {
        public static HttpAfterActionMessage CreateFromContext(ActionExecutedContext context) =>
            new HttpAfterActionMessage(context);

        private ActionExecutedContext _context;
        private HttpAfterActionMessage(ActionExecutedContext context)
        {
            _context = context;
        }

        public Exception Exception => _context.Exception;
        public void ClearException()
        {
            _context.Exception = null;
        }

        protected override HttpContext Context => _context.HttpContext;

        protected override void SetActionResult(IActionResult actionResult)
        {
            _context.Result = actionResult;
        }
    }
}
