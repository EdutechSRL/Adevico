using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public class dtoRolesPerTask
    {
        public virtual TaskRole Role { get; set; }
        public virtual Boolean isDeleted { get; set; }
        public virtual long TAid { get; set; }
       

        public dtoRolesPerTask() { }

        public dtoRolesPerTask(TaskRole oRole, Boolean oIsDeleted, long oTAid ) 
            {
                this.Role=oRole; 
                this.isDeleted=oIsDeleted;
                this.TAid = oTAid;
                 
            }

             }
}
