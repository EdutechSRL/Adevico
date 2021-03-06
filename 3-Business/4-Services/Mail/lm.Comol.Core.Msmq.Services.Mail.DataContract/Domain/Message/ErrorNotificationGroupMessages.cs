﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Mail.DataContract.Domain.Messages
{
    [Serializable, DataContract]
    public class ErrorNotificationGroupMessages
    {
        [DataMember]
        public Notification.Domain.GroupMessages Group { get; set; }
        [DataMember]
        public String AttachmentPath { get; set; }
        [DataMember]
        public String AttachmentSavedPath { get; set; }

        [DataMember]
        public lm.Comol.Core.Msmq.DataContract.ErrorInfo Error { get; set; }

        public ErrorNotificationGroupMessages()
        {
            Error = new lm.Comol.Core.Msmq.DataContract.ErrorInfo();
        }
    }
}
