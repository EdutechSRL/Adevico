using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant (true)]
    public class dtoTaskWithPortalComm
    {
        public virtual long ID { get; set; }
        //public virtual int TaskWBSindex { get; set; }
        //public virtual String TaskWBSstring { get; set; }
        public virtual String Name { get; set; }
        //public virtual String Description { get; set; }
        //public virtual String Notes { get; set; }
        public virtual String CommunityName { get; set; }
        public virtual int CommunityId { get; set; }
        public virtual MetaData MetaInfo { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual int Level { get; set; }
        public virtual TaskPriority Priority { get; set; }
        public virtual TaskStatus Status { get; set; }
        public virtual int Completeness { get; set; }
        public virtual TaskCategory Category { get; set; }
        public virtual Task TaskParent { get; set; }
        public virtual long ProjectID { get; set; }
        public virtual String ProjectName { get; set; }
        //public virtual Task Project { get; set; }
        //public virtual IList<Task> TaskChildren { get; set; }
        //public virtual IList<BaseFile> TaskFiles { get; set; }
        //public virtual bool isArchived { get; set; }
        //public virtual bool isPersonal { get; set; }
        //public virtual bool isPortal { get; set; }
        //public virtual String WBS
        //{
        //    get
        //    {
        //        return TaskWBSstring + TaskWBSindex;
        //    }
        //}

        public dtoTaskWithPortalComm() { }

        public dtoTaskWithPortalComm(Task oTask, String portalName) 
            {   
                
                this.ID=oTask.ID;
                this.Name = oTask.Name;
                this.MetaInfo = oTask.MetaInfo;
                this.Completeness = oTask.Completeness;
                this.StartDate = oTask.StartDate;
                this.EndDate = oTask.EndDate;
                this.Deadline = oTask.Deadline;
                this.Level = oTask.Level;
                this.Priority = oTask.Priority;
                this.Status = oTask.Status;
                this.Category = oTask.Category;
            
                //this.Project = oTask.Project;
                this.TaskParent = oTask.TaskParent;

                if (oTask.Project != null)
                {
                    ProjectID = oTask.Project.ID;
                    ProjectName = oTask.Project.Name;
                }
                else
                {
                    ProjectID = oTask.ID;
                    ProjectName = oTask.Name;
                }

            if (oTask.Community != null && oTask.Community.Id>0)
            {
                this.CommunityName = oTask.Community.Name;
                this.CommunityId = oTask.Community.Id;
            }
            else
            {
                this.CommunityName = portalName;
                this.CommunityId = 0;
            }
            }

        public dtoTaskWithPortalComm(TaskAssignment oTaskAss, String portalName)
        {

            this.ID = oTaskAss.Task.ID;
            this.Name = oTaskAss.Task.Name;
            this.MetaInfo = oTaskAss.Task.MetaInfo;
            this.Completeness = oTaskAss.Task.Completeness;
            this.StartDate = oTaskAss.Task.StartDate;
            this.EndDate = oTaskAss.Task.EndDate;
            this.Deadline = oTaskAss.Task.Deadline;
            this.Level = oTaskAss.Task.Level;
            this.Priority = oTaskAss.Task.Priority;
            this.Status = oTaskAss.Task.Status;
            this.Category = oTaskAss.Task.Category;

            //this.Project = oTask.Project;
            this.TaskParent = oTaskAss.Task.TaskParent;

            if (oTaskAss.Project != null)
            {
                ProjectID = oTaskAss.Project.ID;
                ProjectName = oTaskAss.Project.Name;
            }
            else
            {
                ProjectID = oTaskAss.Task.ID;
                ProjectName = oTaskAss.Task.Name;
            }

            if (oTaskAss.Task.Community != null && oTaskAss.Task.Community.Id > 0)
            {
                this.CommunityName = oTaskAss.Task.Community.Name;
                this.CommunityId = oTaskAss.Task.Community.Id;
            }
            else
            {
                this.CommunityName = portalName;
                this.CommunityId = 0;
            }
        }
    }
}