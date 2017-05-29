using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class dtoModuleAction{
        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public virtual int TypeID { get; set; }
        [DataMember]
        public virtual string Name { get; set; }

        public dtoModuleAction() { }
        public dtoModuleAction(lm.Notification.Core.Domain.ModuleAction action) {
            this.ID = action.ID;
            this.Name = action.Name;
            this.TypeID = action.TypeID;
        }

    }
}
