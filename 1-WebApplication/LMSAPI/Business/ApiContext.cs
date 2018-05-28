using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSAPI.Business
{
    /// <summary>
    /// Altre variabili, come link Id, ModuleId, Module code, etc...
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public class ApiContext
    {
        public long CurrentLinkId { get; set; }
    }
}