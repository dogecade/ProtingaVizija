using System;
using System.Diagnostics;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace NotificationService
{
    public class Sms
    {
        public static string SendSms(string toNumber, string body)
        {
            TwilioClient.Init(Credentials.smsAccountSid, Credentials.smsAuthToken);

            try
            {
                MessageResource.Create(
                    body: body,
                    from: new Twilio.Types.PhoneNumber(Credentials.phoneNumber),
                    to: new Twilio.Types.PhoneNumber(toNumber)
                );

                return Guid.NewGuid().ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}