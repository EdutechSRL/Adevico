using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace lm.Comol.Modules.TaskList.Domain
{

    [Serializable, CLSCompliant(true)] 
    public enum TaskListType
    {
        None = -999,
        Personal = 0,
        PersonalCommunity = 1,
        Community = 2,
    }
}
