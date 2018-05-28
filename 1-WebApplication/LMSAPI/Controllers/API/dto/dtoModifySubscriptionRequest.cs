using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSAPI.Controllers.API.dto
{
    /// <summary>
    /// Richiesta modifica iscrizione
    /// </summary>
    public class dtoModifySubscriptionRequest
    {
        /// <summary>
        /// Id Comunità
        /// </summary>
        public int CommunityId { get; set; }
        /// <summary>
        /// Elenco id utenti
        /// </summary>
        public IList<int> UsersId { get; set; }
        /// <summary>
        /// Azione:
        ///     none = 0,
        ///     Enable = 1,
        ///     Disable = 2,
        ///     Delete = 3
        ///</summary>
        public Adevico.APIconnection.Core.SubscriptionAction Action { get; set; }
    }
}