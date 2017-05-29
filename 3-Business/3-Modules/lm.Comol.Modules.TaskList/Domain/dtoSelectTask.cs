using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
     public class dtoSelectTask
    {
        public lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum Permission { get; set; }
        public String TaskName { get; set; }
        public String TaskWBS { get; set; }
        public int Level { get; set; }
        public long TaskID { get; set; }


        public dtoSelectTask()
        {
            this.TaskID = 0;
            this.TaskName = "";
            this.TaskWBS = "";
            this.Level = -1;
            this.Permission = lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum.None;
        }

        public dtoSelectTask(Task oTask, lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum Permission)
        {
            this.TaskID = oTask.ID;
            this.TaskName = oTask.Name;
            if (oTask.Level == 0)
            {
                this.TaskWBS = "";
            }
            else
            {
                this.TaskWBS = oTask.TaskWBSstring + oTask.TaskWBSindex; 
            }            
            this.Level = oTask.Level;
            this.Permission = Permission;
        }
    }
}
