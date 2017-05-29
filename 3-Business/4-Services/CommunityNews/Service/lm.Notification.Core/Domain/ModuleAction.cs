using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace lm.Notification.Core.Domain
{
    [Serializable]
    public class ModuleAction{
        public virtual int ID { get; set; }
        public virtual int TypeID { get; set; }
        public virtual string Name { get; set; }
        public virtual NotificatedModule Module { get; set; }
    }
}
