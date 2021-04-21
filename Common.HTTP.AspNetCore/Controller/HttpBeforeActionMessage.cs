using Microsoft.AspNetCore.Mvc.Filters;
using Raique.Common.HTTP.Hooks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Raique.Common.HTTP.AspNetCore.Controller
{
    public class HttpBeforeActionMessage : IBeforeActionMessage
    {
        public static HttpBeforeActionMessage CreateFromContext(ActionExecutingContext context) =>
            new HttpBeforeActionMessage(context);

        public void CreateResponse(HttpStatusCode statusCode, string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _context.HttpContext.Response.StatusCode = (int)statusCode;
            var task = _context.HttpContext.Response.Body.WriteAsync(messageBytes, 0, messageBytes.Length);
            task.Wait();
        }

        private ActionExecutingContext _context;

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers => _context.HttpContext.Request.Headers.Select(f => new KeyValuePair<string, IEnumerable<string>>(f.Key, f.Value));

        private HttpBeforeActionMessage(ActionExecutingContext context)
        {
            _context = context;
        }

    }
}
