using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
   [CLSCompliant(true),Serializable]
    public class dtoTaskDetail
    {
        public long TaskID { get; set; }
        public String TaskName { get; set; }
        public long TaskAssignmentID { get; set; }
        public String ProjectName { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public string CommunityName { get; set; }
        public String Description { get; set; }
        public String Category { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Deadline { get; set; }
        public int PersonalCompleteness { get; set; } //a-1 se non sono Resource
        public int TaskCompleteness { get; set; }
        public string Notes{ get; set;}
        public String TaskWBS { get; set; }
        public bool isDeleted { get; set; }

        public dtoTaskDetail() 
        {
            TaskID = -1;
            TaskWBS = "";
            TaskName = "";
            TaskAssignmentID = -1;
            ProjectName = "";
            CommunityName = "";
            TaskCompleteness = 0;
            PersonalCompleteness = -1;
            Description = "";
            Notes = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);
            Deadline = DateTime.Now.AddDays(1);
            isDeleted = true;
        }
        public dtoTaskDetail(TaskAssignment oTaskAssignment) 
        {
            TaskName = oTaskAssignment.Task.Name;
           
            TaskAssignmentID = oTaskAssignment.ID;
            TaskID = oTaskAssignment.Task.ID;
            if (oTaskAssignment.Task.Level != 0)
            {
                ProjectName = oTaskAssignment.Project.Name;
                TaskWBS = oTaskAssignment.Task.TaskWBSstring + oTaskAssignment.Task.TaskWBSindex;
            }
            else
            {
                ProjectName = oTaskAssignment.Task.Name;
                TaskWBS = ""; 
            }
            
            Priority = oTaskAssignment.Task.Priority;
            Status = oTaskAssignment.Task.Status;
            if (oTaskAssignment.Task.Community != null)
            {
                CommunityName = oTaskAssignment.Task.Community.Name;
            }
            else
            {
                CommunityName = "";
            }
            Description=oTaskAssignment.Task.Description;
            Category = "";
            StartDate = oTaskAssignment.Task.StartDate;
            EndDate = oTaskAssignment.Task.EndDate;
            Deadline = oTaskAssignment.Task.Deadline;
            if (oTaskAssignment.TaskRole == TaskRole.Resource || oTaskAssignment.TaskRole == TaskRole.Customized_Resource)
            {
                PersonalCompleteness = oTaskAssignment.Completeness;   
            }
            else
            {
                PersonalCompleteness = -1; 
            }
            
            TaskCompleteness = oTaskAssignment.Task.Completeness;
            Notes = oTaskAssignment.Task.Notes;
            isDeleted = oTaskAssignment.Task.MetaInfo.isDeleted;
        }

        public dtoTaskDetail(Task oTask)
        {
            TaskID = oTask.ID;
            TaskName = oTask.Name;
          
            TaskAssignmentID = -1;
            if (oTask.Level != 0)
            {
                ProjectName = oTask.Project.Name;
                TaskWBS = oTask.TaskWBSstring + oTask.TaskWBSindex;
            }
            else
            {
                ProjectName = oTask.Name;
                TaskWBS = "";
            }
            Priority = oTask.Priority;
            Status = oTask.Status;
            if (oTask.Community != null)
            {
                CommunityName = oTask.Community.Name;
            }
            else
            {
                CommunityName = "Portal";
            }
            Description = oTask.Description;
            if (oTask.Category == null)
                Category = "";
            else
                Category = oTask.Category.Name;
            StartDate = oTask.StartDate;
            EndDate = oTask.EndDate;
            Deadline = oTask.Deadline;
            PersonalCompleteness = -1;
            TaskCompleteness = oTask.Completeness;
            Notes = oTask.Notes;
            
            if (oTask.MetaInfo == null)
            {
                isDeleted = false;
            }
            else
            { 
                isDeleted = oTask.MetaInfo.isDeleted; 
            }
            
        }
    }
}
