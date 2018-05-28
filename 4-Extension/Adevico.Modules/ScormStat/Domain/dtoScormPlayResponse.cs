using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.ScormStat.Domain
{
    public class dtoScormPlayResponse : Adevico.APIconnection.Core.dto.dtoBaseResponse
    {
        /// <summary>
        /// Elenco con i dati dei play dell'utente.
        /// </summary>
        public IList<dtoScormStatPlay> Plays { get; set; }
    }
}
