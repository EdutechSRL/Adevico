using lm.Comol.Core.DomainModel;
using lm.Comol.Core.MailCommons.Domain;
using lm.Comol.Core.MailCommons.Domain.Configurations;
using lm.Comol.Core.MailCommons.Domain.Messages;
using lm.Comol.Core.Msmq.Services.Mail.Service.Config;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
namespace lm.Comol.Core.Msmq.Services.Mail.Service.Business
{
	public class InternalMailService : IDisposable 
	{
        #region "Configuration Settings"
            private lm.Comol.Core.Msmq.Services.Mail.Service.Config.IstanceConfig IstanceConfig { get; set; }
            private SmtpServiceConfig SmtpConfig { get; set; }
            public String AttachmentBasePath { get; set; }
            public String AttachmentSentBasePath { get; set; }
            private ISession Session { get; set; }
            private lm.Comol.Core.Business.BaseModuleManager manager { get; set; }
            private lm.Comol.Core.TemplateMessages.Business.TemplateMessagesService templateService { get; set; }

            private lm.Comol.Core.Business.BaseModuleManager Manager { 
                get {
                    if (manager==null)
                        manager = new Core.Business.BaseModuleManager(new lm.Comol.Core.Data.DataContext(Session));
                    return manager;
                } 
            }
            private lm.Comol.Core.TemplateMessages.Business.TemplateMessagesService TemplateService
            {
                get
                {
                    if (templateService == null)
                        templateService = new lm.Comol.Core.TemplateMessages.Business.TemplateMessagesService(new lm.Comol.Core.Data.DataContext(Session));
                    return templateService;
                }
            }
        #endregion

        public InternalMailService(lm.Comol.Core.Msmq.Services.Mail.Service.Config.IstanceConfig istance, ISession session)
        {
            Session = session;
            IstanceConfig = istance;
            if (istance != null) {
                SmtpConfig = (istance.SmtpConfig != null) ? istance.SmtpConfig : null;
                AttachmentBasePath = istance.AttachmentUploadPath;
                AttachmentSentBasePath = istance.AttachmentSentPath;
            }
        }
        public InternalMailService(SmtpServiceConfig smtp, lm.Comol.Core.Msmq.Services.Mail.Service.Config.IstanceConfig istance, ISession session) 
		{
            Session = session;
            IstanceConfig = istance;
            SmtpConfig = smtp;
            if (istance != null)
            {
                SmtpConfig = (istance.SmtpConfig != null) ? istance.SmtpConfig : null;
                AttachmentBasePath = istance.AttachmentUploadPath;
                AttachmentSentBasePath = istance.AttachmentSentPath;
            }
		}

        #region "Send Mail Methods"
            #region "Send Methods"
                #region "Generic Mail"
                    public Boolean SendMail(Int32 idUser, Int32 idCommunity, String moduleCode, Message dtoMessage, Boolean replaceCRLF = false, String messagePath = "", bool saveMessage = false, String messageSavedPath = "")
                    {
                        return SendMail(Manager.GetDefaultIdLanguage(), Manager.GetLitePerson(idUser),idCommunity, moduleCode, dtoMessage, replaceCRLF, messagePath, saveMessage, messageSavedPath);
                    }
                    public Boolean SendMail(Int32 idDefaultLanguage, litePerson sender, Int32 idCommunity, String moduleCode, Message dtoMessage, Boolean replaceCRLF = false, String messagePath = "", bool saveMessage = false, String messageSavedPath = "")
                    {
                        Boolean result = false;
                        try
                        {
                            String savedPath = GetAttachmentsFullPath(IstanceConfig.AttachmentSentPath, messageSavedPath);

                            ParseAttachments(dtoMessage, messagePath);
                            List<dtoTranslatedMessage> messages= SendMessage(CreateSmtpClient(), sender,  dtoMessage, replaceCRLF);

                            if (messages.Any(m => m.Sent))
                            {
                                if (!saveMessage && !String.IsNullOrEmpty(messagePath))
                                    RemoveAttachments(dtoMessage.Attachments, messagePath, (messagePath != IstanceConfig.AttachmentUploadPath));
                                else if (saveMessage)
                                    SaveMessage(idCommunity, moduleCode, sender, dtoMessage, messages, savedPath);
                            }
                            else if (messages.Any(m => m.Exception == MailExceptionType.AuthenticationError || m.Exception == MailExceptionType.SMTPunavailable))
                                ErrorHandler.addMessageToPoisonQueue(dtoMessage);
                            else {
                                if (!saveMessage && !String.IsNullOrEmpty(messagePath))
                                    RemoveAttachments(dtoMessage.Attachments, messagePath, (messagePath != IstanceConfig.AttachmentUploadPath));
                            }
                        }
                        catch (Exception ex)
                        {
                            result = false;
                        }
                        return result;
                    }
                    private List<dtoTranslatedMessage> SendMessage(SmtpClient smtpClient, litePerson sender, Message dtoMessage, Boolean replaceCRLF=false)
                    {
                        return SendMessage(smtpClient, sender, dtoMessage.Settings, TranslateMessage(dtoMessage.Settings, dtoMessage.Subject, dtoMessage.Body, dtoMessage.PlainBody, dtoMessage.Recipients, dtoMessage.Attachments, replaceCRLF, dtoMessage.UniqueIdentifier, dtoMessage.FatherUniqueIdentifier));
                    }
    
                #endregion

                #region "From notifications"
                    public Boolean SendMail(int idUser, string moduleCode, Notification.Domain.dtoModuleNotificationMessage nMessage, Boolean replaceCRLF = true , string messagePath = "", string messageSavedPath = "")
                    {
                        Boolean result = false;
                        try
                        {
                            Int32 idDefaultLanguage = Manager.GetDefaultIdLanguage();
                            litePerson sender = Manager.GetLitePerson(idUser);

                            String savedPath = GetAttachmentsFullPath(IstanceConfig.AttachmentSentPath, messageSavedPath);
                            ParseAttachments(nMessage, messagePath);
                            List<dtoTranslatedMessage> messages = SendMessage(CreateSmtpClient(), sender, nMessage, replaceCRLF);

                            if (messages.Any(m => m.Sent))
                            {
                                if (!nMessage.Save && !String.IsNullOrEmpty(messagePath))
                                    RemoveAttachments(nMessage.Attachments, messagePath, (messagePath != IstanceConfig.AttachmentUploadPath));
                                else if (nMessage.Save)
                                    SaveMessage(moduleCode, sender, nMessage, messages, savedPath);
                            }
                            else if (messages.Any(m => m.Exception == MailExceptionType.AuthenticationError || m.Exception == MailExceptionType.SMTPunavailable))
                                ErrorHandler.addMessageToPoisonQueue(nMessage);
                            else
                            {
                                if (!nMessage.Save && !String.IsNullOrEmpty(messagePath))
                                    RemoveAttachments(nMessage.Attachments, messagePath, (messagePath != IstanceConfig.AttachmentUploadPath));
                            }
                        }
                        catch (Exception ex)
                        {
                            result = false;
                        }
                        return result;
                    }
                    private List<dtoTranslatedMessage> SendMessage(SmtpClient smtpClient, litePerson sender, Notification.Domain.dtoModuleNotificationMessage nMessage, Boolean replaceCRLF)
                    {
                        return SendMessage(smtpClient, sender, nMessage.MailSettings, TranslateMessage(nMessage.MailSettings, nMessage.Translation.Subject, nMessage.Translation.Body, nMessage.Translation.PlainBody, nMessage.Recipients, nMessage.Attachments, replaceCRLF, nMessage.UniqueIdentifier, nMessage.FatherUniqueIdentifier, nMessage.LanguageCode));
                    }
                    public Boolean SendMail(int idUser, string moduleCode, Notification.Domain.GroupMessages group, Boolean replaceCRLF = true, string messagePath = "", string messageSavedPath = "")
                    {
                        Boolean result = false;
                        try
                        {
                            Int32 idDefaultLanguage = Manager.GetDefaultIdLanguage();
                            litePerson sender = Manager.GetLitePerson(idUser);

                            String savedPath = GetAttachmentsFullPath(IstanceConfig.AttachmentSentPath, messageSavedPath);
                            ParseAttachments(group, messagePath);
                            List<dtoTranslatedMessage> messages = SendMessage(CreateSmtpClient(), sender, group, replaceCRLF);

                            if (messages.Any(m => m.Sent))
                            {
                                if (!group.Settings.Save && !String.IsNullOrEmpty(messagePath))
                                    RemoveAttachments(group.Settings.Attachments, messagePath, (messagePath != IstanceConfig.AttachmentUploadPath));
                                else if (group.Settings.Save)
                                    SaveMessage(moduleCode, sender, group, messages, savedPath);
                            }
                            else if (messages.Any(m => m.Exception == MailExceptionType.AuthenticationError || m.Exception == MailExceptionType.SMTPunavailable))
                                ErrorHandler.addMessageToPoisonQueue(group);
                            else
                            {
                                if (!group.Settings.Save && !String.IsNullOrEmpty(messagePath))
                                    RemoveAttachments(group.Settings.Attachments, messagePath, (messagePath != IstanceConfig.AttachmentUploadPath));
                            }
                        }
                        catch (Exception ex)
                        {
                            result = false;
                        }
                        return result;
                    }

