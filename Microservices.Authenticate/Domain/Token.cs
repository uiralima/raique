using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.Microservices.Authenticate.Domain
{
    public class Token : Base
    {
        public int TokenId { get; set; }
        public string TokenStr { get; set; }
        public int UserId { get; set; }
        public string Device { get; set; }

    }
}
