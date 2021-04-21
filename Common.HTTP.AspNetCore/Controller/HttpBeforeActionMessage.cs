using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Raique.Common.HTTP.Hooks;

namespace Raique.Common.HTTP.AspNetCore.Controller
{
    public class HttpBeforeActionMessage : BaseActionMessage, IBeforeActionMessage
    {
        public static HttpBeforeActionMessage CreateFromContext(ActionExecutingContext context) =>
            new HttpBeforeActionMessage(context);

        private ActionExecutingContext _context;
        private HttpBeforeActionMessage(ActionExecutingContext context)
        {
            _context = context;
        }

        protected override void SetActionResult(IActionResult actionResult)
        {
            _context.Result = actionResult;
        }

        protected override HttpContext Context => _context.HttpContext;
    }
}