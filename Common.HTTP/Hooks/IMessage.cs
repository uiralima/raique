using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Raique.Common.HTTP.Hooks
{
    public interface IMessage
    {
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; }
        void CreateResponse(HttpStatusCode statusCode, string message);
    }
}
