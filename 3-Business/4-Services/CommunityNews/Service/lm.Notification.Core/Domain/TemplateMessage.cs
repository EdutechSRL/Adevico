using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Notification.Core.Domain
{
    [Serializable]
    public class TemplateMessage
    {
        public virtual long ID { get; set; }
        public virtual TemplateType Type { get; set; }
        public virtual string Name { get; set; }
        public virtual string ModuleCode { get; set; }
        public virtual int ModuleID { get; set; }
        public virtual int ActionID { get; set; }
        public virtual string Message { get; set; }
        public virtual int LanguageID { get; set; }

    }
}
