
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
using Objects.CameraProperties;
using Objects.ContactInformation;

namespace FaceAnalysis
{
    public class SearchResultHandler
    {
        private const string NormalProbabilityEmailSubject = "Decent possibility that your missing person was detected!";
        private const string NormalProbabilitySmsBodyBeginning = "Good afternoon. There's a possibility that your missing person ";
        private const string NormalProbabilitySmsBodyEnding = " was detected. Please check you email for more detailed information.";
        private const string NormalProbabilityEmailBodyBeginning = "Good afternoon. There's a possibility that your missing person ";
        private const string NormalProbabilityEmailBodyEnding = " was detected at this location - ";

        private const string HighProbabilityEmailSubject = "HIGH possibility that your missing person was detected!";
        private const string HighProbabilitySmsBodyBeginning = "Good afternoon. There's a HIGH possibility that your missing person ";
        private const string HighProbabilitySmsBodyEnding = " was detected. Please check you email for more detailed information.";
        private const string HighProbabilityEmailBodyBeginning = "Good afternoon. There's a HIGH possibility that your missing person ";
        private const string HighProbabilityEmailBodyEnding = " was detected at this location - ";

        private const string VeryHighProbabilityEmailSubject = "VERY HIGH possibility that your missing person was detected!";
        private const string VeryHighProbabilitySmsBodyBeginning = "Good afternoon. There's a VERY HIGH possibility that your missing person ";
        private const string VeryHighProbabilitySmsBodyEnding = " was detected. Please check you email for more detailed information.";
        private const string VeryHighProbabilityEmailBodyBeginning = "Good afternoon. There's a VERY HIGH possibility that your missing person ";
        private const string VeryHighProbabilityEmailBodyEnding = " was detected at this location - ";

        private readonly Task notifactionSenderTask;
        private readonly ConcurrentDictionary<string, Tuple<DateTime, LikelinessConfidence>> notificationsToSend = new ConcurrentDictionary<string, Tuple<DateTime, LikelinessConfidence>>();
        private readonly ConcurrentDictionary<LikelinessConfidence, LikelinessLevelData> likelinessLevelData = new ConcurrentDictionary<LikelinessConfidence, LikelinessLevelData>();
        private CameraProperties CameraProperties;

        public SearchResultHandler(CancellationToken token)
        {
            LikelinessLevelData highLevelData = new LikelinessLevelData
            {
                TimeLimit = 60,
                EmailSubject = HighProbabilityEmailSubject,
                SmsBodyBeginning = HighProbabilitySmsBodyBeginning,
                SmsBodyEnding = HighProbabilitySmsBodyEnding,
                EmailBodyBeginning = HighProbabilityEmailBodyBeginning,
                EmailBodyEnding = HighProbabilityEmailBodyEnding,
            };
            LikelinessLevelData veryHighLevelData = new LikelinessLevelData
            {
                TimeLimit = 60,
                EmailSubject = VeryHighProbabilityEmailSubject,
                SmsBodyBeginning = VeryHighProbabilitySmsBodyBeginning,
                SmsBodyEnding = VeryHighProbabilitySmsBodyEnding,
                EmailBodyBeginning = VeryHighProbabilityEmailBodyBeginning,
                EmailBodyEnding = VeryHighProbabilityEmailBodyEnding,
            };
            LikelinessLevelData normalLevelData = new LikelinessLevelData
            {
                TimeLimit = 120,
                EmailSubject = NormalProbabilityEmailSubject,
                SmsBodyBeginning = NormalProbabilitySmsBodyBeginning,
                SmsBodyEnding = NormalProbabilitySmsBodyEnding,
                EmailBodyBeginning = NormalProbabilityEmailBodyBeginning,
                EmailBodyEnding = NormalProbabilityEmailBodyEnding,
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
                    if (!notificationsToSend.TryGetValue(likeliness.FaceToken, out var currentValue) || currentValue.Item2 < likeliness.Confidence)
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
            ContactInformation information = new CallsToDb().GetMissingPersonData(faceToken);

            if (information == null)
                return;

            LikelinessLevelData data = likelinessLevelData[confidence];

            byte[] locationPicture = null;
            string locationString = "";

            if (CameraProperties != null)
            {
                var bus = (CameraProperties.IsBus) ? new Bus(CameraProperties.BusId, DateTime.Now) : null;

                var location = (CameraProperties.IsBus)
                    ? new Location(bus)
                    : new Location(CameraProperties.StreetName, CameraProperties.HouseNumber, CameraProperties.CityName,
                        CameraProperties.CountryName, CameraProperties.PostalCode);

                locationString = (CameraProperties.IsBus)
                    ? BusHelpers.GetBusLocation(bus)
                    : LocationHelpers.LocationString(location);

                string locationPicUrl = (CameraProperties.IsBus)
                    ? LocationHelpers.CreateLocationPictureFromCoordinates(location)
                    : LocationHelpers.CreateLocationPictureFromAddress(location);

                using (WebClient client = new WebClient())
                {
                    locationPicture = client.DownloadData(locationPicUrl);
                }
            }

            if (Mail.SendMail(information.contactPersonEmailAddress, data.EmailSubject,
                    data.EmailBodyBeginning + information.missingPersonFirstName + " " +
                    information.missingPersonLastName + data.EmailBodyEnding + locationString,
                    new List<byte[]>() { locationPicture }, new List<string>() { "Location.jpeg" }) != null)
            {
                Debug.WriteLine("Mail message was sent!");
            }
            else
            {
                Debug.WriteLine("Mail message was not sent!");
            }

            if (Sms.SendSms(information.contactPersonPhoneNumber,
                    data.SmsBodyBeginning + information.missingPersonFirstName + " " +
                    information.missingPersonLastName + data.SmsBodyEnding) != null)
            {
                Debug.WriteLine("Sms message was sent!");
            }
            else
            {
                Debug.WriteLine("Sms message was not sent!");
            }
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
