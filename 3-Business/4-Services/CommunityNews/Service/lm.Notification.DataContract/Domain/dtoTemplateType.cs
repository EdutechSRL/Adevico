using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract] 
    [Serializable]
    public enum dtoTemplateType:int 
    {
        [EnumMember]
        UnknownType = 0,

        [EnumMember]
        SummaryNotification = 1,

        [EnumMember]
        SingleNotification = 2,
    }
}
