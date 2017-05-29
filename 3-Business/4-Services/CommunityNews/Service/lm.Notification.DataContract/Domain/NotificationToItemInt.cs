using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class NotificationToItemInt : NotificationToPermission 
    {
        [DataMember]
        public int ItemID { get; set; }

        [DataMember]
        public int ObjectTypeID { get; set; }

           public NotificationToItemInt()
        {
            this.ValueParameters= new List<string>();
            this.NotificatedObjects= new List<dtoNotificatedObject>();
        }
           public NotificationToItemInt(System.Guid uniqueId)
        {
            this.UniqueID = uniqueId;
            this.ValueParameters = new List<string>();
            this.NotificatedObjects = new List<dtoNotificatedObject>();
        }
    }
}