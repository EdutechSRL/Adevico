using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.ErrorsNotification.DataContract.Domain
{
    [DataContract]
    public enum  PersistTo
    {
        [EnumMember]
        Database=1,
        [EnumMember]
        Mail=2,
        [EnumMember]
        File = 3
    }
}
