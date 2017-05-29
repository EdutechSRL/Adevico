using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
    public class dtoUserWithRole
    {
        public int PersonID { get; set; }
        public TaskRole Role { get; set; }

        public dtoUserWithRole()
        {
            PersonID = -1;
            Role = TaskRole.None;
        }

        public dtoUserWithRole(dtoReallocateTA dto)
        {
            PersonID = dto.PersonID;
            Role = dto.Role;        
        }

    }
}
