using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;
 

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable(), CLSCompliant(true)]
    public class dtoInvolvingProjects
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public TaskStatus Status { get; set; }
        public Boolean isDeleted { get; set; }
        public int Completeness { get; set; }
        public DateTime? Deadline { get; set; }

        public dtoInvolvingProjects(Task oProject)
        {
            ProjectId = oProject.ID;
            ProjectName = oProject.Name;


            Status = oProject.Status;
            isDeleted = oProject.MetaInfo.isDeleted;
            Completeness = oProject.Completeness;

            if (oProject.Deadline.HasValue)
                Deadline = oProject.Deadline;
            else Deadline = oProject.EndDate;

        }
    }



}