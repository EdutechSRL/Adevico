using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.ErrorsNotification.DataContract.Domain
{
    [Serializable]
    public class ErrorSettings
    {
        public long Id { get; set; }
        public String ComolUniqueID { get; set; }
        public String Name { get; set; }
        public String HostSMTP { get; set; }
        public String SenderMail { get; set; }
        public String SenderName { get; set; }
        public String RealMailSender { get; set; }
        public String ReplyTo { get; set; }
        public String RecipientMail { get; set; }
        public Boolean UseAuthentication { get; set; }
        public String AccountName { get; set; }
        public String AccountPassword { get; set; }
        public Int32 Port { get; set; }
        public Boolean UseSsl { get; set; }
        public int NotifyAfterErrors { get; set; }
        public int NotificationDelay { get; set; }
    }
}