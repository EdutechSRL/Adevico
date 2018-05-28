using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.ScormStat.Domain
{
    public class dtoFile
    {
        /// <summary>
        /// Id File
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Nome del file
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Estensione del file
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// Percorso nel repository - ToDo: al momento nome comepleto!
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Numero di play totali - ToDo: al momento numero download!
        /// </summary>
        public long TotalPlay { get; set; }
        /// <summary>
        /// Numero download - per generalizzare
        /// </summary>
        public long TotalDownload { get; set; }
        /// <summary>
        /// Ultima modifica (versione)
        /// </summary>
        public string LastUpdate { get; set; }

        /// <summary>
        /// ID ultima versione
        /// </summary>
        public Int64 LastVersionId { get; set; }
    }
}
