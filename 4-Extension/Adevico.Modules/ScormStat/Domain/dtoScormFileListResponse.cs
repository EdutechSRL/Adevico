using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.ScormStat.Domain
{
    public class dtoScormFileListResponse : Adevico.APIconnection.Core.dto.dtoBaseResponse
    {
        public IList<dtoFile> Files { get; set; }
    }
}
