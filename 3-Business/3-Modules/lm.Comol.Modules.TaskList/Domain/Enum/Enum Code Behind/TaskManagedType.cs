using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
    public enum TaskManagedType
    {
        None = 0,
        Projects = 1,
        Tasks = 2
    }
}
