using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class NotificationToRole : NotificationToCommunity
    {
        [DataMember]
        public List<int> RolesID { get; set; }


           public NotificationToRole()
        {
            this.RolesID = new List<int>();
            this.ValueParameters= new List<string>();
            this.NotificatedObjects= new List<dtoNotificatedObject>();
        }
           public NotificationToRole(System.Guid Id)
        {
            this.UniqueID = Id;
            this.RolesID = new List<int>();
            this.ValueParameters = new List<string>();
            this.NotificatedObjects = new List<dtoNotificatedObject>();
        }
    }
}