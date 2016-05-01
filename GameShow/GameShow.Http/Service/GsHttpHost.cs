using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funq;
using ServiceStack;

namespace GameShow.Http.Service
{
    internal class GsHttpHost : AppSelfHostBase {
        public GsHttpHost() 
          : base("GameShow HTTP Service", typeof(GsHttpController).Assembly) { }

    public override void Configure(Funq.Container container) { }
    }
}
