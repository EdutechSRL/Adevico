using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using lm.ErrorsNotification.DataContract.Domain;

namespace lm.ErrorsNotification.Service.Dal
{
    public class DataHelpers
    {
        public static string ConnectionString()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DBconnection"];
        }

        internal static void SendMessage(ErrorSettings setting, String subject, String body)
        {
            MailMessage message = new MailMessage();

            message.IsBodyHtml = true;
            message.Body = body;
            message.From = new MailAddress(setting.SenderMail, setting.SenderName);
            message.Priority = MailPriority.High;
            message.ReplyTo = new MailAddress(setting.ReplyTo);
            message.Subject = subject;
            message.To.Add(new MailAddress(setting.RecipientMail));
            try
            {
                SmtpClient client = new SmtpClient(setting.HostSMTP, (setting.Port>0) ? setting.Port : 25);
                client.EnableSsl = setting.UseSsl;
                if (setting.UseAuthentication && !String.IsNullOrEmpty(setting.AccountName))
                    client.Credentials = new System.Net.NetworkCredential(setting.AccountName, setting.AccountPassword);
                client.Send(message);

            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("SendMessage: ", ex.Message);
                throw ex;
            }

        }

    }
}
