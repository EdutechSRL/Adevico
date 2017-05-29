using lm.Comol.Core.MailCommons.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Msmq.Services.Mail.Service
{
    [Serializable]
    public class dtoTranslatedMessage
    {
        public virtual Guid UniqueIdentifier { get; set; }
        public virtual Guid FatherUniqueIdentifier { get; set; }
        public virtual dtoMessageSettingsTranslation Translation { get; set; }
        public virtual List<Recipient> Recipients { get; set; }
        public virtual List<Recipient> RemovedRecipients { get; set; }
        public virtual String Subject { get; set; }
        public virtual String Body { get; set; }
        public virtual String PlainBody { get; set; }
        public virtual String Signature { get; set; }
        public virtual List<String> Attachments { get; set; }
        public virtual Boolean Sent { get; set; }
        public virtual MailExceptionType Exception { get; set; }
        public virtual Boolean HasException { get { return Sent == false || Exception != MailExceptionType.None; } }

        public dtoTranslatedMessage(dtoMessageSettingsTranslation translation)
        {
            Recipients = new List<Recipient>();
            RemovedRecipients = new List<Recipient>();
            Translation = translation;
            Attachments = new List<String>();
            Exception = MailExceptionType.None;
        }
        public dtoTranslatedMessage(dtoMessageSettingsTranslation translation,Recipient recipient)
        {
            Recipients = new List<Recipient>();
            RemovedRecipients = new List<Recipient>();
            Recipients.Add(recipient);
            Translation = translation;
            Attachments = new List<String>();
            Exception = MailExceptionType.None;
        }
    }
}
