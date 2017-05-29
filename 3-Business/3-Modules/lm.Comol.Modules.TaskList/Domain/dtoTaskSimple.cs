using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
    public class dtoTaskSimple
    {
        public String Name { get; set; }
        public long ID { get; set; }
        public bool isDeleted { get; set; }

        public dtoTaskSimple(Task oTask)
        {
            Name = oTask.TaskWBSstring + oTask.TaskWBSindex + " " + oTask.Name;
            ID = oTask.ID;
            isDeleted = oTask.MetaInfo.isDeleted;
        }
        public dtoTaskSimple()
        {
           
        }
    }
}
