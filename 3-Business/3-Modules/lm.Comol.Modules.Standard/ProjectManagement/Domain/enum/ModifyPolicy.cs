﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public enum ModifyPolicy
    {
        FullFields = 0,
        DateCalculationFields = 1,
    }
}