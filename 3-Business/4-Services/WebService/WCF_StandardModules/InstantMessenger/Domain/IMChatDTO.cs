using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace WCF_StandardModules.InstantMessenger.Domain
{
    [DataContract]
    public class IMChatDTO:IMDomainObject
    {
        /// <summary>
        /// Identificatore di una chat
        /// </summary>
        //[DataMember]
        //public Guid Id { get; set; }

        /// <summary>
        /// Utente 1 della chat
        /// </summary>
        [DataMember]
        public IMUserDTO UserA { get; set; }

        /// <summary>
        /// Utente 2 della chat
        /// </summary>
        [DataMember]
        public IMUserDTO UserB { get; set; }

        /// <summary>
        /// Indica se i due utenti hanno cominciato la chat.
        /// </summary>
        /// <remarks>
        /// Stati chat a seconda dei parametri:
        /// 
        ///                     IsStarted	IsActive	UserDiscarded	
        /// Avvio chat	        0	        1	        Ricevente	
        /// Ricevente rifuita	0	        0	        Ricevente	
        /// Ricevente accetta	1	        1	        0	
        /// Utente chiude	    1	        0	        Utente chiude	
        /// Altro ut nn chiude	1	        0	        Utente chiude	
        /// Altro utente chiude	//	        //	        //	            Elimino Chat
        /// </remarks>
        [DataMember]
        public Boolean IsStarted { get; set; }
        /// <summary>
        /// Indica se una chat è ancora attiva.
        /// </summary>
        [DataMember]
        public Boolean IsActive { get; set; }
        /// <summary>
        /// Utente che non è attivo nella chat.
        /// </summary>
        [DataMember]
        public Int32 UserDiscarded { get; set; }


        /// <summary>
        /// Utente che non è attivo nella chat.
        /// </summary>
        [DataMember]
        public Int32 MessagesCount { get; set; }
    }
}