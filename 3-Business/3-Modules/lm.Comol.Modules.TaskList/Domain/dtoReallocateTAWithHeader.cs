using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
    public class dtoReallocateTAWithHeader
    {
        public long TaskID { get; set; }
        public String TaskName { get; set; }
        public List<dtoReallocateTA> TaskAssignments { get; set; }

        public dtoReallocateTAWithHeader()
        {
            this.TaskID = 0;
            this.TaskName = "";
            this.TaskAssignments = new List<dtoReallocateTA>();
        
        }

        public dtoReallocateTAWithHeader(Task oTask, List<dtoReallocateTA> TaskAssignments)
        {
            this.TaskID = oTask.ID;
            if (oTask.Level != 0)
            {
                this.TaskName = oTask.TaskWBSstring + oTask.TaskWBSstring + " " + oTask.Name;
            }
            else
            {
                TaskName = oTask.Name;
            }
            
            this.TaskAssignments = TaskAssignments;

        }
        public dtoReallocateTAWithHeader(Task oTask, IList<dtoReallocateTA> TaskAssignments)
        {
            this.TaskID = oTask.ID;
            if (oTask.Level != 0)
            {
                this.TaskName = oTask.TaskWBSstring + oTask.TaskWBSstring + " " + oTask.Name;
            }
            else
            {
                TaskName = oTask.Name;
            }

            this.TaskAssignments = TaskAssignments.ToList();

        }

    }
}
