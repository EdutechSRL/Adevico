using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
    public class dtoTaskGantt
    {
        public lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum Permission { get; set; }
        public long TaskID { get; set; }
        public String TaskName { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public double StartDate { get; set; }
        public double EndDate { get; set; }
        public double Deadline { get; set; }
        public int TaskCompleteness { get; set; }
        public String TaskWBS { get; set; }
        public int Level { get; set; }


        public dtoTaskGantt(Task oTask, lm.Comol.Modules.TaskList.Domain.TaskPermissionEnum Permission)
       {
           TaskID = oTask.ID;
           TaskName = oTask.Name;
           
           Priority = oTask.Priority;
           Status = oTask.Status;
           StartDate = ((DateTime)oTask.StartDate).ToOADate();
           EndDate = ((DateTime)oTask.EndDate).ToOADate();
           Deadline = ((DateTime)oTask.Deadline).ToOADate();          
           TaskCompleteness = oTask.Completeness;
           Level = oTask.Level;
           this.Permission = Permission;
        
           if (oTask.Level == 0)
           {
               TaskWBS = "";
           }
        
           
               TaskWBS = oTask.TaskWBSstring + oTask.TaskWBSindex; 
           
       }

         public dtoTaskGantt(dtoTaskMap oDtoTaskMap)
         {
             TaskID = oDtoTaskMap.TaskID;
             TaskWBS = oDtoTaskMap.TaskWBS;
             TaskName = oDtoTaskMap.TaskName;
             TaskCompleteness = oDtoTaskMap.TaskCompleteness;
             Level = oDtoTaskMap.Level;
             StartDate = oDtoTaskMap.StartDate.ToOADate();
             EndDate = oDtoTaskMap.EndDate.ToOADate();
             Deadline = oDtoTaskMap.Deadline.ToOADate();
             Permission = oDtoTaskMap.Permission;
            
         }


         public dtoTaskGantt()
       {
           TaskID = -1;
           TaskWBS = "";
           TaskName = "";
           TaskCompleteness = -1;
           Level = -1;
           StartDate = 0;
           EndDate = 0;
           Deadline = 0;
       
       }


    }





}
