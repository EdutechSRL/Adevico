using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public enum ProjectOrderBy
    {
        None = 0,
        Community = 1,
        Project = 2,
        AllActive = 3,
        AllCompleted = 4,
        AllFuture = 5
    }
}
