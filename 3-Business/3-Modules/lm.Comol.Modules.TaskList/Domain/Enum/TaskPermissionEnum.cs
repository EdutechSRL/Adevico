using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{

    [CLSCompliant(true)]
    [FlagsAttribute]
    public enum TaskPermissionEnum
    {
      //generico
      None=0,
      TreeVisibility=1,
      ManagementUser=2,
      AddFile = 4, //--->>>>>Aggiungere anche ViewFile!?!?!?!?!?!?
      //permessi relativi al task 
      TaskSetCompleteness = 8,
      TaskSetCategory=16,     
      TaskSetEndDate=32,
      TaskSetStartDate=64,
      TaskSetPriority = 128,
      TaskSetStatus=256,
      TaskSetDeadline = 512,
      TaskView=1024,
      //TaskUpdate=1024,
      TaskDelete=2048,
      TaskCreate=4096,
      ProjectDelete =8192,

      //permessi relativi al Progetto
      //ProjectSetProperties=8192,
      //ProjectView=16384,
      //ProjectUpdate=,
      
      //DA FILE MANAGEMENT 

      ChangeApprovation = 16384,
      Undelete=32768 ,
      Delete=65536,
      Download = 131072,
      VirtualDelete = 262144,
      Unlink = 524288,
      Play = 1048576
      

    }
}
//TOGLIERE UPDATE
//addUsers