                    private List<dtoTranslatedMessage> SendMessage(SmtpClient smtpClient, litePerson sender, Notification.Domain.GroupMessages group, Boolean replaceCRLF)
                    {
                        List<dtoTranslatedMessage> messages = new List<dtoTranslatedMessage>();
                        foreach (lm.Comol.Core.Notification.Domain.dtoModuleTranslatedMessage mMessage in group.Messages)
                        {
                            messages.AddRange(TranslateMessage(group.Settings.Mail, mMessage.Translation.Subject, mMessage.Translation.Body, mMessage.Translation.PlainBody, mMessage.Recipients,
                                  group.Settings.Attachments, replaceCRLF, group.Settings.UniqueIdentifier, group.Settings.FatherUniqueIdentifier));

                        }
                        return SendMessage(smtpClient, sender, group.Settings.Mail, messages);
                    }
                #endregion
                    //private Boolean InternalSendMail(Int32 idDefaultLanguage, litePerson sender, String moduleCode, Message dtoMessage, Boolean replaceCRLF = false)
                //{
                //    Boolean result = false;
                //    try
                //    {
                //        using (MailMessage message = new MailMessage())
                //        {
                //            GenerateMessage(message, idDefaultLanguage, sender, dtoMessage, replaceCRLF);
                //            SmtpClient smtpClient = CreateSmtpClient();
                //            List<Recipient> recipients = GetMessageRecipients(dtoMessage);
                //            Int32 maxRecipients = (SmtpConfig != null) ? SmtpConfig.MaxRecipients : 200;

                //            int total = recipients.Count;
                //            int pageNumber = 0;
                //            if (maxRecipients > 0)
                //            {
                //                if (recipients.Count / maxRecipients < 1)
                //                    pageNumber = 0;
                //                else
                //                    pageNumber = (recipients.Count % maxRecipients == 0 ? recipients.Count / maxRecipients - 1 : (recipients.Count / maxRecipients));
                //            }

                //            for (int i = 0; i <= pageNumber; i++)
                //            {
                //                List<Recipient> paged = null;
                //                if (maxRecipients > 0)
                //                    paged = recipients.Skip(maxRecipients * i).Take(maxRecipients).ToList();
                //                else
                //                    paged = recipients;

                //                if (paged != null && paged.Count > 0)
                //                {
                //                    message.To.Clear();
                //                    message.CC.Clear();
                //                    message.Bcc.Clear();
                //                    paged.Where(p => p.Type == MailRecipientType.To).ToList().ForEach(p => message.To.Add(new MailAddress(p.Address, p.DisplayName)));
                //                    paged.Where(p => p.Type == MailRecipientType.CC).ToList().ForEach(p => message.CC.Add(new MailAddress(p.Address, p.DisplayName)));
                //                    paged.Where(p => p.Type == MailRecipientType.BCC).ToList().ForEach(p => message.Bcc.Add(new MailAddress(p.Address, p.DisplayName)));

