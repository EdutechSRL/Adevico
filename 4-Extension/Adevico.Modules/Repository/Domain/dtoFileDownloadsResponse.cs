using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.Repository.Domain
{
    /// <summary>
    /// Info sullo stato dei singoli percorsi di una comunità.
    /// </summary>
    public class dtoFileDownloadsResponse : Adevico.APIconnection.Core.dto.dtoBaseResponse
    {
        public IList<FileRepoDownloads> Downloads { get; set; }
        
    }
}
