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
            MJPEGStreamManager.Test();
            //TODO: subscribe to an event in backend that alerts about a detected face.
            app.MapSignalR();
        }
    }
}
