using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace lm.Comol.Modules.Faq
{
    [Serializable]
    public class Category
    {
        public virtual Int64 Id { get; set; }

        [Required(ErrorMessage = "*"), StringLength(255, ErrorMessage = "Must be under 255 characters")]
        public virtual String Name { get; set; }

        public virtual Int64 Elements { get; set; }
        
        //public virtual IList<Faq> Faqs { get; set; }
        
        public virtual Boolean IsSelected { get; set; }

        public virtual Int32 CommunityId { get; set; }
        public virtual IList<Category_Int> Internationalizations { get; set; }

        public virtual IList<Faq> Faqs { get; set; }
    }
}
