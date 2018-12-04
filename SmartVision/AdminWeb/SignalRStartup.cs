using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using StreamingBackend;

[assembly: OwinStartup(typeof(AdminWeb.SignalRStartup))]

namespace AdminWeb
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
