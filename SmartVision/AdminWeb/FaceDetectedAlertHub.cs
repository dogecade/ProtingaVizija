using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;

namespace AdminWeb
{
    [HubName("faceDetectedAlert")]
    public class FaceDetectedAlertHub : Hub
    {
        private readonly FaceDetectedAlerter alerter;

        public FaceDetectedAlertHub() : this(FaceDetectedAlerter.Instance) { }

        public FaceDetectedAlertHub(FaceDetectedAlerter alerter) => this.alerter = alerter;
    }
}