using Microsoft.AspNetCore.Mvc.Filters;
using Raique.Common.HTTP.Hooks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Raique.Common.HTTP.AspNetCore.Controller
{
    public class HttpAfterActionMessage : IAfterActionMessage
    {
        public static HttpAfterActionMessage CreateFromContext(ActionExecutedContext context) =>
            new HttpAfterActionMessage(context);

        private ActionExecutedContext _context;
        private HttpAfterActionMessage(ActionExecutedContext context)
        {
            _context = context;
        }

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers => _context.HttpContext.Request.Headers.Select(f => new KeyValuePair<string, IEnumerable<string>>(f.Key, f.Value));

        public Exception Exception => _context.Exception;

        public void ClearException()
        {
            _context.Exception = null;
        }

        public void CreateResponse(HttpStatusCode statusCode, string message)
        {
            var messageBytes = Encoding.ASCII.GetBytes(message);
            _context.HttpContext.Response.StatusCode = (int)statusCode;
            _context.HttpContext.Response.Body.Write(messageBytes, 0, messageBytes.Length);
        }
    }
}
