using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.ScormStat.Domain
{
    public class ScormStatFilterSettings
    {
        public virtual Int64 Id { get; set; }
        public virtual lm.Comol.Core.FileRepository.Domain.ScormSettings.EvaluationType FilterType { get; set; }

        public virtual Boolean CheckScormCompletion { get; set; }

        public virtual Boolean CheckTime { get; set; }

        public virtual long MinTime { get; set; }

        public virtual Boolean CheckScore { get; set; }

        public virtual Decimal MinScore { get; set; }

        public virtual int ActivityCount { get; set; }
        
    }

}
