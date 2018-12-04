using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceAnalysis;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using StreamingBackend;

namespace AdminWeb
{
    public class FaceDetectedAlerter
    {
        private readonly static Lazy<FaceDetectedAlerter> instance = new Lazy<FaceDetectedAlerter>(() => new FaceDetectedAlerter(GlobalHost.ConnectionManager.GetHubContext<FaceDetectedAlertHub>().Clients));

        private IHubConnectionContext<dynamic> Clients { get; set; }

        private FaceDetectedAlerter(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            MJPEGStreamManager.Processor.FacesDetected += HandleFacesDetectedEvent;
        }

        private void AlertDetectedFaces(IEnumerable<string> guids)
        {
            Clients.All.alertFaceDetected(guids);
        }

        private void HandleFacesDetectedEvent(object sender, FacesDetectedEventArgs e)
        {
            AlertDetectedFaces(e.Sources.Select(source => source.Id.ToString()));
        }

        public static FaceDetectedAlerter Instance { get { return instance.Value; } } 
    }
}