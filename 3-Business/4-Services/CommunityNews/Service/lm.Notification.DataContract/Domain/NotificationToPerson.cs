using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class NotificationToPerson : NotificationToCommunity
        {
        [DataMember]
        public List<int> PersonsID { get; set; }

        public NotificationToPerson()
        {
            this.PersonsID = new List<int>();
            this.ValueParameters= new List<string>();
            this.NotificatedObjects= new List<dtoNotificatedObject>();
        }
        public NotificationToPerson(System.Guid Id)
        {
            this.UniqueID = Id;
            this.PersonsID = new List<int>();
            this.ValueParameters = new List<string>();
            this.NotificatedObjects = new List<dtoNotificatedObject>();
        }
    }
}