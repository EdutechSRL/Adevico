using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Notification.Core.Domain
{
    [Serializable]
    public class NotificatedModule{
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual IList<ModuleAction> Actions { get; set; }  
    }
}
