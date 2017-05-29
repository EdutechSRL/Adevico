using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public class dtoAdminProjectsWithCommunityHeader
    {
        public String CommunityName { get; set; }
        public int CommunityId { get; set; }
        public List<dtoAdminProjects> ProjectsList { get; set; }

        public dtoAdminProjectsWithCommunityHeader() { }

        public dtoAdminProjectsWithCommunityHeader(Community oCommunity , List<dtoAdminProjects> oProjects) 
            {
                this.CommunityId = oCommunity.Id;
                this.CommunityName=oCommunity.Name;
                this.ProjectsList = oProjects;
            }
    }
}
