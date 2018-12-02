using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AdminWeb
{
    public class FaceDetectedAlerter
    {
        private readonly static Lazy<FaceDetectedAlerter> instance = new Lazy<FaceDetectedAlerter>(() => new FaceDetectedAlerter(GlobalHost.ConnectionManager.GetHubContext<FaceDetectedAlertHub>().Clients));

        private IHubConnectionContext<dynamic> Clients { get; set; }

        //something to hold here?

        private FaceDetectedAlerter(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        private void AlertDetectedFace(string guid)
        {
            Clients.All.alertFaceDetected(guid);
        }

        public static FaceDetectedAlerter Instance { get { return instance.Value; } } 
    }
}