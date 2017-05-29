using lm.Comol.Core.MailCommons.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Msmq.Services.Mail.Service
{
    [Serializable]
    public class dtoMessageSettingsTranslation : IEquatable<dtoMessageSettingsTranslation>
    {
        public virtual Int32 IdLanguage { get; set; }
        public virtual String CodeLanguage { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsMulti { get { return IdLanguage == 0 && CodeLanguage == "multi"; } }
        public String Subject { get; set; }
        //public String SubjectForCopy { get; set; }
        public String Signature { get; set; }
        public String NoReplySignature { get; set; }

        public String GetSignature(Signature type) {
            switch (type)
            {
                case MailCommons.Domain.Signature.FromConfigurationSettings:
                    return Signature;
                case  MailCommons.Domain.Signature.FromNoReplySettings:
                    return NoReplySignature;
                default:
                    return "";
            }
        }

        public bool Equals(dtoMessageSettingsTranslation other)
        {
            return (IdLanguage == other.IdLanguage && CodeLanguage == other.CodeLanguage && Subject == other.Subject && Signature == other.Signature && NoReplySignature == other.NoReplySignature);
        }
    }
}
