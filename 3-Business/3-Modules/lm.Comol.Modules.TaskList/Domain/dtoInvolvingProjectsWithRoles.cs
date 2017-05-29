 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;
 
namespace lm.Comol.Modules.TaskList.Domain
{   
    [CLSCompliant(true)]
    public class dtoInvolvingProjectsWithRoles
    {
        private dtoTaskWithPortalComm tg;
        private TaskPermissionEnum taskPermissionEnum;
        private IList<TaskRole> iList;

        public long TaskId { get; set; }
        public string TaskName { get; set; }
        public long ProjectID { get; set; }
        public string ProjectName { get; set; }
        public TaskStatus Status { get; set; }
        public Boolean isDeleted { get; set; }
        public int Completeness { get; set; }
        public DateTime? Deadline { get; set; }
        public TaskPermissionEnum Permissions { get; set; }
        public IList<TaskRole> Roles { get; set; }

        //public dtoAssignedTasks(Task oTask, TaskRole oRole)
        public dtoInvolvingProjectsWithRoles(Task oTask, TaskPermissionEnum oTaskPermissions, IList<TaskRole> oRoles )
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
                Roles = oRoles;
            }

        public dtoInvolvingProjectsWithRoles() 
            { 
        
            }

        public dtoInvolvingProjectsWithRoles(dtoTaskWithPortalComm oTask, TaskPermissionEnum oTaskPermissions, IList<TaskRole> oRoles)
            {
                // TODO: Complete member initialization
                TaskId = oTask.ID;
                TaskName = oTask.Name;

                ProjectID = oTask.ProjectID;
                ProjectName = oTask.ProjectName;

                Permissions = oTaskPermissions;
                Status = oTask.Status;
                isDeleted = oTask.MetaInfo.isDeleted;
                Completeness = oTask.Completeness;
                Deadline = oTask.Deadline;
                Roles = oRoles;
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