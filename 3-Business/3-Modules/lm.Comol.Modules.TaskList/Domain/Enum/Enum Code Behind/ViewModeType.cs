using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public enum ViewModeType
    {
        None = 0,
        TodayTasks = 1,
        InvolvingProjects = 2,
        TasksManagement = 3,
        TaskAdmin = 4,
        TaskMap = 5
    }
}
