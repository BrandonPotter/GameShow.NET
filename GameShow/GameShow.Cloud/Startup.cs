using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace GameShow.Cloud
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}