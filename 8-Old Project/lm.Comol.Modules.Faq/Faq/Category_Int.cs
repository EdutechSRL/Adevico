using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Faq
{
    [Serializable]
    public class Category_Int
    {
        public virtual Int64 ID { get; set; }
        public virtual String Name { get; set; }
    }
}
