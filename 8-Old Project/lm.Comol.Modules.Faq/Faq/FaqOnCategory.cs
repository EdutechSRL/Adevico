using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Faq
{
    [Serializable]
    public class FaqOnCategory
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64 FaqId { get; set; }
        public virtual Int64 CatId { get; set; }
    }
}
