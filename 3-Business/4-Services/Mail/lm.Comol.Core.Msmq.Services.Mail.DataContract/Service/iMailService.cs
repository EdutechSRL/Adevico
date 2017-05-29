using System;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using lm.Comol.Core.MailCommons.Domain.Messages;
using lm.Comol.Core.MailCommons.Domain.Configurations;

namespace lm.Comol.Core.Msmq.Services.Mail.DataContract.Service
{
    [ServiceContract]
    public interface iServiceMailSender
    {
        /// <summary>
        /// Send direct mail
        /// </summary>
        /// <param name="istanceIdentifier">Istance unique ID</param>
        /// <param name="idUser">Sender user</param>
        /// <param name="moduleCode">Sender module code</param>
        /// <param name="message">message</param>
        /// <param name="attachmentPath">Attachment path (used only if !="" otherwise system uses istance configuration settings)</param>
        /// <param name="saveMessage">Message sent MUST be saved ?</param>
        /// <param name="attachmentSavedPath">If not found in configuration where attachments must be saved IF mail MUST be saved !</param>
        [OperationContract(IsOneWay = true)]
        void SendMailMessage(String istanceIdentifier, Int32 idUser,Int32 idCommunity, String moduleCode, Message message, String attachmentPath = "", Boolean saveMessage = false, String attachmentSavedPath = "");

        [OperationContract(IsOneWay = true)]
        void SendMailWithSettings(String istanceIdentifier, Int32 idUser, Int32 idCommunity, String moduleCode, SmtpServiceConfig config, Message message, String attachmentPath = "", Boolean saveMessage = false, String attachmentSavedPath = "");


        //[OperationContract(IsOneWay = true)]
        //void SendDirectMailMessage(Int32 idUser, String moduleCode, String subject, String body, SenderType sndType = SenderType.System, SubjectType sType = SubjectType.System, Boolean isHtml = true, SignatureType signature = SignatureType.FromConfigurationSettings, Boolean notifyToSender = false, Boolean copyToSender = false, List<String> attachments = null, String attachmentPath = "", Boolean saveMessage = false);

        [OperationContract(IsOneWay = true)]
        void SendMailFromModuleNotification(string istanceIdentifier, Int32 idUser, String moduleCode, lm.Comol.Core.Notification.Domain.dtoModuleNotificationMessage message);

        [OperationContract(IsOneWay = true)]
        void SendMailFromGroupNotification(string istanceIdentifier, Int32 idUser, String moduleCode, lm.Comol.Core.Notification.Domain.GroupMessages message);


        [OperationContract(IsOneWay = true)]
        void SendMailMessagesFromTemplate(string istanceIdentifier, Int32 idUser, String moduleCode, MailCommons.Domain.Messages.MessageSettings mailSettings, List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> messages, lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = null, lm.Comol.Core.DomainModel.ModuleObject obj = null, Int32 idCommunity = -1, Boolean isPortal = false, List<String> attachments = null, String attachmentPath = "", Boolean saveMessage = false);
    }
}