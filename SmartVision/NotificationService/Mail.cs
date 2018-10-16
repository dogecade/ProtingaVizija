using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

        public void SendMail(string recipientMail, string subject, string body, byte[] pictureBytes = null)
        {
            try
            {
                mail.To.Add(recipientMail);
                mail.Subject = subject;
                mail.Body = body;

                if (pictureBytes != null)
                {
                    Bitmap picture = new Bitmap(new MemoryStream(pictureBytes));

                    var stream = new MemoryStream();
                    picture.Save(stream, ImageFormat.Jpeg);
                    stream.Position = 0;

                    mail.Attachments.Add(new Attachment(stream, "MissingPersonImage.jpeg"));
                }

                smtpServer.Send(mail);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}