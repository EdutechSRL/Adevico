using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.ErrorsNotification.DataContract.Domain
{
    [DataContract]
    public class GenericWebError
    {
        [DataMember]
        public System.Guid UniqueID { get; set; }

        [DataMember]
        public int CommunityID { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public int ModuleID { get; set; }

        [DataMember]
        public string ModuleCode { get; set; }

        [DataMember]
        public DateTime SentDate { get; set; }

        [DataMember]
        public DateTime Day { get; set; }
        
        [DataMember]
        public String ServerName { get; set; }

        [DataMember]
        public String Url { get; set; }

        [DataMember]
        public String QueryString { get; set; }

        [DataMember]
        public String Message { get; set; }

        [DataMember]
        public String ExceptionSource { get; set; }

        [DataMember]
        public String BaseExceptionStackTrace { get; set; }

        [DataMember] 
        public String  InnerExceptionMessage  { get; set; }

        [DataMember]
        public String ComolUniqueID { get; set; }

        [DataMember]
        public PersistTo Persist { get; set; }

        [DataMember]
        public ErrorType Type { get; set; }

        public GenericWebError() {
            this.Type = ErrorType.GenericWebError;
        }

        public GenericWebError(String comolUniqueID, PersistTo persist)
        {
            this.UniqueID = Guid.NewGuid();
            this.SentDate = DateTime.Now;
            this.Day = this.SentDate.Date;
            this.ComolUniqueID = comolUniqueID;
            this.Persist = persist;
            this.Type = ErrorType.GenericWebError;
        }

        public GenericWebError(System.Guid uniqueID, int communityID, int moduleID,String moduleCode,int userID, DateTime sentDate, DateTime day,
            String serverName, String url, string queryString, string exceptionSource, string baseExceptionStackTrace, string innerExceptionMessage, String message, String comolUniqueID, PersistTo persist)
        {
            this.UniqueID = uniqueID;
            this.CommunityID = communityID;
            this.ModuleID = moduleID;
            this.ModuleCode = moduleCode;
            this.UserID = userID;
            this.SentDate = sentDate;
            this.Day = day;
            this.ServerName = serverName;
            this.Url = url;
            this.QueryString = queryString;
            this.Message = message;
            this.ExceptionSource = exceptionSource;
            this.BaseExceptionStackTrace = baseExceptionStackTrace;
            this.InnerExceptionMessage = innerExceptionMessage;
            this.ComolUniqueID = comolUniqueID;
            this.Persist = persist;
            this.Type = ErrorType.GenericWebError;
        }
    }
}