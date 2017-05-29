using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_CoreServices.Domain
{
    public class SmallRolePerson
    {
        public int RoleID;
        public int PersonID;

        public SmallRolePerson(int roleID, int personID) {
            this.RoleID = roleID;
            this.PersonID = personID;
        }
    }
}