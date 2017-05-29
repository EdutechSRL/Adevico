using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable(), CLSCompliant(true)]
    public class TaskListFilePermission
    {

        public bool ChangeApprovation {get; set;}
        public bool Undelete { get; set; }
        public bool Delete { get; set; }
        public bool Download { get; set; }
        public bool VirtualDelete { get; set; }
        public bool Unlink { get; set; }
        public bool Play { get; set; }

        public bool DownLoadFile { get; set; }
        
        public TaskListFilePermission()
        {
        }

    }
}

