using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Comol.Core.Msmq.Services.Mail.DataContract.Domain
{
    [DataContract(Name = "RecipientType")]
    public enum RecipientType
    {
        [EnumMember]
        To = 1,
        [EnumMember]
        CC = 2,
        [EnumMember]
        BCC = 3
    }
}
