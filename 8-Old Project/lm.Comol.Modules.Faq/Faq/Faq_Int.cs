using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Faq
{
    [Serializable]
    public class Faq_Int
    {
        public virtual Int64 ID { get; set; }
        public virtual String Quest { get; set; }
        public virtual String Answer { get; set; }
    }
}
