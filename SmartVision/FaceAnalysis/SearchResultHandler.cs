
using NotificationService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using BusService;
using LocationService;
using Objects.Camera;
using Objects.ContactInformation;

namespace FaceAnalysis
{
    public class SearchResultHandler
    {
        private const string normalProbabilityEmailSubject = "Decent possibility that your missing person was detected!";
        private const string normalProbabilitySmsBodyBeginning = "Good afternoon. There's a possibility that your missing person ";
        private const string normalProbabilitySmsBodyEnding = " was detected. Please check you email for more detailed information.";
        private const string normalProbabilityEmailBodyBeginning = "Good afternoon. There's a possibility that your missing person ";
        private const string normalProbabilityEmailBodyEnding = " was detected at this location - ";

        private const string highProbabilityEmailSubject = "HIGH possibility that your missing person was detected!";
        private const string highProbabilitySmsBodyBeginning = "Good afternoon. There's a HIGH possibility that your missing person ";
        private const string highProbabilitySmsBodyEnding = " was detected. Please check you email for more detailed information.";
        private const string highProbabilityEmailBodyBeginning = "Good afternoon. There's a HIGH possibility that your missing person ";
        private const string highProbabilityEmailBodyEnding = " was detected at this location - ";

        private const string veryHighProbabilityEmailSubject = "VERY HIGH possibility that your missing person was detected!";
        private const string veryHighProbabilitySmsBodyBeginning = "Good afternoon. There's a VERY HIGH possibility that your missing person ";
        private const string veryHighProbabilitySmsBodyEnding = " was detected. Please check you email for more detailed information.";
        private const string veryHighProbabilityEmailBodyBeginning = "Good afternoon. There's a VERY HIGH possibility that your missing person ";
        private const string veryHighProbabilityEmailBodyEnding = " was detected at this location - ";

        private readonly Task notifactionSenderTask;
        private readonly ConcurrentDictionary<string, Tuple<DateTime, LikelinessConfidence>> notificationsToSend = new ConcurrentDictionary<string, Tuple<DateTime, LikelinessConfidence>>();
        private readonly ConcurrentDictionary<LikelinessConfidence, LikelinessLevelData> likelinessLevelData = new ConcurrentDictionary<LikelinessConfidence, LikelinessLevelData>();
        private CameraProperties CameraProperties { get; set; }

        public SearchResultHandler(CancellationToken token)
        {
            LikelinessLevelData highLevelData = new LikelinessLevelData
            {
                TimeLimit = 60,
                EmailSubject = highProbabilityEmailSubject,
                SmsBodyBeginning = highProbabilitySmsBodyBeginning,
                SmsBodyEnding = highProbabilitySmsBodyEnding,
                EmailBodyBeginning = highProbabilityEmailBodyBeginning,
                EmailBodyEnding = highProbabilityEmailBodyEnding,
            };
            LikelinessLevelData veryHighLevelData = new LikelinessLevelData
            {
                TimeLimit = 60,
                EmailSubject = veryHighProbabilityEmailSubject,
                SmsBodyBeginning = veryHighProbabilitySmsBodyBeginning,
                SmsBodyEnding = veryHighProbabilitySmsBodyEnding,
                EmailBodyBeginning = veryHighProbabilityEmailBodyBeginning,
                EmailBodyEnding = veryHighProbabilityEmailBodyEnding,
            };
            LikelinessLevelData normalLevelData = new LikelinessLevelData
            {
                TimeLimit = 120,
                EmailSubject = normalProbabilityEmailSubject,
                SmsBodyBeginning = normalProbabilitySmsBodyBeginning,
                SmsBodyEnding = normalProbabilitySmsBodyEnding,
                EmailBodyBeginning = normalProbabilityEmailBodyBeginning,
                EmailBodyEnding = normalProbabilityEmailBodyEnding,
            };

            likelinessLevelData.TryAdd(LikelinessConfidence.VeryHighProbability, veryHighLevelData);
            likelinessLevelData.TryAdd(LikelinessConfidence.NormalProbability, normalLevelData);
            likelinessLevelData.TryAdd(LikelinessConfidence.HighProbability, highLevelData);

            notifactionSenderTask = Task.Run(() => SenderTask(token));
        }

        /// <summary>
        /// Task to process stored notification requests.
        /// </summary>
        /// <param name="token">Cancellation token to stop</param>
        public void SenderTask(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;
                foreach (var entry in notificationsToSend)
                {
                    if (entry.Value.Item1 <= DateTime.Now)
                    {
                        if (entry.Value.Item2 < LikelinessConfidence.VeryHighProbability)
                            SendNotifications(confidence: entry.Value.Item2, faceToken: entry.Key);
                        notificationsToSend.TryRemove(entry.Key, out _);
                    }
                }
            }
        }

