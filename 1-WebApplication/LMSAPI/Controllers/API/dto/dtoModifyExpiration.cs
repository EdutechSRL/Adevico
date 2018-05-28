using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSAPI.Controllers.API.dto
{
    public class dtoModifyExpiration
    {
        /// <summary>
        /// Id comnuità
        /// </summary>
        public int communityId { get; set; }

        /// <summary>
        /// Elenco id utenti da modificare
        /// </summary>
        public IList<int> usersId { get; set; }

        /// <summary>
        /// Validità scadenza (in giorni)
        /// </summary>
        public int validity { get; set; }

        /// <summary>
        /// Estendi validità: se TRUE aggiunge, se FALSE sovrascrive
        /// </summary>
        public bool extendValidity { get; set; }

        /// <summary>
        /// Modalità inizio
        /// </summary>
        public Adevico.APIconnection.Core.ExpirationStartBehaviour startBehaviour { get; set; }
    }



}