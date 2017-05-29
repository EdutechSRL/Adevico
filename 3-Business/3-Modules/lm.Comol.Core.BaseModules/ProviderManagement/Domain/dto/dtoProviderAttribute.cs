﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Core.BaseModules.ProviderManagement
{
    [Serializable]
    public class dtoProviderAttribute
    {
        public virtual long Id { get; set; }
        public virtual long IdProvider { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual String Name { get; set; }
        public virtual String Alias { get; set; }
        public virtual String Description { get; set; }
        public virtual AttributeType Type { get; set; }
    }
}