                //                    smtpClient.Send(message);
                //                }
                //            }
                //            if (dtoMessage.Settings.CopyToSender)
                //            {
                //                message.To.Clear();
                //                message.CC.Clear();
                //                message.Bcc.Clear();
                //                //if (message.IsBodyHtml)
                //                //    message.Subject = System.Net.WebUtility.HtmlEncode(GetSubjectForCopy((sender != null) ? sender.LanguageID : idDefaultLanguage, idDefaultLanguage, dtoMessage));
                //                //else
                //                message.Subject = GetSubjectForCopy((sender != null) ? sender.LanguageID : idDefaultLanguage, idDefaultLanguage, dtoMessage);
                //                if (message.ReplyToList.Any())
                //                    message.To.Add(message.ReplyToList.FirstOrDefault());
                //                else
                //                    message.To.Add(message.Sender);
                //                smtpClient.Send(message);
                //            }
                //        }
                //        result = true;
                //    }
                //    catch (ArgumentNullException nullMessageException)
                //    {
                //        throw new MailException(nullMessageException, MailExceptionType.NullMessage);
                //    }
                //    catch (ArgumentOutOfRangeException outOfRangeException)
                //    {
                //        throw new MailException(outOfRangeException, MailExceptionType.NoDestinationAddress);
                //    }
                //    catch (SmtpFailedRecipientsException failedException)
                //    {
                //        throw new MailException(failedException, MailExceptionType.InvalidAddress);
                //    }
                //    catch (SmtpException smtpException)
                //    {
                //        throw new MailException(smtpException, MailExceptionType.SMTPunavailable);
                //    }
                //    catch (Exception ex)
                //    {
                //        throw new MailException(ex, MailExceptionType.UnknownError);
                //    }
                //    return result;
                //}

               
                private List<dtoTranslatedMessage> SendMessage(SmtpClient smtpClient, litePerson sender, MessageSettings settings, List<dtoTranslatedMessage> messages)
                {
                    MailAddress replyTo = (sender == null || settings.SenderType== SenderUserType.System) ? null : new System.Net.Mail.MailAddress(sender.Mail, sender.SurnameAndName);                   
                    return SendMessage(smtpClient, GetSender(sender, settings, SmtpConfig), GetFrom(sender, settings, SmtpConfig), replyTo, messages, settings.NotifyToSender, settings.IsBodyHtml, settings.IsBodyHtml);
                }
                private List<dtoTranslatedMessage> SendMessage(SmtpClient smtpClient, MailAddress senderMail, MailAddress fromMail, MailAddress replyTo, List<dtoTranslatedMessage> messages, Boolean notifyToSender, Boolean isBodyHtml, Boolean replaceCRLF = false)
                {
                    foreach (dtoTranslatedMessage m in messages)
                    {
                        if (m.Recipients.Any())
                            SendMailMessage(smtpClient, replyTo, senderMail, fromMail, notifyToSender, isBodyHtml, m, replaceCRLF);
                        else
                            m.Sent = false;
                    }
                    return messages;
                }
                /// <summary>
                /// Send translated message to recipients
                /// </summary>
                /// <param name="smtpClient"></param>
                /// <param name="replyTo"></param>
                /// <param name="sender"></param>
                /// <param name="from"></param>
                /// <param name="notifyToSender"></param>
                /// <param name="isBodyHtml"></param>
                /// <param name="dtoMessage"></param>
                /// <param name="replaceCRLF"></param>
                private void SendMailMessage(SmtpClient smtpClient,MailAddress replyTo,MailAddress sender,MailAddress from,Boolean notifyToSender, Boolean isBodyHtml,dtoTranslatedMessage dtoMessage, Boolean replaceCRLF = false)
                {
                    Boolean sent = false;
                    try
                    {
                        using (MailMessage message = CreateMessage(replyTo, sender, from, notifyToSender, isBodyHtml,dtoMessage, replaceCRLF))
                        {
                            List<Recipient> recipients = dtoMessage.Recipients;
                            Int32 maxRecipients = (SmtpConfig != null) ? SmtpConfig.MaxRecipients : 200;

                            int total = recipients.Count;
                            int pageNumber = 0;
                            if (maxRecipients > 0)
                            {
                                if (recipients.Count / maxRecipients < 1)
                                    pageNumber = 0;
                                else
                                    pageNumber = (recipients.Count % maxRecipients == 0 ? recipients.Count / maxRecipients - 1 : (recipients.Count / maxRecipients));
                            }

                            for (int i = 0; i <= pageNumber; i++)
                            {
                                List<Recipient> paged = null;
                                if (maxRecipients > 0)
                                    paged = recipients.Skip(maxRecipients * i).Take(maxRecipients).ToList();
                                else
                                    paged = recipients;

                                if (paged != null && paged.Count > 0)
                                {
                                    message.To.Clear();
                                    message.CC.Clear();
                                    message.Bcc.Clear();
                                    paged.Where(p => p.Type == RecipientType.To).ToList().ForEach(p => message.To.Add(new MailAddress(p.Address, p.DisplayName)));
                                    paged.Where(p => p.Type == RecipientType.CC).ToList().ForEach(p => message.CC.Add(new MailAddress(p.Address, p.DisplayName)));
                                    paged.Where(p => p.Type == RecipientType.BCC).ToList().ForEach(p => message.Bcc.Add(new MailAddress(p.Address, p.DisplayName)));

                                    smtpClient.Send(message);
                                }
                            }
                        }
                        dtoMessage.Sent = true;
                    }
                    catch (ArgumentNullException nullMessageException)
                    {
                        dtoMessage.Sent = sent;
                        dtoMessage.Exception = MailExceptionType.NullMessage; 
                    }
                    catch (ArgumentOutOfRangeException outOfRangeException)
                    {
                        dtoMessage.Sent = sent;
                        dtoMessage.Exception = MailExceptionType.NoDestinationAddress; 
                    }
                    catch (SmtpFailedRecipientsException failedException)
                    {
                        dtoMessage.Sent = sent;
                        dtoMessage.Exception = MailExceptionType.InvalidAddress;
                    }
                    catch (SmtpException smtpException)
                    {
                        dtoMessage.Sent = sent;
                        dtoMessage.Exception = MailExceptionType.SMTPunavailable;
                    }
                    catch (Exception ex)
                    {
                        dtoMessage.Sent = sent;
                        dtoMessage.Exception = MailExceptionType.UnknownError;
                    }
                }

