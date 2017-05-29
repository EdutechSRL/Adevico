using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class NotificationToItem<T> : NotificationToPermission
    {
        [DataMember]
        public T ItemID { get; set; }

        [DataMember]
        public int ObjectTypeID { get; set; }

    }
}