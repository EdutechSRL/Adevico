using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.Mooc.Domain
{
    /// <summary>
    /// Info sullo stato dei singoli percorsi di una comunità.
    /// </summary>
    public class dtoMoocInfoResponse : Adevico.APIconnection.Core.dto.dtoBaseResponse
    {
        /// <summary>
        /// Elenco moduli (PF) di tipo mooc con i relativi dati:
        /// Tipo, completamento corrente, completamento minimo, id, nome, etc...
        /// </summary>
        public IList<lm.Comol.Modules.EduPath.Domain.DTO.dtoCokadeInfo> Moocs { get; set; }
        /// <summary>
        /// SE a livello di comunità la coccarda è stata abilitata.
        /// </summary>
        public bool IsCommunityCokade { get; set; }
        /// <summary>
        /// SE sono stati superati i criteri per avere la coccarda
        /// </summary>
        public bool CokadeCompleted { get; set; }
        /// <summary>
        /// SE sono stati superati i criteri per avere la coccarda d'oro
        /// </summary>
        public bool CokadeCompletedGold { get; set; }
    }
}
