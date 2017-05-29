using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WCF_StandardModules.InstantMessenger.Domain
{
    [DataContract]
    public class IMDomainObject
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}