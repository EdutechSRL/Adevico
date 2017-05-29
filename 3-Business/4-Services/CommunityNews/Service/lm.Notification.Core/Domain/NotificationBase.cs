using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Notification.Core.Domain
{
    [Serializable]
    public class NotificationBase
    {
        public virtual System.Guid  ID { get; set; }
        public virtual System.Guid UniqueNotifyID { get; set; }
        public virtual int LanguageID { get; set; }
        public virtual int CommunityID { get; set; }
        public virtual int ModuleID { get; set; }
        public virtual string ModuleCode { get; set; }
        public virtual int ActionID { get; set; }
        public virtual DateTime SentDate { get; set; }
        public virtual DateTime SavedDate { get; set; }
        public virtual DateTime  Day { get; set; }
        public virtual TemplateMessage Template { get; set; }
        public virtual Boolean isDeleted{ get; set; }
        public virtual String Message { get; set; }

        
    }
}
