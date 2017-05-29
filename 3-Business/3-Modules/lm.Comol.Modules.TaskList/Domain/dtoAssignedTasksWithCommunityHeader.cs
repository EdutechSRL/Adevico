using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public class dtoAssignedTasksWithCommunityHeader
    {
        public string CommunityName { get; set; }
        public int CommunityID { get; set; }
        public IList<dtoAssignedTasks> AssignedTasks { get; set; }

        public dtoAssignedTasksWithCommunityHeader(Community oCommunity, IList<dtoAssignedTasks> oTasks)
        {
            if (oCommunity != null && oCommunity.Id>0)
            {
                this.CommunityName = oCommunity.Name;
                this.CommunityID = oCommunity.Id;
            }
            else {
                this.CommunityName = "Portale";
                this.CommunityID = 0;
            }
          
            AssignedTasks = oTasks;  
            
        }

    }
}
