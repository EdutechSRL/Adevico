using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class dtoNotificatedObject
    {
        [DataMember]
        public int ModuleID { get; set; }
       
        [DataMember]
        public string ModuleCode {get;set;}

        [DataMember]
        public int ObjectTypeID {get;set;}

        [DataMember]
        public string ObjectID { get; set; }

        [DataMember]
        public string FullyQualiFiedName { get; set; }

       

        public dtoNotificatedObject() { }
        public dtoNotificatedObject(lm.Notification.Core.Domain.NotificatedObject obj)
        {
            this.ModuleID = obj.ModuleID;
            this.ModuleCode = obj.ModuleCode;
            this.ObjectTypeID = obj.ObjectTypeID;
            this.FullyQualiFiedName = obj.FullyQualiFiedName;
            this.ObjectID = obj.ObjectID;
        }

    }
}
