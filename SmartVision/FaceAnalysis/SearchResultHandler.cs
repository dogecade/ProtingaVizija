
using NotificationService;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public class SearchResultHandler
    {
        private const string normalProbabilityEmailSubject = "HIGH possibility that your missing person was detected!";
        private const string normalProbabilitySmsBodyBeginning = "Good afternoon. There's a possibility that your missing person ";
        private const string normalProbabilitySmsBodyEnding = " was detected. Please check you email for more detailed information.";
        private const string normalProbabilityEmailBodyBeginning = "Good afternoon. There's a possibility that your missing person ";
        private const string normalProbabilityEmailBodyEnding = " was detected. Please find attached frame in which your person was spotted.";

        private const string highProbabilityEmailSubject = "HIGH possibility that your missing person was detected!";
        private const string highProbabilitySmsBodyBeginning ="Good afternoon. There's a HIGH possibility that your missing person ";
        private const string highProbabilitySmsBodyEnding =" was detected. Please check you email for more detailed information.";
        private const string highProbabilityEmailBodyBeginning = "Good afternoon. There's a HIGH possibility that your missing person ";
        private const string highProbabilityEmailBodyEnding = " was detected. Please find attached frame in which your person was spotted.";

        private const string veryHighProbabilityEmailSubject ="VERY HIGH possibility that your missing person was detected!";
        private const string veryHighProbabilitySmsBodyBeginning ="Good afternoon. There's a VERY HIGH possibility that your missing person ";
        private const string veryHighProbabilitySmsBodyEnding =" was detected. Please check you email for more detailed information.";
        private const string veryHighProbabilityEmailBodyBeginning = "Good afternoon. There's a VERY HIGH possibility that your missing person ";
        private const string veryHighProbabilityEmailBodyEnding = " was detected. Please find attached frame in which your person was spotted.";

        private readonly Task notifactionSenderTask;
        private readonly ConcurrentDictionary<string, Tuple<DateTime, LikelinessConfidence>> notificationsToSend = new ConcurrentDictionary<string, Tuple<DateTime, LikelinessConfidence>>();
        private readonly ConcurrentDictionary<LikelinessConfidence, LikelinessLevelData> likelinessLevelData = new ConcurrentDictionary<LikelinessConfidence, LikelinessLevelData>();

        public SearchResultHandler(CancellationToken token)
        {
            LikelinessLevelData highLevelData = new LikelinessLevelData
            {
                TimeLimit = 60,
                EmailSubject = highProbabilityEmailSubject,
                SmsBodyBeginning = highProbabilitySmsBodyBeginning,
                SmsBodyEnding = highProbabilitySmsBodyEnding,
                EmailBodyBeginning = highProbabilityEmailBodyBeginning,
                EmailBodyEnding = highProbabilityEmailBodyEnding
            };
            LikelinessLevelData veryHighLevelData = new LikelinessLevelData
            {
                TimeLimit = 60,
                EmailSubject = veryHighProbabilityEmailSubject,
                SmsBodyBeginning = veryHighProbabilitySmsBodyBeginning,
                SmsBodyEnding = veryHighProbabilitySmsBodyEnding,
                EmailBodyBeginning = veryHighProbabilityEmailBodyBeginning,
                EmailBodyEnding = veryHighProbabilityEmailBodyEnding
            };
            LikelinessLevelData normalLevelData = new LikelinessLevelData
            {
                TimeLimit = 120,
                EmailSubject = normalProbabilityEmailSubject,
                SmsBodyBeginning = normalProbabilitySmsBodyBeginning,
                SmsBodyEnding = normalProbabilitySmsBodyEnding,
                EmailBodyBeginning = normalProbabilityEmailBodyBeginning,
                EmailBodyEnding = normalProbabilityEmailBodyEnding
            };
            likelinessLevelData.TryAdd(LikelinessConfidence.VeryHighProbability, veryHighLevelData);
            likelinessLevelData.TryAdd(LikelinessConfidence.NormalProbability, normalLevelData);
            likelinessLevelData.TryAdd(LikelinessConfidence.HighProbability, highLevelData);

            notifactionSenderTask = Task.Run(() => SenderTask(token));
        }

        public void SenderTask(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;
                foreach (var entry in notificationsToSend)
                {
                    if (entry.Value.Item1 < DateTime.Now)
                    {
                        if (entry.Value.Item2 < LikelinessConfidence.VeryHighProbability)
                            SendNotifications(confidence: entry.Value.Item2, faceToken: entry.Key);
                        notificationsToSend.TryRemove(entry.Key, out _);
                    }
                }
            }
        }

        public void HandleSearchResult(LikelinessResult likeliness)
        {
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
                    new[] { sendTime, oldValue.Item1 }.Max(),
                    (LikelinessConfidence)Math.Max((int)oldValue.Item2, (int)likeliness.Confidence)
                )
            );
        }

        private void SendNotifications(LikelinessConfidence confidence, string faceToken)
        {
            NecessaryContactInformation information = GetMissingPersonData(faceToken);
            LikelinessLevelData data = likelinessLevelData[confidence];
            Mail.SendMail(information.contactPersonEmailAddress, data.EmailSubject,
                          data.EmailBodyBeginning + information.missingPersonFirstName + " " +
                          information.missingPersonLastName + data.EmailBodyEnding);
            Sms.SendSms(information.contactPersonPhoneNumber, data.SmsBodyBeginning + information.missingPersonFirstName + " " + information.missingPersonLastName + data.SmsBodyEnding);
        }

        private static NecessaryContactInformation GetMissingPersonData(string matchedFace)
        {
            //TODO: Get missing person from db where identifier is facetoken
            string missingPersonFirstName = "name";
            string missingPersonLastName = "lastname";
            string contactPersonPhoneNumber = "+37068959812";
            string contactPersonEmailAddress = "deividas.brazenas@gmail.com";

            return new NecessaryContactInformation(missingPersonFirstName, missingPersonLastName,
                contactPersonPhoneNumber, contactPersonEmailAddress);
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