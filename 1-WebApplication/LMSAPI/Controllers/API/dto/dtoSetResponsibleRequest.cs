using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSAPI.Controllers.API.dto
{
    /// <summary>
    /// Richiesta modifica responsabile
    /// </summary>
    public class dtoSetResponsibleRequest
    {
        /// <summary>
        /// Id Comunità
        /// </summary>
        public int CommunityId { get; set; }
        /// <summary>
        /// Id nuovo responsabile
        /// </summary>
        public int UserId { get; set; }
    }
}