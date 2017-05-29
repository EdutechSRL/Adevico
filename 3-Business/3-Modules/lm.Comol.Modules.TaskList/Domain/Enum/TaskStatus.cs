using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
   public enum TaskStatus  
    {
       suspended=1,
       started=2,
       completed=3,
       notStarted=4
    
    }
}
