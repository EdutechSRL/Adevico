using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.ErrorsNotification.DataContract.Domain
{
    [Serializable]
    public class MailTemplate
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public ErrorType Type { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
    }
}