                /// <summary>
                /// Create Mail Message
                /// </summary>
                /// <param name="replyTo"></param>
                /// <param name="sender"></param>
                /// <param name="from"></param>
                /// <param name="notifyToSender"></param>
                /// <param name="isBodyHtml"></param>
                /// <param name="dtoMessage"></param>
                /// <param name="replaceCRLF"></param>
                /// <returns></returns>
                private MailMessage CreateMessage(MailAddress replyTo, MailAddress sender, MailAddress from, Boolean notifyToSender, Boolean isBodyHtml, dtoTranslatedMessage dtoMessage, Boolean replaceCRLF = false)
                {
                    MailMessage message = new MailMessage();

                    //mailMessage.Headers.Add("Message-Id",
                    //     String.Format("<{0}@{1}>",
                    //     Guid.NewGuid().ToString(),
                    //    "mail.example.com"));

                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.SubjectEncoding = System.Text.Encoding.UTF8;
                    message.Sender = sender;
                    message.From = from;
                    if (replyTo != null)
                        message.ReplyToList.Add(replyTo);
                    if (SmtpConfig != null && !SmtpConfig.RelayAllowed && !message.ReplyToList.Any())
                        message.ReplyToList.Add(message.From);

                    if (notifyToSender && message.ReplyToList.Any())
                    {
                        try
                        {
                            foreach (MailAddress r in message.ReplyToList)
                            {
                                if (!string.IsNullOrEmpty(r.DisplayName))
                                    message.Headers.Add("Disposition-Notification-To", r.DisplayName + " <" + r.Address + ">");
                                else
                                    message.Headers.Add("Disposition-Notification-To", "<" + r.Address + ">");
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    message.IsBodyHtml = isBodyHtml;
                    message.Subject = dtoMessage.Subject;

                    if (dtoMessage.Attachments != null && dtoMessage.Attachments.Count > 0)
                        AddAttachments(message, dtoMessage.Attachments);
                   
                    //else
                    //    body = System.Net.WebUtility.HtmlEncode(body.Replace("<br>", "\r\n"));
                    message.Body = dtoMessage.Body;
                    return message;
                }

                /// <summary>
                /// Get message to send, find recipients default language and create translated message to send
                /// </summary>
                /// <param name="dtoMessage"></param>
                /// <returns></returns>
                private List<dtoTranslatedMessage> TranslateMessage(MessageSettings settings, String subject, String body, String plainBody, List<Recipient> mRecipients, List<String> attachments, Boolean replaceCRLF, Guid uniqueIdentifier, Guid fatherUniqueIdentifier, String languageCode="")
                {
                    List<dtoMessageSettingsTranslation> translations = GetTranslationsForMessageItems(settings, subject, languageCode);
                    List<Recipient> recipients = GetRealRecipients(mRecipients);

                    List<dtoTranslatedMessage> messages = recipients.Select(r => r.IdLanguage).Distinct().ToList().Select(i => new dtoTranslatedMessage(translations.Where(t=>t.IdLanguage== i).FirstOrDefault())
                        {
                            Body = body,
                            Subject = subject,
                            Signature = translations.Where(t=>t.IdLanguage== i).FirstOrDefault().GetSignature(settings.Signature),
                            Recipients= recipients.Where(r=> r.IdLanguage==i).ToList(),
                            Attachments = attachments,
                            UniqueIdentifier = uniqueIdentifier,
                            FatherUniqueIdentifier= fatherUniqueIdentifier,
                            PlainBody=plainBody
                        }).ToList();
                    foreach (dtoTranslatedMessage m in messages)
                    {
                        String mbody = (settings.IsBodyHtml && replaceCRLF ? body.Replace("\r\n", "<br>") : body);
                        if (!String.IsNullOrEmpty(m.Signature))
                            mbody += ((settings.IsBodyHtml) ? "<br/><br/>" : "\r\n\r\n\r\n") + m.Signature;

                        if (!settings.IsBodyHtml)
                            m.Body = mbody.Replace("<br>", "\r\n");
                        else
                            m.Body = mbody;
                        m.PlainBody = (String.IsNullOrEmpty(m.PlainBody) ? m.PlainBody : m.PlainBody.Replace("<br>","\r\n").Replace("<br/>","\r\n"));
                    }
                 
                    return messages;
                }

                //private void GenerateMessage(MailMessage message, Int32 idDefaultLanguage, litePerson sender, Message dtoMessage, Boolean replaceCRLF = false)
                //{
                //    message.BodyEncoding = System.Text.Encoding.UTF8;
                //    message.SubjectEncoding = System.Text.Encoding.UTF8;
                //    MailAddress replyTo = (sender == null) ? null : new System.Net.Mail.MailAddress(sender.Mail, sender.SurnameAndName);
                //    message.Sender = GetSender(sender, dtoMessage, SmtpConfig);
                //    message.From = GetFrom(sender, dtoMessage, SmtpConfig);
                //    if (replyTo != null)
                //        message.ReplyToList.Add(replyTo);
                //    if (SmtpConfig != null && !SmtpConfig.RelayAllowed && !message.ReplyToList.Any())
                //        message.ReplyToList.Add(message.From);

                //    if (dtoMessage.Settings.NotifyToSender && message.ReplyToList.Any())
                //    {
                //        try
                //        {
                //            foreach (MailAddress r in message.ReplyToList)
                //            {
                //                if (!string.IsNullOrEmpty(r.DisplayName))
                //                    message.Headers.Add("Disposition-Notification-To", r.DisplayName + " <" + r.Address + ">");
                //                else
                //                    message.Headers.Add("Disposition-Notification-To", "<" + r.Address + ">");
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //        }
                //    }

                //    Int32 idUserLanguage = (sender != null) ? sender.LanguageID : idDefaultLanguage;
                //    message.IsBodyHtml = dtoMessage.IsBodyHtml;
                //    //if (message.IsBodyHtml)
                //    //    message.Subject =  System.Net.WebUtility.HtmlEncode(GetSubject(idUserLanguage,idDefaultLanguage,dtoMessage));
                //    //else
                //    message.Subject = GetSubject(idUserLanguage, idDefaultLanguage, dtoMessage);

                //    if (dtoMessage.Attachments != null && dtoMessage.Attachments.Count > 0)
                //        AddAttachments(message, dtoMessage.Attachments);
                //    String body = (dtoMessage.IsBodyHtml && replaceCRLF ? dtoMessage.Body.Replace("\r\n", "<br>") : dtoMessage.Body);
                //    body += ((message.IsBodyHtml) ? "<br/><br/>" : "\r\n\r\n\r\n") + GetSignature(idUserLanguage, idDefaultLanguage, dtoMessage.Settings.Signature);

                //    if (!message.IsBodyHtml)
                //        body = body.Replace("<br>", "\r\n");
                //    //else
                //    //    body = System.Net.WebUtility.HtmlEncode(body.Replace("<br>", "\r\n"));
                //    message.Body = body;
                //}
            #endregion

            #region "Attachments"
                public String GetAttachmentsFullPath(String basePath, String messagePath = "")
                {
                    String path = basePath;
                    if (!String.IsNullOrEmpty(messagePath))
                    {
                        if (messagePath.StartsWith(basePath))
                            path = messagePath;
                        else
                            path += messagePath;
                    }
                    return path;
                }
                public Int32 RemoveAttachments(List<String> attachments, String currentPath = "", Boolean alsoDirectory = true)
                {
                    Int32 results = 0;
                    if (attachments != null && attachments.Any())
                        results = attachments.Where(f => lm.Comol.Core.File.Delete.File(((String.IsNullOrEmpty(currentPath) || f.StartsWith(currentPath)) ? f : currentPath + f))).Count();
                    if (alsoDirectory && !String.IsNullOrEmpty(currentPath))
                        lm.Comol.Core.File.Delete.Directory(currentPath, false);
                    return 0;
                }

                /// <summary>
                /// Add attachments to MailMessage
                /// </summary>
                /// <param name="message"></param>
                /// <param name="attachments">list of files path</param>
                /// <param name="verify">verify if file exists</param>
                private void AddAttachments(MailMessage message, List<String> attachments, Boolean verifyAttachments = false)
                {
                    foreach (String filePath in attachments)
                    {
                        if (verifyAttachments && lm.Comol.Core.File.Exists.File(filePath))
                            message.Attachments.Add(new Attachment(filePath));
                        else if (!verifyAttachments)
                            message.Attachments.Add(new Attachment(filePath));
                    }
                }

                /// <summary>
                /// Verify if attachments exists
                /// </summary>
                /// <param name="attachmentPath"></param>
                /// <param name="attachments"></param>
                /// <returns></returns>
                private List<String> GetVerifiedAttachments(List<String> attachments, String attachmentPath)
                {
                    List<String> results = new List<string>();
                    foreach (String filePath in attachments)
                    {
                        if (lm.Comol.Core.File.Exists.File(filePath))
                            results.Add(filePath);
                        else if (lm.Comol.Core.File.Exists.File(attachmentPath + filePath))
                            results.Add(attachmentPath + filePath);
                    }
                    return results;
                }

                /// <summary>
                /// Verify mail attachments and remove attachments not found
                /// </summary>
                /// <param name="message"></param>
                /// <param name="attachmentPath"></param>
                private void ParseAttachments(Message message, String attachmentPath)
                {
                    message.Attachments = (message.Attachments!=null && message.Attachments.Any()) ? GetVerifiedAttachments(message.Attachments, attachmentPath) : message.Attachments;
                }
                private void ParseAttachments(Notification.Domain.dtoModuleNotificationMessage message, String attachmentPath)
                {
                    message.Attachments = (message.Attachments != null && message.Attachments.Any()) ? GetVerifiedAttachments(message.Attachments, attachmentPath) : message.Attachments;
                }
                private void ParseAttachments(Notification.Domain.GroupMessages group, String attachmentPath)
                {
                    group.Settings.Attachments = (group.Settings.Attachments!= null && group.Settings.Attachments.Any()) ? GetVerifiedAttachments(group.Settings.Attachments, attachmentPath) : group.Settings.Attachments;
                }
       
            #endregion

            #region "Settings"
                private SmtpClient CreateSmtpClient(SmtpServiceConfig config = null)
                {
                    if (config == null)
                        config = SmtpConfig;
                    SmtpClient smtpClient = new SmtpClient(config.Host, config.Port);
                    smtpClient.EnableSsl = config.Authentication.EnableSsl;
                    if (config.Authentication.Enabled)
                        smtpClient.Credentials = new System.Net.NetworkCredential(config.Authentication.AccountName, config.Authentication.AccountPassword);
                    return smtpClient;
                }
                #region "Language"
                    /// <summary>
                    /// Get translations for subject, signature
                    /// </summary>
                    /// <param name="dtoMessage"></param>
                    /// <returns></returns>
                    private List<dtoMessageSettingsTranslation> GetTranslationsForMessageItems(MessageSettings settings, String subject, String languageCode = "")
                    {
                        IList<Language> languages = Manager.GetAllLanguages();
                        Int32 idDefault = languages.Where(l=> l.isDefault).Select(l=> l.Id).FirstOrDefault();
                        List<dtoMessageSettingsTranslation> items = languages.Where(l=> String.IsNullOrEmpty(languageCode)|| l.Code==languageCode || l.isDefault).Select(l => new dtoMessageSettingsTranslation()
                        {
                            CodeLanguage = l.Code,
                            IdLanguage = l.Id,
                            IsDefault= l.isDefault,
                            NoReplySignature = GetSignature(l.Id,idDefault, Signature.FromNoReplySettings),
                            Signature = GetSignature(l.Id, idDefault, Signature.FromConfigurationSettings),
                            Subject = GetSubject(l.Id, idDefault, settings, subject)
                        }).ToList();
                        if (SmtpConfig!=null && SmtpConfig.DefaultSettings.Any(s=> s.IdLanguage==0))
                            items.AddRange(SmtpConfig.DefaultSettings.Where(s => s.IdLanguage == 0).Select(s => new dtoMessageSettingsTranslation()
                                {
                                    CodeLanguage = s.CodeLanguage,
                                    IdLanguage = s.IdLanguage,
                                    IsDefault = s.IsDefault,
                                    NoReplySignature = s.NoReplySignature,
                                    Signature = s.Signature,
                                    Subject = GetSubject(s, settings, subject)
                                }).ToList());

                        if (!items.Any(m => m.IsMulti))
                            items.AddRange(languages.Where(l=>l.isDefault).Select(l => new dtoMessageSettingsTranslation()
                            {
                                CodeLanguage = "multi",
                                IdLanguage = 0,
                                IsDefault = false,
                                NoReplySignature = GetSignature(l.Id, l.Id, Signature.FromNoReplySettings),
                                Signature = GetSignature(l.Id, l.Id, Signature.FromConfigurationSettings),
                                Subject = GetSubject(l.Id, l.Id, settings, subject)
                            }).ToList());
                        return items;
                    }

                    private String GetSubjectPrefix(Int32 idUserLanguage, Int32 idDefaultLanguage, MessageSettings settings)
                    {
                        return (settings.PrefixType == SubjectPrefixType.None || IstanceConfig == null) ? "" : IstanceConfig.SmtpConfig.GetSubjectPrefix(idUserLanguage, idDefaultLanguage);
                    }
                    private String GetSubjectPrefix(SenderSettings settings, MessageSettings messageSettings)
                    {
                        return (messageSettings.PrefixType == SubjectPrefixType.None || settings == null) ? "" : settings.SubjectPrefix;
                    }
                    private String GetSubject(SenderSettings settings, MessageSettings messageSettings, String subject)
                    {
                        return GetSubject(GetSubjectPrefix(settings, messageSettings), subject);
                    }
                    private String GetSubject(Int32 idUserLanguage, Int32 idDefaultLanguage, MessageSettings messageSettings, String subject)
                    {
                        return GetSubject(GetSubjectPrefix(idUserLanguage, idDefaultLanguage, messageSettings), subject);
                    }
                    private String GetSubject(String prefix, String subject)
                    {
                        if (!String.IsNullOrEmpty(prefix) && !String.IsNullOrEmpty(subject) && subject.StartsWith(prefix))
                            prefix = "";
                        return prefix + subject;
                    }
                    private String GetSubjectForCopy(Int32 idUserLanguage, Int32 idDefaultLanguage, MessageSettings messageSettings, String subject)
                    {
                        String prefix = (messageSettings.PrefixType == SubjectPrefixType.None || IstanceConfig == null) ? "" : IstanceConfig.SmtpConfig.GetSubjectForSenderCopy(idUserLanguage, idDefaultLanguage);
                        return GetSubjectPrefix(idUserLanguage, idDefaultLanguage, messageSettings) + prefix + subject;
                    }
                    private String GetSignature(Int32 idUserLanguage, Int32 idDefaultLanguage, Signature type)
                    {
                        if (SmtpConfig == null)
                            return "";
                        switch (type)
                        {
                            case Signature.FromNoReplySettings:
                                return SmtpConfig.GetNoReplySignature(idUserLanguage, idDefaultLanguage);
                            case Signature.FromConfigurationSettings:
                                return SmtpConfig.GetSignature(idUserLanguage, idDefaultLanguage);
                        }
                        return "";
                    }
                    private System.Net.Mail.MailAddress GetFrom(litePerson person, MessageSettings settings, SmtpServiceConfig config)
                    {
                        System.Net.Mail.MailAddress systemMail = null;
                        System.Net.Mail.MailAddress userMail = (person == null) ? null : new System.Net.Mail.MailAddress(person.Mail, person.SurnameAndName);
                        if (config != null && config.SystemSender != null && settings.SenderType == SenderUserType.System)
                            systemMail = new System.Net.Mail.MailAddress(config.SystemSender.Address, config.SystemSender.DisplayName);
                        if (systemMail != null)
                            return systemMail;
                        else if (userMail != null)
                            return userMail;
                        else
                            return null;
                    }
                    private System.Net.Mail.MailAddress GetSender(litePerson person, MessageSettings settings, SmtpServiceConfig config)
                    {
                        if (config != null && config.SystemSender != null)
                            return (config.RelayAllowed && settings.SenderType != SenderUserType.System && person != null && person.TypeID != (int)UserTypeStandard.Guest && person.TypeID != (int)UserTypeStandard.PublicUser) ? GetFrom(person, settings, config) : new System.Net.Mail.MailAddress(config.SystemSender.Address, config.SystemSender.DisplayName);
                        return null;
                    }
                #endregion
            #endregion

            #region "Recipients"
                private List<Recipient> GetRealRecipients(List<Recipient> mRecipients)
                {
                    List<Recipient> recipients = new List<Recipient>();
                    recipients.AddRange(mRecipients.Where(r => !String.IsNullOrEmpty(r.Address)).ToList());
                    ///recipients.AddRange(mRecipients.Where(r => !String.IsNullOrEmpty(r.Address) && r.IdUserModule>0).ToList());
                    /// find by idRole and IdCommunity
                    mRecipients.Where(r => r.IdRole > 0 && r.IdCommunity > 0 && (r.IdPerson<=0 || r.IdUserModule<=0)).ToList().ForEach(r => recipients.AddRange(GetMessageRecipients(r.IdRole, r.IdCommunity, r.Status, r.Type)));
                    /// find by status
                    mRecipients.Where(r => r.IdRole == 0 && r.IdCommunity > 0 && (r.IdPerson <= 0 || r.IdUserModule <= 0)).ToList().ForEach(r => recipients.AddRange(GetMessageRecipients(r.IdCommunity, r.Status, r.Type)));
                    if (mRecipients.Where(r => r.IdPerson > 0 && String.IsNullOrEmpty(r.Address)).Any())
                        recipients.AddRange(GetMessageRecipients(mRecipients.Where(r => r.IdPerson > 0 && String.IsNullOrEmpty(r.Address))));
                    return recipients;
                }
                private List<Recipient> GetMessageRecipients(Int32 idRole, Int32 idCommunity, RecipientStatus status, RecipientType type)
                {
                    List<Recipient> recipients = new List<Recipient>();
                    var query = (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.IdRole == idRole && s.IdCommunity == idCommunity select s);
                    switch (status)
                    {
                        case RecipientStatus.Available:
                            query = query.Where(s => s.Accepted && s.Enabled);
                            break;
                        case RecipientStatus.Blocked:
                            query = query.Where(s => s.Accepted && !s.Enabled);
                            break;
                        case RecipientStatus.Waiting:
                            query = query.Where(s => !s.Accepted && !s.Enabled);
                            break;
                    }
                    return GetMessageRecipients(query.Select(s => s.IdPerson).ToList(), status, type);
                }
                private List<Recipient> GetMessageRecipients(Int32 idCommunity, RecipientStatus status, RecipientType type)
                {
                    List<Recipient> recipients = new List<Recipient>();
                    var query = (from s in Manager.GetIQ<liteSubscriptionInfo>() where s.IdCommunity == idCommunity select s);
                    switch (status)
                    {
                        case RecipientStatus.Available:
                            query = query.Where(s => s.Accepted && s.Enabled);
                            break;
                        case RecipientStatus.Blocked:
                            query = query.Where(s => s.Accepted && !s.Enabled);
                            break;
                        case RecipientStatus.Waiting:
                            query = query.Where(s => !s.Accepted && !s.Enabled);
                            break;
                    }
                    return GetMessageRecipients(query.Select(s => s.IdPerson).ToList(), status, type);
                }
                private List<Recipient> GetMessageRecipients(IEnumerable<Recipient> items)
                {
                    List<Recipient> recipients = new List<Recipient>();
                    List<litePerson> persons = Manager.GetLitePersons(items.Select(i => i.IdPerson).Distinct().ToList());

                    recipients.AddRange(items.Where(i => persons.Any(p => p.Id == i.IdPerson)).Select(i => new Recipient() { Address = persons.Where(p => p.Id == i.IdPerson).FirstOrDefault().Mail, DisplayName = persons.Where(p => p.Id == i.IdPerson).FirstOrDefault().SurnameAndName, Status = i.Status, Type = i.Type }).ToList());
                    return recipients;
                }
                private List<Recipient> GetMessageRecipients(List<Int32> idPersons, RecipientStatus status, RecipientType type)
                {
                    return Manager.GetLitePersons(idPersons.Distinct().ToList()).Select(p => new Recipient() { Address = p.Mail, DisplayName = p.SurnameAndName, Status = status, Type = type, IdLanguage = p.LanguageID }).ToList();
                }
            #endregion
        #endregion

        #region "Save Methods"
            public lm.Comol.Core.Mail.Messages.MailMessage SaveMessage(Int32 idCommunity, String moduleCode, litePerson sender, Message dtoMessage, List<dtoTranslatedMessage> tMessages, String savedPath, lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = null, ModuleObject obj = null)
            {
                lm.Comol.Core.Mail.Messages.Ownership ownership = new lm.Comol.Core.Mail.Messages.Ownership();
                ownership.ModuleObject = obj;
                ownership.ModuleCode = moduleCode;
                ownership.IdModule = Manager.GetModuleID(moduleCode);
                ownership.IsPortal = (idCommunity == 0);
                if (idCommunity > 0)
                    ownership.Community = Manager.GetLiteCommunity(idCommunity);
                return SaveMessage(ownership,sender,dtoMessage, tMessages, savedPath,template);
            }
            public lm.Comol.Core.Mail.Messages.MailMessage SaveMessage(lm.Comol.Core.Mail.Messages.Ownership ownership, litePerson sender, Message dtoMessage, List<dtoTranslatedMessage> tMessages, String savedPath, lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = null)
            {
                lm.Comol.Core.Mail.Messages.MailMessage message = null;
                try
                {
                    Manager.BeginTransaction();
                    DateTime createdOn = DateTime.Now;
                    if (sender != null && sender.TypeID != (int)UserTypeStandard.Guest && sender.TypeID != (int)UserTypeStandard.PublicUser && tMessages != null && tMessages.Any() && tMessages.Where(m => m.Recipients.Any() || m.RemovedRecipients.Any()).Any())
                        message = AddMessage(sender, createdOn, ownership, dtoMessage.Settings, tMessages,dtoMessage.Attachments, dtoMessage.UniqueIdentifier, dtoMessage.FatherUniqueIdentifier, savedPath, AddTemplate(sender, createdOn, dtoMessage.Settings, template));
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    message = null;
                    Manager.RollBack();
                }
                return message;
            }
            public lm.Comol.Core.Mail.Messages.MailMessage SaveMessage(String moduleCode, litePerson sender, Notification.Domain.dtoModuleNotificationMessage nMessage, List<dtoTranslatedMessage> tMessages, String savedPath)
            {
                lm.Comol.Core.Mail.Messages.Ownership ownership = new lm.Comol.Core.Mail.Messages.Ownership();
                ownership.ModuleObject = nMessage.ObjectOwner;
                ownership.ModuleCode = moduleCode;
                ownership.IdModule = Manager.GetModuleID(moduleCode);
                ownership.IsPortal = (nMessage.IdCommunity == 0);
                if (nMessage.IdCommunity > 0)
                    ownership.Community = Manager.GetLiteCommunity(nMessage.IdCommunity);
                return SaveMessage(ownership, sender, nMessage, tMessages, savedPath);
            }
            public lm.Comol.Core.Mail.Messages.MailMessage SaveMessage(lm.Comol.Core.Mail.Messages.Ownership ownership, litePerson sender, Notification.Domain.dtoModuleNotificationMessage nMessage, List<dtoTranslatedMessage> tMessages, String savedPath)
            {
                lm.Comol.Core.Mail.Messages.MailMessage message = null;
                try
                {
                    Manager.BeginTransaction();
                    DateTime createdOn = DateTime.Now;
                    if (sender != null && sender.TypeID != (int)UserTypeStandard.Guest && sender.TypeID != (int)UserTypeStandard.PublicUser && tMessages != null && tMessages.Any() && tMessages.Where(m => m.Recipients.Any() || m.RemovedRecipients.Any()).Any())

                        message = AddMessage(sender, createdOn, ownership, nMessage.MailSettings, tMessages,nMessage.Attachments,nMessage.UniqueIdentifier, nMessage.FatherUniqueIdentifier, savedPath, AddTemplate(sender, createdOn, nMessage.MailSettings, GenerateTemplateFromNotification(nMessage)));
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    message = null;
                    Manager.RollBack();
                }
                return message;
            }
            private lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate GenerateTemplateFromNotification(Notification.Domain.dtoModuleNotificationMessage nMessage)
            {
                lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = new lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate(nMessage.MailSettings);
                //template.IdTemplate = nMessage.IdTemplate;
                //lm.Comol.Core.TemplateMessages.Domain.TemplateDefinitionVersion version = null;
                //if (nMessage.IdVersion > 0 || nMessage.IdTemplate > 0)
                //    version = Service.GetVersion(template.IdTemplate, View.IdSelectedVersion);
                //if (version != null)
                //{
                //    template.IdVersion = version.Id;
                //    template.IsTemplateCompliant = IsTemplateCompliant(translations, version);
                //}
                //else
                //    template.IsTemplateCompliant = false;
                //if (!template.IsTemplateCompliant)
                //{
                //    template.DefaultTranslation = (translations.Where(t => t.LanguageCode == "multi" && t.IdLanguage == 0).Any() ? translations.Where(t => t.LanguageCode == "multi" && t.IdLanguage == 0).FirstOrDefault().Translation.Copy() : null);
                //    template.Translations = translations.Where(t => !(t.LanguageCode == "multi" && t.IdLanguage == 0) && t.IsValid).Select(t => new lm.Comol.Core.Mail.Messages.dtoBaseMailTemplateContent()
                //    {
                //        IdLanguage = t.IdLanguage,
                //        LanguageCode = t.LanguageCode,
                //        LanguageName = t.LanguageName,
                //        Translation = t.Translation
                //    }).ToList();
                //}
                return template;
            }


            public lm.Comol.Core.Mail.Messages.MailMessage SaveMessage(String moduleCode, litePerson sender, Notification.Domain.GroupMessages group, List<dtoTranslatedMessage> tMessages, String savedPath)
            {
                lm.Comol.Core.Mail.Messages.Ownership ownership = new lm.Comol.Core.Mail.Messages.Ownership();
                ownership.ModuleObject = group.ObjectOwner;
                ownership.ModuleCode = moduleCode;
                ownership.IdModule = Manager.GetModuleID(moduleCode);
                ownership.IsPortal = (group.IdCommunity == 0);
                if (group.IdCommunity > 0)
                    ownership.Community = Manager.GetLiteCommunity(group.IdCommunity);
                return SaveMessage(ownership, sender, group, tMessages, savedPath);
            }
            private lm.Comol.Core.Mail.Messages.MailMessage SaveMessage(lm.Comol.Core.Mail.Messages.Ownership ownership, litePerson sender, Notification.Domain.GroupMessages group, List<dtoTranslatedMessage> tMessages, String savedPath)
            {
                lm.Comol.Core.Mail.Messages.MailMessage message = null;
                try
                {
                    Manager.BeginTransaction();
                    DateTime createdOn = DateTime.Now;
                    if (sender != null && sender.TypeID != (int)UserTypeStandard.Guest && sender.TypeID != (int)UserTypeStandard.PublicUser && tMessages != null && tMessages.Any() && tMessages.Where(m => m.Recipients.Any() || m.RemovedRecipients.Any()).Any())

                        message = AddMessage(sender, createdOn, ownership, group.Settings.Mail, tMessages,  group.Settings.Attachments,  group.Settings.UniqueIdentifier,  group.Settings.FatherUniqueIdentifier, savedPath, AddTemplate(sender, createdOn,  group.Settings.Mail, GenerateTemplateFromNotification(group)));
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    message = null;
                    Manager.RollBack();
                }
                return message;
            }
            private lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate GenerateTemplateFromNotification(Notification.Domain.GroupMessages group)
            {
                lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = new lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate(group.Settings.Mail);
                template.IdTemplate = group.Settings.Template.IdTemplate;
                template.IdVersion = group.Settings.Template.IdVersion;
                lm.Comol.Core.TemplateMessages.Domain.TemplateDefinitionVersion version = null;
                if (template.IdVersion > 0 || template.IdTemplate > 0)
                    version = TemplateService.GetVersion(template.IdTemplate, template.IdVersion);
                if (version != null)
                {
                    template.IdVersion = version.Id;
                    template.IsTemplateCompliant = group.Settings.Template.IsCompliant;
                }
                //else
                //    template.IsTemplateCompliant = false;
                if (group.Settings.Template.IsCompliant)
                {
                    template.DefaultTranslation = version.DefaultTranslation.Copy();
                    template.Translations = version.Translations.Where(t => !(t.LanguageCode == "multi" && t.IdLanguage == 0) && t.IsValid).Select(t => new lm.Comol.Core.Mail.Messages.dtoBaseMailTemplateContent()
                    {
                        IdLanguage = t.IdLanguage,
                        LanguageCode = t.LanguageCode,
                        LanguageName = t.LanguageName,
                        Translation = t.Translation
                    }).ToList();
                }
                else
                    template = null;
                return template;
            }

            //private Boolean IsTemplateCompliant(List<dtoTemplateTranslation> translations, TemplateDefinitionVersion version)
            //{
            //    Boolean isCompliant = true;
            //    foreach (dtoTemplateTranslation t in translations)
            //    {
            //        if (t.IdLanguage == 0 && t.LanguageCode == "multi")
            //        {
            //            isCompliant = t.IsCompliant(version.DefaultTranslation);
            //        }
            //        else
            //        {
            //            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content = version.GetTranslation(t.LanguageCode, t.IdLanguage);
            //            isCompliant = (content != null && t.IsCompliant(content));
            //        }
            //        if (!isCompliant)
            //            break;
            //    }

            //    return isCompliant;
            //}
            //public MailMessage SaveMessage(List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> tMessages, lm.Comol.Core.Mail.SmtpConfig smtpConfig, lm.Comol.Core.Mail.dtoMailSettings mailSettings, lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = null, ModuleObject obj = null, String moduleCode = "", Int32 idModule = 0, Int32 idCommunity = -1, Boolean isPortal = false)
            //{
            //    if (idCommunity == -1 && obj != null)
            //        idCommunity = obj.CommunityID;

            //    Ownership ownership = new Ownership();
            //    ownership.ModuleObject = obj;
            //    ownership.ModuleCode = (!String.IsNullOrEmpty(moduleCode)) ? moduleCode : (idModule > 0) ? Manager.GetModuleCode(idModule) : "";
            //    ownership.IdModule = (idModule > 0) ? idModule : (!String.IsNullOrEmpty(moduleCode) ? Manager.GetModuleID(moduleCode) : 0);
            //    ownership.IsPortal = isPortal || (idCommunity == 0);
            //    if (idCommunity > 0)
            //        ownership.Community = Manager.GetLiteCommunity(idCommunity);
            //    return SaveMessage(ownership, tMessages, smtpConfig, mailSettings, template);
            //}
            private lm.Comol.Core.Mail.Messages.MailTemplate AddTemplate(litePerson sender, DateTime createdOn,MessageSettings settings,  lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template)
            {
                Boolean inTransaction = Manager.IsInTransaction();
                lm.Comol.Core.Mail.Messages.MailTemplate item = null;
                try
                {
                    if (!inTransaction)
                        Manager.BeginTransaction();
                    if (sender != null && template!=null && sender.TypeID != (int)UserTypeStandard.Guest && sender.TypeID != (int)UserTypeStandard.PublicUser)
                    {
                        item = (from t in Manager.GetIQ<lm.Comol.Core.Mail.Messages.MailTemplate>()
                                where t.IdTemplate == template.IdTemplate && t.IdVersion == template.IdVersion && template.IsTemplateCompliant && t.IsTemplateCompliant
                                    && t.Deleted == BaseStatusDeleted.None
                                select t).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (item == null)
                        {
                            item = new lm.Comol.Core.Mail.Messages.MailTemplate();
                            item.CreatedBy = sender;
                            item.CreatedOn = createdOn;
                            if (template != null)
                            {
                                item.IdTemplate = template.IdTemplate;
                                item.IdVersion = template.IdVersion;
                                if (template.DefaultTranslation != null && !template.DefaultTranslation.IsEmpty())
                                    item.DefaultTranslation = template.DefaultTranslation.Copy();
                                item.IsTemplateCompliant = template.IsTemplateCompliant;
                                item.MailSettings = template.MailSettings;
                            }
                            else
                                item.MailSettings = settings;
                            if (template.DefaultTranslation.IsEmpty())
                                template.DefaultTranslation.IsHtml = item.MailSettings.IsBodyHtml;
                            Manager.SaveOrUpdate(item);
                            if (template != null && template.Translations != null && template.Translations.Any())
                            {
                                List<lm.Comol.Core.Mail.Messages.MailTemplateContent> contents = (from r in template.Translations
                                                                      where !r.IsEmpty
                                                                      select
                                                                          new lm.Comol.Core.Mail.Messages.MailTemplateContent()
                                                                          {
                                                                              IdLanguage = r.IdLanguage,
                                                                              LanguageCode = r.LanguageCode,
                                                                              LanguageName = r.LanguageName,
                                                                              Template = item,
                                                                              Translation = r.Translation.Copy(),
                                                                          }).ToList();
                                Manager.SaveOrUpdateList(contents);
                                item.Translations = contents;
                                Manager.SaveOrUpdate(item);
                            }
                        }
                    }
                    if (!inTransaction)
                        Manager.Commit();
                }
                catch (Exception ex)
                {
                    item = null;
                    if (!inTransaction)
                        Manager.RollBack();
                }
                return item;
            }
            private lm.Comol.Core.Mail.Messages.MailMessage AddMessage(litePerson sender, DateTime createdOn, lm.Comol.Core.Mail.Messages.Ownership ownership, MessageSettings settings, List<dtoTranslatedMessage> tMessages, List<String> mAttachments, System.Guid uniqueIdentifier, System.Guid fatherUniqueIdentifier, String savedPath, lm.Comol.Core.Mail.Messages.MailTemplate template = null)
            {
                lm.Comol.Core.Mail.Messages.MailMessage message = new lm.Comol.Core.Mail.Messages.MailMessage();
                message.UniqueIdentifier = uniqueIdentifier;
                message.FatherUniqueIdentifier = fatherUniqueIdentifier;
                message.CreatedOn = createdOn;
                message.CreatedBy = sender;
                message.MailSettings = settings;
                message.Ownership = ownership;
                message.Template = template;
                message.SentBySystem = (settings.SenderType == SenderUserType.System);
                Manager.SaveOrUpdate(message);
                Language language = Manager.GetDefaultLanguage();
                if (tMessages.Any())
                {
                    List<lm.Comol.Core.Mail.Messages.MessageTranslation> translations = tMessages.Where(t => t.Recipients.Any() || t.RemovedRecipients.Any()).Select(t => AddMessageTranslation((language != null) ? language.Id : 0, sender, createdOn, ownership, settings, t, message, template)).ToList();
                    if (translations.Any())
                    {
                        Manager.SaveOrUpdateList(translations);
                        message.Translations = translations;
                        Manager.SaveOrUpdate(message);
                    }
                }
                if (!String.IsNullOrEmpty(savedPath) && mAttachments !=null && mAttachments.Any())
                {
                    savedPath += message.Id.ToString() + "\\";
                    if (!lm.Comol.Core.File.Exists.Directory(savedPath))
                        lm.Comol.Core.File.Create.Directory(savedPath);
                    if (lm.Comol.Core.File.Exists.Directory(savedPath))
                    {
                        long displayOrder = 0;
                        List<lm.Comol.Core.Mail.Messages.MailAttachment> attachments = mAttachments.Where(a => lm.Comol.Core.File.Exists.File(a)).Select(a => CreateAttachment(ref displayOrder, ownership, message, a, savedPath)).ToList().Where(a => a != null).ToList();
                        if (attachments.Any())
                        {
                            Manager.SaveOrUpdateList(attachments.Where(a => a != null));
                            message.Attachments = attachments;
                            Manager.SaveOrUpdate(message);
                        }
                    }
                }

                return message;
            }
            private lm.Comol.Core.Mail.Messages.MessageTranslation AddMessageTranslation(Int32 idLanguage, litePerson sender, DateTime createdOn, lm.Comol.Core.Mail.Messages.Ownership ownership, MessageSettings mailSettings, dtoTranslatedMessage dtoMessage, lm.Comol.Core.Mail.Messages.MailMessage message, lm.Comol.Core.Mail.Messages.MailTemplate template)
            {
                lm.Comol.Core.Mail.Messages.MessageTranslation translation = new lm.Comol.Core.Mail.Messages.MessageTranslation();
                translation.Body = dtoMessage.Body;
                translation.CreatedOn = createdOn;
                translation.CreatedBy = sender;
                translation.IdLanguage = dtoMessage.Translation.IdLanguage;
                translation.LanguageCode = dtoMessage.Translation.CodeLanguage;
                translation.SentBySystem = message.SentBySystem;
                translation.Subject = dtoMessage.Subject;
                translation.Ownership = ownership;
                translation.Message = message;
                Manager.SaveOrUpdate(translation);
                List<lm.Comol.Core.Mail.Messages.MailRecipient> recipients = new List<lm.Comol.Core.Mail.Messages.MailRecipient>();
                if (dtoMessage.Recipients != null && dtoMessage.Recipients.Any())
                {
                    recipients = (from r in dtoMessage.Recipients
                                  select
                                      new lm.Comol.Core.Mail.Messages.MailRecipient()
                                      {
                                          DisplayName = r.DisplayName,
                                          IdPerson = r.IdPerson,
                                          IdUserModule = r.IdUserModule,
                                          IsMailSent = dtoMessage.Sent,
                                          IdRole = r.IdRole,
                                          MailAddress = r.Address,
                                          Message = message,
                                          Item = translation,
                                          Ownership = message.Ownership,
                                          IdLanguage = translation.IdLanguage,
                                          LanguageCode = translation.LanguageCode,
                                          Type = (RecipientType)((int)r.Type),
                                          IdModuleObject = r.IdModuleObject,
                                          IdModuleType = r.IdModuleType
                                      }).ToList();
                }
                if (dtoMessage.RemovedRecipients != null && dtoMessage.RemovedRecipients.Any())
                {
                    recipients.AddRange((from r in dtoMessage.RemovedRecipients
                                         select
                                             new lm.Comol.Core.Mail.Messages.MailRecipient()
                                             {
                                                 DisplayName = r.DisplayName,
                                                 IdPerson = r.IdPerson,
                                                 IdUserModule = r.IdUserModule,
                                                 IsMailSent = false,
                                                 MailAddress = r.Address,
                                                 IdRole= r.IdRole,
                                                 Message = message,
                                                 Item = translation,
                                                 Ownership = message.Ownership,
                                                 IdLanguage = translation.IdLanguage,
                                                 LanguageCode = translation.LanguageCode,
                                                 Type = (RecipientType)((int)r.Type),
                                                 IdModuleObject = r.IdModuleObject,
                                                 IdModuleType = r.IdModuleType
                                             }).ToList());
                }
                if (recipients.Count > 0)
                {
                    Manager.SaveOrUpdateList(recipients);
                    translation.Recipients = recipients;
                    Manager.SaveOrUpdate(translation);
                }
                return translation;
            }

            private lm.Comol.Core.Mail.Messages.MailAttachment CreateAttachment(ref long displayOrder, lm.Comol.Core.Mail.Messages.Ownership ownership,lm.Comol.Core.Mail.Messages.MailMessage message, String sourceFile, String destinationPath)
            {
                lm.Comol.Core.Mail.Messages.MailAttachment attachment = new lm.Comol.Core.Mail.Messages.MailAttachment();
                attachment.Message = message;

                lm.Comol.Core.File.dtoFileSystemInfo file = lm.Comol.Core.File.ContentOf.File_dtoInfo(sourceFile);
                if (file != null)
                {
                    if (lm.Comol.Core.File.Create.CopyFile(sourceFile, destinationPath + file.Name))
                    {
                        attachment.DirectFullname = destinationPath + file.Name;
                        attachment.DirectFilename = file.Name;
                        attachment.Type = Core.Mail.Messages.MailAttachmentType.DirectFolder;
                        lm.Comol.Core.File.Delete.File(sourceFile);
                    }
                }
                else
                    return null;
                attachment.Deleted = BaseStatusDeleted.None;
            
                Manager.SaveOrUpdate(attachment);
                displayOrder++;
           
                return attachment;
            }

            //private lm.Comol.Core.Mail.dtoMailSettings ConvertToDtoMailSettings(Message dtoMessage)
            //{
            //    lm.Comol.Core.Mail.dtoMailSettings item = new lm.Comol.Core.Mail.dtoMailSettings();
            //    item.CopyToSender = dtoMessage.Settings.NotifyToSender;
            //    item.isHtml = dtoMessage.Settings.IsBodyHtml;
            //    item.NotifyToSender = dtoMessage.Settings.NotifyToSender;
            //    item.Sender = (Core.Mail.SenderType)((int)dtoMessage.Settings.SenderType);
            //    item.SignatureType = (Core.Mail.SignatureType)((int)dtoMessage.Settings.Signature);
            //    item.Subject = (Core.Mail.SubjectType)((int)dtoMessage.Settings.PrefixType);
            //    return item;
            //}
        #endregion
            
        public void Dispose()
        {
            manager = null;
            Session.Dispose();
        }
    }
}