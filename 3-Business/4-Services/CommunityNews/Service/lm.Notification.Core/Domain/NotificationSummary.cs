using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Notification.Core.Domain
{
    [Serializable]
    public class NotificationSummary: NotificationBase 
    {
        public virtual int TotalMessages { get; set; }
        public virtual int PersonID { get; set; }
    }
}
