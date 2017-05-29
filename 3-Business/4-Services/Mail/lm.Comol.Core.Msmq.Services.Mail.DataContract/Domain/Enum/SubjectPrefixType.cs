using System;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Msmq.Services.Mail.DataContract.Domain
{
    [DataContract(Name = "SubjectPrefixType"), Serializable]
    public enum SubjectPrefixType
    {
        [EnumMember]
        SystemConfiguration = 0,
        [EnumMember]
        None = 1
    }
}