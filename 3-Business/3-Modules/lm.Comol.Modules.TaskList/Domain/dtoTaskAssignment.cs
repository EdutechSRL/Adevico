using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{

        [Serializable(), CLSCompliant(true)]
        public class dtoTaskAssignment
        {
           public long ID{get;set;}
           public string AssignedUser { get; set; }
           //public string AuthorOfAssignment { get; set; }
          // public System.DateTime DateOfAssignment { get; set; }
           public string Role { get; set; }
           public int Completeness { get; set; }
           public bool isDeleted { get; set; }

            public dtoTaskAssignment(long ID, string AssignedUser, string Role, int Completeness, bool isDeleted)
            {
                this.ID = ID;
                this.AssignedUser = AssignedUser;
                this.Role = Role;
                this.Completeness = Completeness;
                this.isDeleted = isDeleted;
                //this.AuthorOfAssignment = AuthorOfAssignment;
                //this.DateOfAssignment = DateOfAssignment;
            }

            public dtoTaskAssignment(TaskAssignment oTaskAssignment)
            {
                this.ID = oTaskAssignment.ID;
                this.AssignedUser = oTaskAssignment.AssignedUser.SurnameAndName;
                this.Role = oTaskAssignment.TaskRole.ToString();
                this.Completeness = oTaskAssignment.Completeness;
                this.isDeleted = oTaskAssignment.MetaInfo.isDeleted;
                //this.AuthorOfAssignment = AuthorOfAssignment;
                //this.DateOfAssignment = DateOfAssignment;
            }


            public dtoTaskAssignment()
            {
                this.ID =-1;
                this.AssignedUser = "";
                this.Role = "none";
                this.Completeness = -1;
                this.isDeleted = true;
            }
            
        }
    
}
