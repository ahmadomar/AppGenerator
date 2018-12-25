using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace AppsGenerator.Classes.Utilities.Messages.Email
{
    public class EmailManagement
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        public string SendEmail(string To,string Subject,string Body)
        {
            this.Subject = Subject;
            this.Body = Body;
            return SendEmail(To);
        }


        private string SendEmail(string To)
        {
            try
            {
                // Command line argument must the the SMTP host.
                var client = new SmtpClient();
                
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Timeout = 10000;

                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("", "");
                
                MailAddress from = new MailAddress("", "AppsGenerator");
                MailAddress to = new MailAddress(To);
                MailMessage msg = new MailMessage(from, to);
                msg.Sender = new MailAddress("");

                msg.Subject = Subject;
                msg.Body = Body;
                msg.IsBodyHtml = true;
                msg.BodyEncoding = UTF8Encoding.UTF8;
                msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                msg.Priority = MailPriority.High;

                client.Send(msg);

                return "Received";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string SendEmail(String to, String header)
        {
            string emailPath = HostingEnvironment.MapPath("~/App_Data/email.html");
            String body = System.IO.File.ReadAllText(emailPath);


            body = body.Replace("{header}", header);
            body = body.Replace("{body}", body);

            return SendEmail(to);
        }
    }
}
