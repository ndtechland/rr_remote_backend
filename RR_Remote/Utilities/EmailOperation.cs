using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace RR_Remote.Utilities
{
    public class EmailOperation
    {
        private readonly string _smptuser;
        private readonly string _smptpass;
        public EmailOperation(IConfiguration configuration)
        {
            _smptuser = configuration.GetValue<string>("Email:smptuser");
            _smptpass = configuration.GetValue<string>("Email:smptpass");
        }
        public  int SendEmail(EmailEF emailef)
        {

            try
            {
                string sender = _smptuser;
                string password = _smptpass;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sender);
                message.To.Add(emailef.Email);
                message.Subject = emailef.Subject;
                message.Body = emailef.Message;
                System.Net.Mail.Attachment attachment;

                // Download the file from the URL.
                //attachment = new System.Net.Mail.Attachment(MedicinePdf());

                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(sender, password);
                client.EnableSsl = true;
                client.Send(message);

                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        public class EmailEF
        {
            public string Email { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
        }
    }
}