using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.ErrorsNotification.DataContract.Domain
{
    [DataContract]
    
    public enum ErrorType
    {
        [EnumMember]
        CommunityModuleError=1,
        [EnumMember]
        DBerror=2,
        [EnumMember]
        GenericError = 3,
        [EnumMember]
        GenericModuleError = 4,
        [EnumMember]
        GenericWebError = 5,
        [EnumMember]
        FileError = 6
    }
}
