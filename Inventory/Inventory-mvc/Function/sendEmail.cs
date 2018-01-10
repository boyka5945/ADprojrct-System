using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Inventory_mvc.Function
{
    public class sendEmail
    {
        private SmtpClient mailClient;
        private MailMessage message;
        private string attachmentURI;
        private Attachment att;

        public sendEmail(string DstemailAddress, string subject, string content, string attachmentsURI = null)
        {
            mailClient = new SmtpClient("smtp.gmail.com");
            attachmentURI = attachmentsURI;
            mailClient.Credentials = new NetworkCredential("SA45Team3", "passw0rd123456");
            message = new MailMessage(new MailAddress("SA45Team3@gmail.com"), new MailAddress(DstemailAddress));
            mailClient.EnableSsl = true;
            message.IsBodyHtml = true;
            message.Body = content;
            message.Priority = MailPriority.Normal;
            message.Subject = subject;
        }

        public void send()
        {
            if (attachmentURI != null)
            {
                att = new Attachment(attachmentURI);
                message.Attachments.Add(att);
            }
            
            mailClient.Send(message);
        }
    }
}