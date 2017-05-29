using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
     [CLSCompliant(true)]
    public class TaskCategory  
    {
        public virtual int ID { get; set; }
        public virtual String Name { get; set; }
        public virtual bool isDeleted { get; set; }
    }
}
 