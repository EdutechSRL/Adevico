using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public class dtoCommunityForDDL
    {
        public int ID { get; set; }
        public String Name { get; set; }


        public dtoCommunityForDDL() { }

        public dtoCommunityForDDL(Community oCommunity) 
            {
                this.ID = oCommunity.Id;
                this.Name = oCommunity.Name; 
            }    
    }
}
