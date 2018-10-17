using System;
using System.Diagnostics;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace NotificationService
{
    public class Sms
    {
        public static void SendSms(string toNumber, string body)
        {
            TwilioClient.Init(Credentials.smsAccountSid, Credentials.smsAuthToken);

            try
            {
                var message = MessageResource.Create(
                    body: body,
                    from: new Twilio.Types.PhoneNumber(Credentials.phoneNumber), 
                    to: new Twilio.Types.PhoneNumber(toNumber)
                );
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}