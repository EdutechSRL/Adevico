using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.ErrorsNotification.DataContract.Domain
{
    [DataContract]
    public class DBerror
    {
        [DataMember]
        public System.Guid UniqueID { get; set; }

        [DataMember]
        public DateTime SentDate { get; set; }

        [DataMember]
        public DateTime Day { get; set; }
        
        [DataMember]
        public String Message { get; set; }

        [DataMember]
        public String StackTrace { get; set; }
        
        [DataMember]
        public String SQLcommand { get; set; }

        [DataMember]
        public List<String> SQLparameters { get; set; }

        [DataMember]
        public String ComolUniqueID { get; set; }

        [DataMember]
        public PersistTo Persist { get; set; }

        [DataMember]
        public ErrorType Type { get; set; }

        public DBerror() {
            this.Type = ErrorType.DBerror;
        }

        public DBerror(String comolUniqueID, PersistTo persist)
        {
            this.UniqueID = Guid.NewGuid();
            this.SentDate = DateTime.Now;
            this.Day = this.SentDate.Date;
            this.SQLparameters = new List<String>();
            this.ComolUniqueID = comolUniqueID;
            this.Persist = persist;
            this.Type = ErrorType.DBerror;
        }
        public DBerror(String message, String stackTrace, String command, List<String> parameters, String comolUniqueID, PersistTo persist)
        {
            this.UniqueID = Guid.NewGuid();
            this.SentDate = DateTime.Now;
            this.Day = this.SentDate.Date;
            this.StackTrace = stackTrace;
            this.Message = message;
            this.SQLcommand = command;
            if (parameters == null)
                this.SQLparameters = new List<String>();
            else
                this.SQLparameters = parameters;
            this.ComolUniqueID = comolUniqueID;
            this.Persist = persist;
            this.Type = ErrorType.DBerror;
        }
    }
}