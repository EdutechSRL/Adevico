using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public class dtoAssignedTasksWithProjectHeader
    {
        public String CommunityName { get; set; }
        public int CommunityID { get; set; }
        public String ProjectName { get; set; }
        public long ProjectId { get; set; }
        public IList<dtoAssignedTasks> AssignedTasks { get; set; }
        public String HeaderProject { get; set; }
 

        public dtoAssignedTasksWithProjectHeader(Task oProject, List<dtoAssignedTasks> oTasks)
        {
            if (oProject != null && oProject.ID > 0)
            {
                this.ProjectName = oProject.Name;
                this.ProjectId = oProject.ID ;
            }
            else
            {
                this.ProjectName = "MeStesso";
                this.ProjectId = 0;
            }

            if (oProject.Community != null)
                CommunityName = oProject.Community.Name;
            else CommunityName = "Portale";

            HeaderProject = this.ProjectName + "   [" + this.CommunityName + "] "; 
            
            AssignedTasks = oTasks;           
                    
        }
        
    }
}