        /// <summary>
        /// Handles an incoming search result.
        /// </summary>
        /// <param name="likeliness">Search result to handle</param>
        public void HandleSearchResult(CameraProperties cameraProperties, LikelinessResult likeliness)
        {
            CameraProperties = cameraProperties;

            Debug.WriteLine(likeliness.Confidence);
            switch (likeliness.Confidence)
            {
                case LikelinessConfidence.LowProbability:
                    return;
                case LikelinessConfidence.VeryHighProbability:
                    Tuple<DateTime, LikelinessConfidence> currentValue;
                    if (!notificationsToSend.TryGetValue(likeliness.FaceToken, out currentValue) || currentValue.Item2 < likeliness.Confidence)
                        SendNotifications(likeliness.Confidence, likeliness.FaceToken);
                    break;
            }
            DateTime sendTime = DateTime.Now.AddSeconds(likelinessLevelData[likeliness.Confidence].TimeLimit);
            notificationsToSend.AddOrUpdate(
                likeliness.FaceToken,
                Tuple.Create(sendTime, likeliness.Confidence),
                (key, oldValue) => Tuple.Create(
                    likeliness.Confidence == LikelinessConfidence.VeryHighProbability ? sendTime : new[] { sendTime, oldValue.Item1 }.Min(),
                    (LikelinessConfidence)Math.Max((int)oldValue.Item2, (int)likeliness.Confidence)
                )
            );
        }

        /// <summary>
        /// Sends Email and SMS
        /// </summary>
        /// <param name="confidence">Likeliness that the person is identified</param>
        /// <param name="faceToken">Face token of the missing person</param>
        private void SendNotifications(LikelinessConfidence confidence, string faceToken)
        {
            DateTime time = DateTime.Now;

            ContactInformation information = new CallsToDb().GetMissingPersonData(faceToken);
            //if (information == null)
            //    return;
            LikelinessLevelData data = likelinessLevelData[confidence];

            string place = "";
            byte[] locationPicture = null;

            if (CameraProperties != null)
            {
                Bus bus = (CameraProperties.IsBus) ? new Bus(CameraProperties.BusId, time) : null;

                Location location = (CameraProperties.IsBus)
                    ? new Location(bus)
                    : new Location(CameraProperties.StreetName, CameraProperties.HouseNumber, CameraProperties.CityName, CameraProperties.CountryName, CameraProperties.PostalNumber);

                place = (CameraProperties.IsBus)
                    ? BusHelpers.GetBusLocation(bus)
                    : LocationHelpers.GetLocationName(location);

                string locationPictureUrl = (CameraProperties.IsBus)
                    ? LocationHelpers.CreateLocationPictureFromCoordinates(location)
                    : LocationHelpers.CreateLocationPictureFromAddress(location);

                using (WebClient client = new WebClient())
                {
                    locationPicture = client.DownloadData(locationPictureUrl);
                }
            }

            if (Mail.SendMail("deividas.brazenas@gmail.com", data.EmailSubject,
                    data.EmailBodyBeginning + "deividas" + " " +
                    "B" + data.EmailBodyEnding + place, new List<byte[]>() { locationPicture }, new List<string>() { "Location.jpeg" }) == null)
            {
                Debug.WriteLine("Mail message was not sent!");
            }

            //if (Mail.SendMail(information.contactPersonEmailAddress, data.EmailSubject,
            //        data.EmailBodyBeginning + information.missingPersonFirstName + " " +
            //        information.missingPersonLastName + data.EmailBodyEnding + place, new List<byte[]>() { locationPicture }, new List<string>() { "Location" }) == null)
            //{
            //    Debug.WriteLine("Mail message was not sent!");
            //}

            if (Sms.SendSms("+37068959812",
                    data.SmsBodyBeginning + "D" + " " +
                    "B" + data.SmsBodyEnding) == null)
            {
                Debug.WriteLine("Sms message was not sent!");
            }

            //if (Sms.SendSms(information.contactPersonPhoneNumber,
            //        data.SmsBodyBeginning + information.missingPersonFirstName + " " +
            //        information.missingPersonLastName + data.SmsBodyEnding) == null)
            //{
            //    Debug.WriteLine("Sms message was not sent!");
            //}
        }
    }

    public struct LikelinessLevelData
    {
        public int TimeLimit { get; set; }
        public string EmailSubject { get; set; }
        public string SmsBodyBeginning { get; set; }
        public string SmsBodyEnding { get; set; }
        public string EmailBodyBeginning { get; set; }
        public string EmailBodyEnding { get; set; }
    }
}
