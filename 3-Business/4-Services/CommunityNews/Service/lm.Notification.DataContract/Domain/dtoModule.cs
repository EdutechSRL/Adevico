using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class dtoModule{
        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Code { get; set; }
        [DataMember]
        public virtual List<dtoModuleAction> Actions { get; set; }

       public dtoModule() { }
        public dtoModule(lm.Notification.Core.Domain.NotificatedModule m) {
            this.Code = m.Code;
            this.ID = m.ID;
            this.Name = m.Name;
            this.Actions= new List<dtoModuleAction>();
            if (m.Actions!=null && m.Actions.Count > 0)
                this.Actions.AddRange((from a in m.Actions select new dtoModuleAction(a)).ToList<dtoModuleAction>());
        }

    }
}