using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AdminPage
{
    public class VideoStreamingHub : Hub
    {
        public void Send(string data)
        {
            Clients.All.sendStream(data);
        }
    }
}