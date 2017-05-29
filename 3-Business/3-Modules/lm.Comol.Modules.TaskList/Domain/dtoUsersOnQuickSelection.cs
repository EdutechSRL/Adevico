using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
     [Serializable(), CLSCompliant(true)]
    public class dtoUsersOnQuickSelection  
    {
        
        public int  Id {get; set;}
        public String Name{get; set;}

        public String Rolelist { get; set; }
        public int Completeness { get; set; }
        
        public dtoUsersOnQuickSelection() { }

        public dtoUsersOnQuickSelection(Person oPerson, String oRolelist, int oPercent) 
        {
            Id = oPerson.Id;
            Name = oPerson.SurnameAndName; //oPerson.Name.ToString() + " " + oPerson.Surname.ToString();
            Rolelist = oRolelist;
            Completeness = oPercent;
        }

        public dtoUsersOnQuickSelection(Person oPerson) 
        {
            Id = oPerson.Id;
            Name = oPerson.Name.ToString() + " " + oPerson.Surname.ToString();
            Rolelist =  " ";
            Completeness = 0 ;
        }
    }
}
