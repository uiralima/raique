using Raique.Common.HTTP.Hooks;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Raique.Common.HTTP.AspNet
{
    public class HttpAfterActionMessage : IAfterActionMessage
    {
        public static HttpAfterActionMessage CreateFromContext(HttpActionExecutedContext context) =>
            new HttpAfterActionMessage(context);

        private HttpActionExecutedContext _context;
        private HttpAfterActionMessage(HttpActionExecutedContext context)
        {
            _context = context;
        }

        public Exception Exception => _context.Exception;

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers => _context.Request.Headers;

        public void ClearException()
        {
            _context.Exception = null;
        }

        public void CreateResponse(HttpStatusCode statusCode, string message)
        {
            _context.Response = new HttpResponseMessage(statusCode);
            _context.Response.Content = new StringContent(message);
        }
    }
}
