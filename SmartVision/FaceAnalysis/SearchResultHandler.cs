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
using Quartz;
using Quartz.Impl;
using LocationService;
using Objects;
using Objects.CameraProperties;
using Objects.ContactInformation;
using Quartz.Impl.Matchers;

namespace FaceAnalysis
{
    public class SearchResultHandler
    {
        private const string emailSubject = "{0} possibility that your missing person was detected!";
        private const string smsBodyBeginning = "Good afternoon. There's a {0} possibility that your missing person ";
        private const string smsBodyEnding = " was detected. Please check you email for more detailed information.";
        private const string emailBodyBeginning = "Good afternoon. There's a {0} possibility that your missing person ";
        private const string emailBodyEnding = " was detected at this location - ";

        private readonly IScheduler scheduler;

        private static readonly ConcurrentDictionary<LikelinessConfidence, LikelinessLevelData> likelinessLevelData =
            new ConcurrentDictionary<LikelinessConfidence, LikelinessLevelData>
        (
            new List<KeyValuePair<LikelinessConfidence, LikelinessLevelData>>
            {
                new KeyValuePair<LikelinessConfidence, LikelinessLevelData>
                (
                    LikelinessConfidence.VeryHighProbability,
                    new LikelinessLevelData
                    {
                        TimeLimit = 60,
                        EmailSubject = string.Format(emailSubject, "VERY HIGH"),
                        SmsBodyBeginning = string.Format(smsBodyBeginning, "VERY HIGH"),
                        SmsBodyEnding = smsBodyEnding,
                        EmailBodyBeginning = string.Format(emailBodyBeginning, "VERY HIGH"),
                        EmailBodyEnding = emailBodyEnding,
                    }
                ),
                new KeyValuePair<LikelinessConfidence, LikelinessLevelData>
                (
                    LikelinessConfidence.HighProbability,
                    new LikelinessLevelData
                    {
                        TimeLimit = 60,
                        EmailSubject = string.Format(emailSubject, "HIGH"),
                        SmsBodyBeginning = string.Format(smsBodyBeginning, "HIGH"),
                        SmsBodyEnding = smsBodyEnding,
                        EmailBodyBeginning = string.Format(emailBodyBeginning, "HIGH"),
                        EmailBodyEnding = emailBodyEnding,
                    }
                ),
                new KeyValuePair<LikelinessConfidence, LikelinessLevelData>
                (
                    LikelinessConfidence.NormalProbability,
                    new LikelinessLevelData
                    {
                        TimeLimit = 120,
                        EmailSubject = string.Format(emailSubject, "average"),
                        SmsBodyBeginning = string.Format(smsBodyBeginning, "average"),
                        SmsBodyEnding = smsBodyEnding,
                        EmailBodyBeginning = string.Format(emailBodyBeginning, "average"),
                        EmailBodyEnding = emailBodyEnding,
                    }
                )
            }
        );

        public SearchResultHandler(CameraProperties cameraProperties)
        {
            scheduler = new StdSchedulerFactory().GetScheduler().GetAwaiter().GetResult();
            scheduler.Start();
            UpdateProperties(cameraProperties);
        }

        public void Complete()
        {
            scheduler.Shutdown();
        }

        public void UpdateProperties(CameraProperties cameraProperties)
        {
            scheduler.Context.Put("cameraProperties", cameraProperties);
        }

        /// <summary>
        /// Handles an incoming search result.
        /// </summary>
        /// <param name="likeliness">Search result to handle</param>
        public async Task HandleSearchResult(LikelinessResult likeliness)
        {
            Debug.WriteLine(string.Format("Handling search result with confidence={0}", likeliness.Confidence));
            if (await scheduler.CheckExists(new JobKey(likeliness.FaceToken, "timeout")))
                return;
            switch (likeliness.Confidence)
            {
                case LikelinessConfidence.LowProbability:
                    break;
                case LikelinessConfidence.NormalProbability:
                case LikelinessConfidence.HighProbability:
                    await ScheduleJobByLikeliness<SendNotificationJob>(likeliness, "notificationJob");
                    break;
                case LikelinessConfidence.VeryHighProbability:
                    var key = new JobKey(likeliness.FaceToken, "notificationJob");
                    IJobDetail instantNotificationJob = JobBuilder.Create<SendNotificationJob>()
                        .WithIdentity(key)
                        .UsingJobData("confidence", (int)likeliness.Confidence)
                        .Build();
                    await scheduler.AddJob(instantNotificationJob, true, true);
                    await scheduler.TriggerJob(key);
                    await ScheduleJobByLikeliness<TimeoutJob>(likeliness, "timeout");
                    break;
            }
        }

