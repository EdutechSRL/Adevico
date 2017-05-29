using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;
namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable, CLSCompliant(true)]
    public class dtoUsers
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public dtoUsers(Person oPerson) 
            {
                this.Id = oPerson.Id;
                this.Name = oPerson.Name + " " + oPerson.Surname ; 

            }

        }
     
}
 
