using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Notification.Core.Domain
{
    [Serializable]
    public class PersonNotification
    {
        //public virtual List<NotificationMessage> Notifications { get; set; }
        public virtual System.Guid NotificationUniqueID { get; set; }
        public virtual System.Guid ID { get; set; }
        public virtual int PersonID { get; set; }
        public virtual Boolean isViewed { get; set; }
        public virtual Boolean isDeleted { get; set; }
        public virtual DateTime Day { get; set; }
        public virtual DateTime SentDate { get; set; }
        public virtual int CommunityID { get; set; }
        public PersonNotification(){
            this.isViewed = false;
            this.isDeleted = false;
        }

    }
}
