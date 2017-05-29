using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public enum TaskRole
    {
        None = 0,
        ProjectOwner = 1,
        Manager =2,
        Resource =3,
        Customized_Resource = 4, 
        Visitor =5,
        
        /*TaskSetCategory = 8,
        TaskSetPriority = 16,
        TaskSetEndDate = 32,
        TaskSetStartDate = 64,
        TaskSetStatus = 128,
        TaskSetCompleteness = 256,
        TaskView = 512,
        TaskUpdate = 1024,
        TaskDelete = 2048*/
    }
}