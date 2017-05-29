using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.ErrorsNotification.DataContract.Domain
{
    [Serializable]
    public class MailNotificationSettings
    {
        public long Id { get; set; }
        public String ComolUniqueID { get; set; }
        public DateTime? NotifyedOn { get; set; }
        public int ErrorsCount { get; set; }
    }
}