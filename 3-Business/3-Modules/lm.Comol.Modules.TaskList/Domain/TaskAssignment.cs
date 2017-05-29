using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;  
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.TaskList.Domain
{
    public class TaskAssignment 
    {
        public virtual long ID { get; set; }
        public virtual TreeVisibility TreeVisibility { get; set; }
        public virtual TaskRole TaskRole { get; set; }
        public virtual int TaskPermissions { get; set; }
        public virtual Person AssignedUser { get; set; }
        public virtual Task Task { get; set; }
        public virtual MetaData MetaInfo { get; set; }
        public virtual int Completeness { get; set; }
        public virtual Task Project { get; set; }
              
        //  public virtual String Discriminator { get; set; }

        public TaskAssignment()
        {
        }
    }


}
