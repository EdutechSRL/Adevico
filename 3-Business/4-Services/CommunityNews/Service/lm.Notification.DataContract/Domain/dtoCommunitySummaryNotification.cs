using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class dtoCommunitySummaryNotification
    {
        [DataMember]
        public virtual int ID { get; set; }

        [DataMember]
        public virtual int ModuleID { get; set; }

        [DataMember]
        public virtual DateTime Day { get; set; }
        
        [DataMember]
        public virtual int Count { get; set; }
    }
}