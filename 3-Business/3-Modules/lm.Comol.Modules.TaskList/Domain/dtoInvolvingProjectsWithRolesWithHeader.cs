using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public class dtoInvolvingProjectsWithRolesWithHeader
    {
        public string CommunityName { get; set; }
        public int CommunityID { get; set; }
        public IList<dtoInvolvingProjectsWithRoles> AssignedTasks { get; set; }

        public dtoInvolvingProjectsWithRolesWithHeader(Community oCommunity, IList<dtoInvolvingProjectsWithRoles> oTasks)
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


        public dtoInvolvingProjectsWithRolesWithHeader(int oCommunityID, String oCommunityName, IList<dtoInvolvingProjectsWithRoles> oTasks)
        {
            if (oCommunityID !=  0)
            {
                this.CommunityName =  oCommunityName;
                this.CommunityID = oCommunityID ;
            }
            else
            {
                this.CommunityName = "Portale";
                this.CommunityID = 0;
            }

            AssignedTasks = oTasks;

        }

    }
}