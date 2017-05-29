using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Mail.DataContract.Domain.Messages
{
    [Serializable, DataContract]
    public class ErrorMessage
    {
        [DataMember]
        public lm.Comol.Core.MailCommons.Domain.Messages.Message Message { get; set; }
        [DataMember]
        public lm.Comol.Core.Msmq.DataContract.ErrorInfo Error { get; set; }

        public ErrorMessage()
        {
            Error = new lm.Comol.Core.Msmq.DataContract.ErrorInfo();
        }
    }
   
   
}