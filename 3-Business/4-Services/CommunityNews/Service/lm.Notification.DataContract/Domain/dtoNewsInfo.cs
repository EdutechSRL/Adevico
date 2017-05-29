using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class dtoNewsInfo{
        [DataMember]
        public System.Guid UniqueID { get; set; }

        [DataMember]
        public DateTime Day { get; set; }

        [DataMember]
        public int  CommunityID { get; set; }
        
        [DataMember]
        public int ModuleID { get; set; }

        [DataMember]
        public DateTime SentDate { get; set; }

        public dtoNewsInfo() { }

    }
}