using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class NotificationToCommunity
    {
        [DataMember]
        public System.Guid UniqueID { get; set; }

        [DataMember]
        public int ModuleID {get;set;}
       
        [DataMember]
        public string ModuleCode {get;set;}

        [DataMember]
        public int CommunityID {get;set;}

        [DataMember]
        public int ActionID {get;set;}

        [DataMember]
        public List<string> ValueParameters {get;set;}

        [DataMember]
        public List<dtoNotificatedObject> NotificatedObjects { get; set; }

        //[DataMember]
        //public string ObjectID { get; set; }

        //[DataMember]
        //public string ObjectFullyQualifiedName { get; set; }

        [DataMember]
        public DateTime SentDate { get; set; }
        

        public NotificationToCommunity()
        {
            this.ValueParameters= new List<string>();
            this.NotificatedObjects= new List<dtoNotificatedObject>();
        }
        public NotificationToCommunity(System.Guid Id)
        {
            this.UniqueID = Id;
            this.ValueParameters = new List<string>();
            this.NotificatedObjects = new List<dtoNotificatedObject>();
        }

    }
}
