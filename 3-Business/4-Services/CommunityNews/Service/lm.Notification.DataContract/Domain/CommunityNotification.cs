using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class CommunityNews
    {
        [DataMember]
        public System.Guid UniqueID { get; set; }

        [DataMember]
        public int CommunityID { get; set; }

        [DataMember]
        public DateTime SentDate { get; set; }

        [DataMember]
        public DateTime Day { get; set; }
        
        [DataMember]
        public String Message { get; set; }

        [DataMember]
        public int ModuleID { get; set; }

        public CommunityNews() { }

        public CommunityNews(System.Guid uniqueID, int communityID, DateTime sentDate, DateTime day, String message,int moduleID) {
            this.UniqueID = uniqueID;
            this.CommunityID = communityID;
            this.SentDate = sentDate;
            this.Day = day;
            this.Message = message;
            this.ModuleID = moduleID;
        }
    }
}