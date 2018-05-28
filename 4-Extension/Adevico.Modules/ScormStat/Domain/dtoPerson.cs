using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.ScormStat.Domain
{
    public class dtoPerson
    {
        /// <summary>
        /// Id persona
        /// </summary>
        Int32 Id { get; set; }
        /// <summary>
        /// Nome persona
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Cognome persona
        /// </summary>
        string Surname { get; set; }
        /// <summary>
        /// Mail
        /// </summary>
        string Mail { get; set; }
        /// <summary>
        /// Ruolo nella comunità
        /// </summary>
        string Role { get; set; }

        /// <summary>
        /// Codice fiscale
        /// </summary>
        /// <remarks>
        /// SE il richiedente non ha permessi per la visualizzazione,
        /// il campo sarà vuoto
        /// </remarks>
        string TaxCode { get; set; }
    }
}
