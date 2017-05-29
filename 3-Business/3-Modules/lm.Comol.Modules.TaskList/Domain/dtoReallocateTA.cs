using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
    public class dtoReallocateTA
    {
        public Guid ID { get; set; }
        public int Completeness { get; set; }
        public long TaskID { get; set; }
        public lm.Comol.Modules.TaskList.Domain.TaskRole Role { get; set; }
        public int PersonID { get; set; }
        public String PersonSurnameName { get; set; }
        public bool isDeleted { get; set; }

        public dtoReallocateTA()
        {
            ID = System.Guid.NewGuid();
            Completeness = -1;
            TaskID = 0;
            Role = lm.Comol.Modules.TaskList.Domain.TaskRole.None;
            PersonID = 0;
            PersonSurnameName = "";
            isDeleted = true;
        }

        public dtoReallocateTA(TaskAssignment oTA)
        {
         ID =System.Guid.NewGuid();
         Completeness = oTA.Completeness;
         TaskID = oTA.Task.ID;
         Role = oTA.TaskRole;
         PersonID = oTA.AssignedUser.Id;
         PersonSurnameName = oTA.AssignedUser.SurnameAndName; 
         isDeleted = oTA.MetaInfo.isDeleted;
        }

        public dtoReallocateTA(long TaskID, lm.Comol.Modules.TaskList.Domain.TaskRole Role, int PersonID, string PersonSurnameAndName)
        {
            ID = System.Guid.NewGuid();
            Completeness = 0;
            this.TaskID = TaskID;
            this.Role = Role;
            this.PersonID = PersonID;
            this.PersonSurnameName = PersonSurnameAndName;
            isDeleted = false;
        }


        public dtoReallocateTA(dtoReallocateTA oDTO)
        {
            ID = System.Guid.NewGuid();
            Completeness = oDTO.Completeness;
            TaskID = oDTO.TaskID;
            Role = oDTO.Role;
            PersonID = oDTO.PersonID;
            PersonSurnameName = oDTO.PersonSurnameName;
            isDeleted = oDTO.isDeleted;
        }
        
    }


    public class dtoReallocateTACompare: IEqualityComparer< dtoReallocateTA>
    {


        #region IEqualityComparer<dtoReallocateTA> Members

        public bool Equals(dtoReallocateTA x, dtoReallocateTA y)
        {
            return x.Role.Equals(y.Role) && x.PersonID.Equals(y.PersonID)&& x.TaskID.Equals(y.TaskID);
        }

        public int GetHashCode(dtoReallocateTA obj)
        {
            return obj.Role.GetHashCode() ^ obj.PersonID.GetHashCode()^ obj.TaskID.GetHashCode();
        }

        #endregion
    }


}
