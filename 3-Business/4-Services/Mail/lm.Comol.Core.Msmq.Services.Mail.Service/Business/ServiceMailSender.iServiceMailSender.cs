using lm.Comol.Core.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Diagnostics;
using lm.Comol.Core.Msmq.Services.Mail.DataContract.Service;
using lm.Comol.Core.Msmq.Services.Mail.Service.Config;
using NHibernate;
using lm.Comol.Core.Msmq.Services.Mail.Service.Business;
using lm.Comol.Core.MailCommons.Domain.Configurations;
using lm.Comol.Core.MailCommons.Domain.Messages;

namespace lm.Comol.Core.Msmq.Services.Mail.Service
{
    // NOTE: If you change the class name "ErrorsNotificationService" here, you must also update the reference to "ErrorsNotificationService" in App.config.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public partial class ServiceMailSender : iServiceMailSender, IDisposable
    {
        private static ServiceConfig config = null;
        private ServiceConfig Config
        {
            get{
                if (config==null)
                    config = lm.Comol.Core.WinService.Configurations.Configuration<ServiceConfig>.Load(System.Configuration.ConfigurationManager.AppSettings["ServicePath"]);
                return config;
            }
            set{
                config = value;
            }
        }


        private SmtpServiceConfig GetSmtpSettings(string identifier)
        {
            if (Config==null)
                config = lm.Comol.Core.WinService.Configurations.Configuration<ServiceConfig>.Load(System.Configuration.ConfigurationManager.AppSettings["ServicePath"]);
            return (config == null || config.Istances == null || String.IsNullOrEmpty(identifier)) ? null : config.Istances.Where(i => !String.IsNullOrEmpty(i.UniqueIdentifier) && i.UniqueIdentifier.ToLower() == identifier.ToLower()).Select(i => i.SmtpConfig).FirstOrDefault();
        }

       

        public void SendMailMessage(string istanceIdentifier, int idUser, int idCommunity, string moduleCode, Message message, string attachmentPath = "", bool saveMessage = false, string attachmentSavedPath = "")
        {
            InternalSendMail(istanceIdentifier, idUser, idCommunity, moduleCode, GetSmtpSettings(istanceIdentifier), message, attachmentPath, saveMessage, attachmentSavedPath);
        }
        public void SendMailWithSettings(string istanceIdentifier, int idUser, int idCommunity, string moduleCode, SmtpServiceConfig config, Message message, string attachmentPath = "", bool saveMessage = false, string attachmentSavedPath = "")
        {
            InternalSendMail(istanceIdentifier, idUser, idCommunity, moduleCode, config,message,  attachmentPath, saveMessage, attachmentSavedPath);
        }

        private void InternalSendMail(string istanceIdentifier, int idUser, int idCommunity, string moduleCode, SmtpServiceConfig cfg, Message message, string attachmentPath = "", bool saveMessage = false, string attachmentSavedPath = "")
        {
            IstanceConfig istance = Config.GetIstanceConfiguration(istanceIdentifier);
            if (cfg != null && istance != null)
            {
                using (ISession session = lm.Comol.Core.Data.SessionDispatcher.NewSession(istance.ConnectionString))
                {
                    if (session != null)
                    {
                        using (InternalMailService service = new InternalMailService(cfg,istance, session))
                        {
                            Boolean result = false;
                            String messagePath = service.GetAttachmentsFullPath(istance.AttachmentUploadPath, attachmentPath);
                            try
                            {
                                result = service.SendMail(idUser, idCommunity,moduleCode, message, (message.Settings.IsBodyHtml), messagePath, saveMessage, attachmentSavedPath);
                            }
                            catch (Exception ex)
                            {
                               ErrorHandler.addMessageToPoisonQueue(ex, message);
                            }    
                        }
                    }
                    else
                        ErrorHandler.addMessageToPoisonQueue(message);
                }
            }
        }

        public void SendMailFromModuleNotification(string istanceIdentifier, int idUser, string moduleCode, Notification.Domain.dtoModuleNotificationMessage message)
        {
            IstanceConfig istance = Config.GetIstanceConfiguration(istanceIdentifier);
            SmtpServiceConfig cfg = GetSmtpSettings(istanceIdentifier);
            if (cfg != null && istance != null)
            {
                using (ISession session = lm.Comol.Core.Data.SessionDispatcher.NewSession(istance.ConnectionString))
                {
                    if (session != null)
                    {
                        using (InternalMailService service = new InternalMailService(cfg, istance, session))
                        {
                            Boolean result = false;
                            String messagePath = service.GetAttachmentsFullPath(istance.AttachmentUploadPath, message.AttachmentPath);
                            try
                            {
                                result = service.SendMail(idUser, moduleCode, message, (message.MailSettings.IsBodyHtml),messagePath, message.AttachmentSavedPath);
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler.addMessageToPoisonQueue(ex, message);
                            }
                        }
                    }
                    else
                        ErrorHandler.addMessageToPoisonQueue(message);
                }
            }
        }

        public void SendMailFromGroupNotification(string istanceIdentifier, int idUser, string moduleCode, Notification.Domain.GroupMessages group)
        {
            IstanceConfig istance = Config.GetIstanceConfiguration(istanceIdentifier);
            SmtpServiceConfig cfg = GetSmtpSettings(istanceIdentifier);
            if (cfg != null && istance != null)
            {
                using (ISession session = lm.Comol.Core.Data.SessionDispatcher.NewSession(istance.ConnectionString))
                {
                    if (session != null)
                    {
                        using (InternalMailService service = new InternalMailService(cfg, istance, session))
                        {
                            Boolean result = false;
                            String messagePath = service.GetAttachmentsFullPath(istance.AttachmentUploadPath, group.Settings.AttachmentPath);
                            try
                            {
                                result = service.SendMail(idUser, moduleCode, group, (group.Settings.Mail.IsBodyHtml), messagePath, group.Settings.AttachmentSavedPath);
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler.addMessageToPoisonQueue(ex, group);
                            }
                        }
                    }
                    else
                        ErrorHandler.addMessageToPoisonQueue(group);
                }
            }
        }


        public void SendMailMessagesFromTemplate(string istanceIdentifier, int idUser, string moduleCode, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mailSettings, List<Core.Mail.Messages.dtoMailTranslatedMessage> messages, Core.Mail.Messages.dtoBaseMailTemplate template = null, DomainModel.ModuleObject obj = null, int idCommunity = -1, bool isPortal = false, List<string> attachments = null, string attachmentPath = "", bool saveMessage = false)
        {
            
        }

        public void Dispose()
        {

        }



      
    }
}