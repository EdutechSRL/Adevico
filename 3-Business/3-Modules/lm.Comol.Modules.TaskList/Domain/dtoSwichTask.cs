using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
    public class dtoSwichTask
    {
        public lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum Permission { get; set; }
        public long TaskID { get; set; }
     
        public String TaskName { get; set; }
        public bool isLast { get; set; }
        public bool isFirst { get; set; }
        public TaskStatus Status { get; set; }
      
        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }
        public DateTime? Deadline { get; set; }
    
        public String TaskWBS { get; set; }
        public int Level { get; set; }

        public dtoSwichTask(Task oTask, bool isFirst, bool isLast, lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum Permission)
       {
           TaskID = oTask.ID;
           TaskName = oTask.Name;
           this.isFirst = isFirst;
           this.isLast = isLast;
           //StartDate = oTask.StartDate;
           //EndDate = oTask.EndDate;
           Deadline = oTask.Deadline;
           this.Status = oTask.Status; 
          
           Level = oTask.Level;
           this.Permission = Permission;
           if (oTask.Level == 0)
           {
               this.TaskWBS = "";
           }
           else
           {
               this.TaskWBS = oTask.TaskWBSstring + oTask.TaskWBSindex;
           } 
           
       }
        public dtoSwichTask(Task oTask,bool isFirst,bool isLast)
        {
            TaskID = oTask.ID;
            TaskName = oTask.Name;       
            Deadline = oTask.Deadline;
            this.Status = oTask.Status; 
       
            Level = oTask.Level;
            this.isFirst = isFirst;
            this.isLast = isLast;
            if (oTask.Level == 0)
            {
                this.TaskWBS = "";
            }
            else
            {
                this.TaskWBS = oTask.TaskWBSstring + oTask.TaskWBSindex;
            } 

        }
              
        public dtoSwichTask()
       {
           TaskID = -1;
           TaskWBS = "";
           TaskName = "";
           this.isLast = false;
           this.isFirst = false;
           Level = -1;
           //StartDate = DateTime.Now;
           //EndDate = DateTime.Now.AddDays(1);
           Deadline = DateTime.Now.AddDays(1);
           Permission = lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum.None;
           //Status = 0; 
         
       }

         

    }


}
