﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class ActivityPersonRuleOverride : PersonRuleOverride
    {
        public virtual Activity Activity { get; set; }
    }
}
