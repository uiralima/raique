using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Raique.Common.HTTP.AspNetCore.Controller
{
    public static class ActionResultFactory
    {
        public static IActionResult CreateFromStatusCode(HttpStatusCode statusCode)
        {
            switch(statusCode)
            {
                case HttpStatusCode.BadRequest:
                    return new BadRequestResult();
                case HttpStatusCode.OK:
                    return new OkResult();
                case HttpStatusCode.InternalServerError:
                    return new StatusCodeResult(500);
                case HttpStatusCode.Forbidden:
                    return new ForbidResult();
            }
            return new StatusCodeResult((int)statusCode);
        }
    }
}
