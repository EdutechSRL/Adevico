using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class NotificationToItemVersionLong : NotificationToPermission
    {
        [DataMember]
        public int IdItem { get; set; }
        [DataMember]
        public int IdVersion { get; set; }

        [DataMember]
        public int ObjectTypeID { get; set; }

        public NotificationToItemVersionLong()
        {
            ValueParameters= new List<string>();
            NotificatedObjects= new List<dtoNotificatedObject>();
        }
        public NotificationToItemVersionLong(System.Guid uniqueId)
        {
            UniqueID = uniqueId;
            ValueParameters = new List<string>();
            NotificatedObjects = new List<dtoNotificatedObject>();
        }
    }
}