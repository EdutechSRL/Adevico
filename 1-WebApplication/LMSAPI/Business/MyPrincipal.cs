using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Adevico.WebAPI.ActionFilter
{
    public class MyPrincipal :IPrincipal
    {
        public bool IsInRole(string role)
        {
            return true;
        }

        public IIdentity Identity
        {
            get
            {
                IIdentity test = new GenericIdentity("nome", "tipo");
                return test;
            }
        }

        public Int64 Id { get; set; }
    }
}