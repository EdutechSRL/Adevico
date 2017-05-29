using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.TaskList.Domain 
{
    [CLSCompliant(true)]
    public class dtoAssignUsers

    {
        //public long TaskAssignmentId { get; set; }
        public IList<dtoRolesPerTask> Roles { get; set;}
        public String UserName { get; set; }
        public int Completeness { get; set; }
        public bool isDeleted { get; set; }


        public dtoAssignUsers() { }
        public dtoAssignUsers(Person oPerson, IList<dtoRolesPerTask> oRoles, IList<int> oTaskCompleteness) 
            {
                //this.TaskAssignmentId = oTaskAssignment.ID;
                this.Roles = oRoles;

                if (oTaskCompleteness.Count == 0)
                    this.Completeness = -1;
                else
                    this.Completeness = oTaskCompleteness[0];  

                this.UserName = oPerson.Name + " " + oPerson.Surname;
                //this.isDeleted = oTaskAssignment.MetaInfo.isDeleted;                
            }

        //public dtoAssignUsers(Person oPerson, IList<TaskRole> oRoles, TaskAssignment oTaskAssignment)
        //{
        //    TaskAssignmentId = oTaskAssignment.ID;
        //    Roles = oRoles;
        //    Completeness = oTaskAssignment.Task.Completeness;
        //    UserName =  oPerson.Name + " " + oPerson.Surname;

        //}
 

    }
}
