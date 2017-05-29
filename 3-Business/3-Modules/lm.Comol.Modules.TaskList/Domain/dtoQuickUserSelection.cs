using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
    public class dtoQuickUserSelection
        
    {
        public lm.Comol.Modules.TaskList.Domain.dtoTaskMap dtoSwitch { get; set; }
        public String QuickUserList{ get; set; }

        public dtoQuickUserSelection() { }

        public dtoQuickUserSelection(lm.Comol.Modules.TaskList.Domain.dtoTaskMap oDtoTaskMap, String oString)
        {
            this.dtoSwitch = oDtoTaskMap;
            this.QuickUserList = oString; 
        }

       
    }
}
