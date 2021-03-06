﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain.Evaluation
{
    [Serializable]
    public class CommitteeAssignedSubmitterType : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual EvaluationCommittee Committee { get; set; }
        public virtual SubmitterType SubmitterType { get; set; }
        public CommitteeAssignedSubmitterType()
        {

        }
    }
}