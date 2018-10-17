using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace NotificationService
{
    public class Mail
    {
        private readonly MailMessage mail;
        private readonly SmtpClient smtpServer;

        public Mail()
        {
            mail = new MailMessage();
            mail.From = new MailAddress(MailCredentials.mailAddress);

            smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Credentials = new System.Net.NetworkCredential(MailCredentials.mailAddress, MailCredentials.mailPassword);
            smtpServer.Port = 587;
            smtpServer.EnableSsl = true;
        }

        public void SendMail(string recipientMail, string subject, string body)
        {
            try
            {
                mail.To.Add(recipientMail);
                mail.Subject = subject;
                mail.Body = body;

                smtpServer.Send(mail);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}