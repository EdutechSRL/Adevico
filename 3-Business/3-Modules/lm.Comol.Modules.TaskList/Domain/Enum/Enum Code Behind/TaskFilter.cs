using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public enum TaskFilter
    {
       None = 0,
       AllCommunities = 1,
       CurrentCommunity = 2,
       PortalPersonal = 3,
       CommunityPersonal = 4,
       AllCommunitiesPersonal= 5
    }
}
