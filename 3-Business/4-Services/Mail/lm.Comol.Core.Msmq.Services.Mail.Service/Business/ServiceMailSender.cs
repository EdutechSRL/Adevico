using lm.Comol.Core.Msmq.Services.Mail.DataContract.Domain.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace lm.Comol.Core.Msmq.Services.Mail.Service
{
    public partial class ServiceMailSender 
    {
        //private MailException InternalSendMail(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage, Boolean replaceCRLF = false)
        //{
        //    try
        //    {
        //        MailMessage message = CreateMessage(idUserLanguage, dLanguage, dtoMessage, replaceCRLF);
        //        SmtpClient smtpClient = CreateSmtpClient();
        //        List<dtoRecipients> recipients = new List<dtoRecipients>();
        //        dtoMessage.To.ForEach(t => recipients.Add(new dtoRecipients() { Mail = t, Type = RecipientType.To }));
        //        dtoMessage.CC.ForEach(t => recipients.Add(new dtoRecipients() { Mail = t, Type = RecipientType.CC }));
        //        dtoMessage.BCC.ForEach(t => recipients.Add(new dtoRecipients() { Mail = t, Type = RecipientType.BCC }));
        //        Int32 maxRecipients = SMTPconfig.MaxRecipients;
        //        int Total = recipients.Count;
        //        int PageNumber = 0;
        //        if (maxRecipients > 0)
        //        {
        //            if (recipients.Count / maxRecipients < 1)
        //                PageNumber = 0;
        //            else
        //                PageNumber = (recipients.Count % maxRecipients == 0 ? recipients.Count / maxRecipients - 1 : (recipients.Count / maxRecipients));
        //        }

        //        for (int i = 0; i <= PageNumber; i++)
        //        {
        //            List<dtoRecipients> paged = null;
        //            if (SMTPconfig.MaxRecipients > 0)
        //                paged = recipients.Skip(maxRecipients * i).Take(maxRecipients).ToList();
        //            else
        //                paged = recipients;

        //            if (paged != null && paged.Count > 0)
        //            {
        //                message.To.Clear();
        //                message.CC.Clear();
        //                message.Bcc.Clear();
        //                paged.Where(p => p.Type == RecipientType.To).ToList().ForEach(p => message.To.Add(p.Mail));
        //                paged.Where(p => p.Type == RecipientType.CC).ToList().ForEach(p => message.CC.Add(p.Mail));
        //                paged.Where(p => p.Type == RecipientType.BCC).ToList().ForEach(p => message.Bcc.Add(p.Mail));

        //                smtpClient.Send(message);
        //            }
        //        }
        //        if (MailSettings.CopyToSender)
        //        {
        //            message.To.Clear();
        //            message.CC.Clear();
        //            message.Bcc.Clear();
        //            message.Subject = SubjectForCopy(idUserLanguage, dLanguage, dtoMessage);
        //            message.To.Add(new MailAddress(SMTPconfig.System.Sender.Address, dtoMessage.FromUser.Address));
        //            smtpClient.Send(message);
        //        }
        //        return MailException.MailSent;
        //    }
        //    catch (ArgumentOutOfRangeException outOfRangeException)
        //    {
        //        return MailException.NoDestinationAddress;
        //    }
        //    catch (SmtpException smtpException)
        //    {
        //        return MailException.SMTPunavailable;
        //    }
        //    catch (Exception ex)
        //    {
        //        return MailException.UnknownError;
        //    }
        //}
        private SmtpClient CreateSmtpClient(SmtpServiceConfig config)
        {
            SmtpClient smtpClient = new SmtpClient(config.Host, config.Port);
            smtpClient.EnableSsl = config.Authentication.EnableSsl;
            if (config.Authentication.Enabled)
                smtpClient.Credentials = new System.Net.NetworkCredential(config.Authentication.AccountName, config.Authentication.AccountPassword);
            return smtpClient;
        }
        private MailMessage CreateMessage(Int32 idUserLanguage, lm.Comol.Core.DomainModel.Language dLanguage, lm.Comol.Core.Msmq.Services.Mail.DataContract.Domain.Messages.Message dtoMessage, Boolean replaceCRLF = false)
        {
            MailMessage message = new System.Net.Mail.MailMessage();
            //message.BodyEncoding = System.Text.Encoding.Unicode;
            //message.SubjectEncoding = System.Text.Encoding.Unicode;
            message.Sender = Sender(dtoMessage);
            message.From = From(dtoMessage);
            if (dtoMessage.ReplyTo != null)
                message.ReplyToList.Add(dtoMessage.ReplyTo);
            if (!SMTPconfig.RelayAllowed && !message.ReplyToList.Any())
                message.ReplyToList.Add(message.From);


            if (MailSettings.NotifyToSender && message.ReplyToList.Any())
            {
                try
                {
                    if (!string.IsNullOrEmpty(dtoMessage.ReplyTo.DisplayName))
                    {
                        message.Headers.Add("Disposition-Notification-To", dtoMessage.ReplyTo.DisplayName + " <" + dtoMessage.ReplyTo.Address + ">");
                    }
                    else
                    {
                        message.Headers.Add("Disposition-Notification-To", "<" + dtoMessage.ReplyTo.Address + ">");
                    }

                }
                catch (Exception ex)
                {
                }
            }


            message.Subject = Subject(idUserLanguage, dLanguage, dtoMessage);
            message.IsBodyHtml = MailSettings.isHtml;
            if (dtoMessage.Attachments != null && dtoMessage.Attachments.Count > 0)
                dtoMessage.Attachments.ForEach(a => message.Attachments.Add(a));
            message.Body = (message.IsBodyHtml && replaceCRLF ? dtoMessage.Body.Replace("\r\n", "<br>") : dtoMessage.Body);
            if (MailSettings.SignatureType == SignatureType.FromNoReplySettings)
                message.Body += ((message.IsBodyHtml) ? "<br/><br/>" : "\r\n\r\n\r\n") + SMTPconfig.System.NoReplySignature(idUserLanguage, dLanguage);
            else if (MailSettings.SignatureType == SignatureType.FromConfigurationSettings)
                message.Body += ((message.IsBodyHtml) ? "<br/><br/>" : "\r\n\r\n\r\n") + SMTPconfig.System.Signature(idUserLanguage, dLanguage);
            return message;
        }
        #region "Default Values"

            private String GetSubject(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage)
            {
                return (MailSettings.Subject == SubjectType.Free) ? dtoMessage.UserSubject : SMTPconfig.System.SubjectPrefix(idUserLanguage, dLanguage) + dtoMessage.UserSubject;
            }
            private String SubjectForCopy(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage)
            {
                return (MailSettings.Subject == SubjectType.Free) ? SMTPconfig.System.SubjectForSenderCopy(idUserLanguage, dLanguage) + dtoMessage.UserSubject : SMTPconfig.System.SubjectPrefix(idUserLanguage, dLanguage) + SMTPconfig.System.SubjectForSenderCopy(idUserLanguage, dLanguage) + dtoMessage.UserSubject;
            }
            private MailAddress From(dtoMailMessage dtoMessage)
            {
                return (MailSettings.Sender == SenderType.System) ? SMTPconfig.System.Sender : dtoMessage.FromUser;
                //return (SMTPconfig.RelayAllowed) ? ((MailSettings.Sender == SenderType.System) ? SMTPconfig.SystemSender : dtoMessage.FromUser) : ((SMTPconfig.SystemSender.Address ==dtoMessage.FromUser.Address) ?  SMTPconfig.SystemSender : new MailAddress( SMTPconfig.SystemSender.Address, dtoMessage.FromUser.Address)) ;
            }
            private MailAddress Sender(dtoMailMessage dtoMessage)
            {
                return (SMTPconfig.RelayAllowed) ? ((MailSettings.Sender == SenderType.System || dtoMessage.FromUser == null) ? SMTPconfig.System.Sender : dtoMessage.FromUser) : SMTPconfig.System.Sender;
            }
        #endregion
    }
}

//namespace lm.Comol.Core.Msmq.Services.Mail.Service
//{
//    class Class2
//    {
//        public class MailService
//    {
//        #region "Person"
//            private SmtpConfig SMTPconfig {get;set;}
//            private dtoMailSettings MailSettings {get;set;}
//        #endregion

//        public MailService(SmtpConfig smtp, dtoMailSettings settings) 
//        {
//            SMTPconfig = smtp;
//            MailSettings = settings;
//        }
//        public MailException SendMail(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage, Boolean replaceCRLF = false)
//        {
//            return InternalSendMail(idUserLanguage, dLanguage,dtoMessage, replaceCRLF);
//        }
//        public MailException SendMail(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage, String ToAdresses, RecipientType type, Boolean replaceCRLF = false)
//        {
//            return (dtoMessage.AddAddresses(ToAdresses, type) == MailException.None) ? InternalSendMail(idUserLanguage, dLanguage, dtoMessage, replaceCRLF) : MailException.InvalidAddress;
//        }
   
//    }
//}
