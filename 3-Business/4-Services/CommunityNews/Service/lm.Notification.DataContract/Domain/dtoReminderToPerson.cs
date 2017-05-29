using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class dtoCommunityWithNews
    {
        [DataMember]
        public virtual System.Guid ID { get; set; }
        [DataMember]
        public virtual int CommunityID { get; set; }
        [DataMember]
        public virtual int PersonID { get; set; }
        [DataMember]
        public virtual long ActionCount { get; set; }
        [DataMember]
        public virtual DateTime LastUpdate { get; set; }
        [DataMember]
        public virtual DateTime LastUserRead { get; set; }

        public dtoCommunityWithNews() { }
        public dtoCommunityWithNews(lm.Notification.Core.Domain.CommunityNewsSummary r) {
            this.ActionCount = r.ActionCount;
            this.CommunityID = r.CommunityID;
            this.PersonID = r.PersonID;
            this.ID = r.ID;
            this.LastUpdate = r.LastUpdate;
            this.LastUserRead = r.LastUserRead.GetValueOrDefault(new DateTime());
        }
    }
}
