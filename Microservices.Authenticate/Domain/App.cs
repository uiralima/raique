using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.Microservices.Authenticate.Domain
{
    public class App : Base
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }

    }
}
