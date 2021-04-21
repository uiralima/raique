using Raique.Common.HTTP.Hooks;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Raique.Common.HTTP.AspNet
{
    public class HttpBeforeActionMessage : IBeforeActionMessage
    {
        public static HttpBeforeActionMessage CreateFromContext(HttpActionContext context) =>
            new HttpBeforeActionMessage(context);

        public void CreateResponse(HttpStatusCode statusCode, string message)
        {
            _context.Response = new HttpResponseMessage(statusCode);
            _context.Response.Content = new StringContent(message);
        }

        private HttpActionContext _context;

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers => _context.Request.Headers;

        private HttpBeforeActionMessage(HttpActionContext context)
        {
            _context = context;
        }

    }
}
