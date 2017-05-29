using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace lm.Comol.Modules.TaskList.Domain
    {

    //[ CLSCompliant(true)]
    public enum TaskListOrder:int       
         
    {
        None,
		Task,
		Project,
		Status,
		Completeness,
        Deadline
		
    }
}

