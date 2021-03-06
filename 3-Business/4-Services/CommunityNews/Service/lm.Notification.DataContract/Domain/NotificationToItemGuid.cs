﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class NotificationToItemGuid : NotificationToPermission
    {
        [DataMember]
        public System.Guid ItemID { get; set; }

        [DataMember]
        public int ObjectTypeID { get; set; }

         public NotificationToItemGuid()
        {
            this.ValueParameters= new List<string>();
            this.NotificatedObjects= new List<dtoNotificatedObject>();
        }
         public NotificationToItemGuid(System.Guid uniqueId)
        {
            this.UniqueID = uniqueId;
            this.ValueParameters = new List<string>();
            this.NotificatedObjects = new List<dtoNotificatedObject>();
        }
    }
}