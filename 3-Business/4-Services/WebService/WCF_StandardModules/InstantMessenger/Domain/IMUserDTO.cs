using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace WCF_StandardModules.InstantMessenger.Domain
{
    [DataContract]
    public class IMUserDTO:IMDomainObject
    {
        /// <summary>
        /// Id utente
        /// </summary>
        [DataMember]
        public Int32 PersonId { get; set; }

        /// <summary>
        /// Nome visualizzato
        /// </summary>
        [DataMember]
        public String DisplayName { get; set; }

        /// <summary>
        /// Ultimo accesso alla chat
        /// </summary>
        [DataMember]
        public DateTime LastAccess { get; set; }

        /// <summary>
        /// Se è attivo
        /// </summary>
        [DataMember]
        public Boolean IsActive { get; set; }

        /// <summary>
        /// Indica se l'utente è entrato nella chat
        /// </summary>
        [DataMember]
        public Boolean IsEnter { get; set; }

        //[DataMember]
        //public Boolean IsChatvisible { get; set; }

        //[DataMember]
        //public Guid CurrentChat { get; set; }
    }
}
