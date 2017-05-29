using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class NotificationToPermission : NotificationToCommunity 
    {
        [DataMember]
        public int Permission { get; set; }

         public NotificationToPermission()
        {
            this.ValueParameters= new List<string>();
            this.NotificatedObjects= new List<dtoNotificatedObject>();
        }
         public NotificationToPermission(System.Guid Id)
        {
            this.UniqueID = Id;
            this.ValueParameters = new List<string>();
            this.NotificatedObjects = new List<dtoNotificatedObject>();
        }
    }
}
