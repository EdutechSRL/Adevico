﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class dtoPermission 
    {
        public virtual Boolean AllowDelete { get; set; }
        public virtual Boolean AllowVirtualDelete { get; set; }
        public virtual Boolean AllowUnDelete { get; set; }
        public virtual Boolean AllowEdit { get; set; }
        public virtual Boolean AllowSetAvailable { get; set; }
        public virtual Boolean AllowSetUnavailable { get; set; }
        public virtual Boolean AllowView { get; set; }
        public virtual Boolean AllowAssignTo { get; set; }

        public dtoPermission()
        { 
        
        }
    }
}