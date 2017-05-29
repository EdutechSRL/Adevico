using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Notification.Core.Domain
{
    [Serializable]
    public class NotificatedObject
    {
        public virtual System.Guid ID { get; set; }
        public virtual System.Guid UniqueNotifyID { get; set; }
        public virtual int ModuleID { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual int ObjectTypeID { get; set; }
        public virtual String FullyQualiFiedName { get; set; }
        public virtual String ObjectID { get; set; }
    }
}
