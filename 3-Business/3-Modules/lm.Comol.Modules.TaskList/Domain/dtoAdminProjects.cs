using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    
    public class dtoAdminProjects
    {
        public long ProjectID { get; set; }
        public String ProjectName { get; set; }
        public DateTime? Deadline { get; set; }
        public int Completeness { get; set; }
        public TaskStatus Status { get; set; }
        public Boolean isDeleted { get; set; }
        public Boolean AllowEdit { get; set; }
        public Boolean AllowDelete { get; set; }
        public Boolean AllowUndelete { get; set; }
        public Boolean AllowVirtualDelete { get; set; }
        public dtoAdminProjects() { }

        public dtoAdminProjects(Task oTask)
            {
                this.ProjectID = oTask.ID; 
                this.ProjectName = oTask.Name;
                this.Deadline = oTask.Deadline;
                this.Completeness = oTask.Completeness;
                this.Status = oTask.Status;
                this.isDeleted = oTask.MetaInfo.isDeleted;
                this.AllowEdit = true;
                AllowDelete = oTask.MetaInfo.isDeleted;
                AllowUndelete = oTask.MetaInfo.isDeleted;
                AllowVirtualDelete = !oTask.MetaInfo.isDeleted;
            }
        public dtoAdminProjects(dtoAdminProjects oTask)
        {
            this.ProjectID = oTask.ProjectID;
            this.ProjectName = oTask.ProjectName;
            this.Deadline = oTask.Deadline;
            this.Completeness = oTask.Completeness;
            this.Status = oTask.Status;
            this.isDeleted = oTask.isDeleted;
            this.AllowEdit = oTask.AllowEdit;
            AllowDelete = oTask.AllowDelete;
            AllowUndelete = oTask.AllowUndelete;
            AllowVirtualDelete = oTask.AllowVirtualDelete;
        }

    }
}
