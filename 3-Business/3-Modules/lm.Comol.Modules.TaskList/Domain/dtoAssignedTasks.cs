using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;
 

namespace lm.Comol.Modules.TaskList.Domain 
{   
    [CLSCompliant(true)]
    public class dtoAssignedTasks
    {
        public long TaskId { get; set; }
        public string TaskName { get; set; }
        public long ProjectID { get; set; }
        public string ProjectName { get; set; }
        public TaskStatus Status { get; set; }
        public Boolean isDeleted { get; set; }
        public int Completeness { get; set; }
        public DateTime? Deadline { get; set; }
        public TaskPermissionEnum Permissions { get; set; }
        
        //public dtoAssignedTasks(Task oTask, TaskRole oRole)
        public dtoAssignedTasks(Task oTask, TaskPermissionEnum oTaskPermissions )
        {
            TaskId = oTask.ID;
            TaskName = oTask.Name;

            if (oTask.Project != null)
            {
                ProjectID = oTask.Project.ID;
                ProjectName = oTask.Project.Name;
            }
            else
            {
                ProjectID = oTask.ID ;
                ProjectName = oTask.Name;
            }

            Permissions = oTaskPermissions;
            Status = oTask.Status;
            isDeleted = oTask.MetaInfo.isDeleted;
            Completeness = oTask.Completeness;
            Deadline = oTask.Deadline;
        }

        public dtoAssignedTasks(dtoTaskWithPortalComm  oTask, TaskPermissionEnum oTaskPermissions)
        {
            TaskId = oTask.ID;
            TaskName = oTask.Name;
            
            ProjectID = oTask.ProjectID;
            ProjectName = oTask.ProjectName;            

            Permissions = oTaskPermissions;
            Status = oTask.Status;
            isDeleted = oTask.MetaInfo.isDeleted;
            Completeness = oTask.Completeness;
            Deadline = oTask.Deadline;
        }

        public dtoAssignedTasks() 
        { 
        
        }
}

   

}
//Sub New(ByVal task As Task)
//            'Me.Task = task
//            TaskID = task.ID
//            TaskName = task.Name
//            If IsNothing(task.Project) Then
//                ProjectID = 0
//                ProjectName = TaskName
//            Else
//                ProjectID = task.Project.ID
//                ProjectName = task.Project.Name
//            End If
//            Status = task.Status
//            Completeness = task.Completeness
//            Deadline = task.Deadline
//            If IsNothing(task.Deadline) Then
//                Deadline = Date.Now
//            End If
//            isDeleted = task.MetaInfo.isDeleted

//        End Sub