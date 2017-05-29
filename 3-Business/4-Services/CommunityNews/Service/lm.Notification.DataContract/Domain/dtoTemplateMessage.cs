using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.Notification.DataContract.Domain
{
    [DataContract]
    public class dtoTemplateMessage
    {
        [DataMember]
        public virtual long ID { get; set; }
        [DataMember]
        public virtual dtoTemplateType Type { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string ModuleCode { get; set; }
        [DataMember]
        public virtual int ModuleID { get; set; }
        [DataMember]
        public virtual int ActionID { get; set; }
        [DataMember]
        public virtual string Message { get; set; }
        [DataMember]
        public virtual int LanguageID { get; set; }

        public dtoTemplateMessage() { }
        public dtoTemplateMessage(lm.Notification.Core.Domain.TemplateMessage template ){
            this.ID = template.ID;
            this.ActionID = template.ActionID;
            this.Name = template.Name;
            this.ModuleCode = template.ModuleCode;
            this.ModuleID = template.ModuleID;
            this.Message = template.Message;
            this.LanguageID = template.LanguageID;
            this.Type = (dtoTemplateType)template.Type;
        }

    }
}
