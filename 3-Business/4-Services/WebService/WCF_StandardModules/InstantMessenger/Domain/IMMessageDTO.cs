using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace WCF_StandardModules.InstantMessenger.Domain
{
    [DataContract]
    public class IMMessageDTO
    {
        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public String MessageText { get; set; }

        [DataMember]
        public Guid SenderId { get; set; }

        [DataMember]
        public Int32 SenderPersonId { get; set; }
    }
}