        /// <summary>
        /// Schedules a notification job.
        /// Detail depend on the likeliness level
        /// </summary>
        /// <typeparam name="JobType">Type of job to create</typeparam>
        /// <param name="likelinessResult">Likeliness result</param>
        /// <param name="group">Group of the job to create</param>
        /// <returns></returns>
        private async Task ScheduleJobByLikeliness<JobType>(LikelinessResult likelinessResult, string group) where JobType : IJob
        {
            var jobKey = new JobKey(likelinessResult.FaceToken, group);
            IJobDetail job = JobBuilder.Create<JobType>()
                .WithIdentity(jobKey)
                .UsingJobData("confidence", (int)likelinessResult.Confidence)
                .Build();
            ITrigger triggerByConfidence = TriggerBuilder.Create()
                .ForJob(job)
                .StartAt(DateTime.Now.AddSeconds(likelinessLevelData[likelinessResult.Confidence].TimeLimit))
                .Build();
            var oldJob = await scheduler.GetJobDetail(jobKey);
            if (oldJob != null)
            {
                var trigger = (await scheduler.GetTriggersOfJob(jobKey)).FirstOrDefault();
                var triggerToUse = trigger == null || trigger.StartTimeUtc > triggerByConfidence.StartTimeUtc ?
                    triggerByConfidence : trigger;
                if (oldJob.JobDataMap.GetInt("confidence") < (int)likelinessResult.Confidence)
                {
                    await scheduler.AddJob(job, true, true);
                    await scheduler.ScheduleJob(triggerToUse);
                }
                else
                    await scheduler.RescheduleJob(trigger.Key, triggerToUse);
                return;
            }
            else
            {
                await scheduler.AddJob(job, true, true);
                await scheduler.ScheduleJob(triggerByConfidence);
            }

        }

        /// <summary>
        /// Handles a single searchResult with no scheduling - sends it out if it meets the minimum confidence requirement.
        /// </summary>
        /// <returns></returns>
        public static async Task HandleOneResult(LikelinessResult result, CameraProperties cameraProperties = null)
        {
            await SendNotification(result.FaceToken, result.Confidence, cameraProperties);
        }

        /// <summary>
        /// Sends email and SMS
        /// </summary>
        /// <param name="faceToken"></param>
        /// <param name="confidence"></param>
        /// <param name="cameraProperties"></param>
        private static async Task SendNotification(string faceToken, LikelinessConfidence confidence, CameraProperties cameraProperties)
        {
            ContactInformation information = await new CallsToDb().GetMissingPersonData(faceToken);

            if (information == null)
                return;

            LikelinessLevelData data = likelinessLevelData[confidence];

            byte[] locationPicture = null;
            string locationString = "";

            if (cameraProperties != null)
            {
                var bus = (cameraProperties.IsBus) ? new Bus(cameraProperties.BusId, DateTime.Now) : null;

                var location = (cameraProperties.IsBus)
                    ? new Location(bus)
                    : (cameraProperties.Latitude == 0 && cameraProperties.Longitude == 0) ? new Location(cameraProperties.StreetName, cameraProperties.HouseNumber, cameraProperties.CityName,
                        cameraProperties.CountryName, cameraProperties.PostalCode) : LocationHelpers.CoordinatesToLocation(cameraProperties.Latitude, cameraProperties.Longitude);

                locationString = (cameraProperties.IsBus)
                    ? BusHelpers.GetBusLocation(bus)
                    : LocationHelpers.LocationString(location);

                string locationPicUrl = (cameraProperties.IsBus)
                    ? LocationHelpers.CreateLocationPictureFromCoordinates(location)
                    : LocationHelpers.CreateLocationPictureFromAddress(location);

                using (WebClient client = new WebClient())
                {
                    locationPicture = await client.DownloadDataTaskAsync(locationPicUrl);
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

        /// <summary>
        /// Dummy job to prevent "spam" of messages,
        /// to be scheduled after sending high probability notification
        /// </summary>
        internal class TimeoutJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                Debug.WriteLine(string.Format("Notifications for face token {0} can be sent again.", context.JobDetail.Key.Name));

                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Job to send email and SMS.
        /// </summary>
        [DisallowConcurrentExecution]
        internal class SendNotificationJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                var faceToken = context.JobDetail.Key.Name;
                var confidence = (LikelinessConfidence)context.JobDetail.JobDataMap.GetInt("confidence");
                Debug.WriteLine(string.Format("Executing notifaction job, faceToken={0}, confidence={1}", faceToken, confidence));
                var schedulerContext = context.Scheduler.Context;
                CameraProperties cameraProperties = (CameraProperties)schedulerContext.Get("cameraProperties");

                await SendNotification(faceToken, confidence, cameraProperties);

                //make sure repeated requests for this face token are not sent.
                await context.Scheduler.DeleteJob(context.JobDetail.Key);
            }
        }
        internal class LikelinessLevelData
        {
            public int TimeLimit { get; set; }
            public string EmailSubject { get; set; }
            public string SmsBodyBeginning { get; set; }
            public string SmsBodyEnding { get; set; }
            public string EmailBodyBeginning { get; set; }
            public string EmailBodyEnding { get; set; }
        }
    }
}