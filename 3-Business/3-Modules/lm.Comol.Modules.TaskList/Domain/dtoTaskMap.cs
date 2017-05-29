using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
    public class dtoTaskMap
    {
        public lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum Permission { get; set; }
       public long TaskID { get; set; }
       public String TaskName { get; set; }
       public TaskPriority Priority { get; set; }
       public TaskStatus Status { get; set; }
       public DateTime StartDate { get; set; }
       public DateTime EndDate { get; set; }
       public DateTime Deadline { get; set; }     
       public int TaskCompleteness { get; set; }     
       public String TaskWBS { get; set; }
       public int Level { get; set; }
       public bool isDeleted { get; set; }

       public dtoTaskMap(Task oTask, lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum Permission)
       {
           TaskID = oTask.ID;
           TaskName = oTask.Name;
           
           Priority = oTask.Priority;
           Status = oTask.Status;        
            StartDate =(DateTime )oTask.StartDate;

           if (oTask.EndDate.HasValue)
               EndDate = (DateTime)oTask.EndDate;
            
           Deadline = (oTask.Deadline.HasValue) ? oTask.Deadline.Value : Deadline;
           TaskCompleteness = oTask.Completeness;
           Level = oTask.Level;
           this.Permission = Permission;
           isDeleted = oTask.MetaInfo.isDeleted;
           if (oTask.Level == 0)
           {
               TaskWBS = "";
           }
           else if (isDeleted)
           {
               TaskWBS = oTask.TaskWBSstring + "x"; 
           }
           else
           {
               TaskWBS = oTask.TaskWBSstring + oTask.TaskWBSindex; 
           }
       }




       public dtoTaskMap()
       {
           TaskID = -1;
           TaskWBS = "";
           TaskName = "";
           TaskCompleteness = -1;
           Level = -1;
           StartDate = DateTime.Now;
           EndDate = DateTime.Now.AddDays(1);
           Deadline = DateTime.Now.AddDays(1);
           Permission = lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum.None;
           isDeleted = false;
       }
    }
}
