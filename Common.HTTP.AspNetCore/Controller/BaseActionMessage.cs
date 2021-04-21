using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Raique.Common.HTTP.AspNetCore.Controller
{
    public abstract class BaseActionMessage
    {
        public void CreateResponse(HttpStatusCode statusCode, string message)
        {
            SetActionResult(new ObjectResult(message)
            {
                StatusCode = (int)statusCode
            });
        }
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers => 
            Context.Request.Headers.Select(f => new KeyValuePair<string, IEnumerable<string>>(f.Key, f.Value));

        protected abstract HttpContext Context { get; }
        protected abstract void SetActionResult(IActionResult actionResult);
    }
}
