using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.ErrorsNotification.DataContract.Domain;
using System.Net.Mail;

namespace lm.ErrorsNotification.Service.Dal
{
    public class ManagerMail : IManagerErrors
    {
        private MailTemplate _template;
        private ErrorSettings _setting;

        #region IManagerErrors Members

        public void SaveCommunityModuleError(CommunityModuleError error)
        {
            try
            {
                String subject = string.Format(_template.Subject, error.ComolUniqueID);
                String body = string.Format(_template.Body, error.SentDate.ToLongDateString(), error.ModuleCode, error.CommunityID, error.Message, error.InnerExceptionMessage);
                DataHelpers.SendMessage(_setting, subject, body);

            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Mail - CommunityModuleError ", ex.Message);
                throw ex;
            }
        }

        public void SaveDBerror(DBerror error)
        {
            try { 
                String subject = string.Format(_template.Subject, error.ComolUniqueID);
                String parameters = "";
                foreach (String parameter in error.SQLparameters)
                {
                    parameters += parameter + Environment.NewLine;
                }
                String body = string.Format(_template.Body, error.SentDate.ToLongDateString(), error.SQLcommand, parameters, error.Message, error.StackTrace);
                DataHelpers.SendMessage(_setting , subject, body);

            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Mail - DBerror", ex.Message);
                throw ex;
            }
        }

        public void SaveGenericError(GenericError error)
        {
            try
            {
                String subject = string.Format(_template.Subject, error.ComolUniqueID);
                String body = string.Format(_template.Body, error.SentDate.ToLongDateString(), error.Message, error.InnerExceptionMessage);
                DataHelpers.SendMessage(_setting, subject, body);

            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Mail - GenericError :", ex.Message);
                throw ex;
            }
        }

        public void SaveGenericModuleError(GenericModuleError error)
        {
            try
            {
                String subject = string.Format(_template.Subject, error.ComolUniqueID);
                String body = string.Format(_template.Body, error.SentDate.ToLongDateString(),error.ModuleCode, error.Message, error.InnerExceptionMessage);
                DataHelpers.SendMessage(_setting, subject, body);

            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Mail - GenericModuleError: ", ex.Message);
                throw ex;
            }
        }

        public void SaveGenericWebError(GenericWebError error)
        {
            try
            {
                String subject = string.Format(_template.Subject, error.ComolUniqueID);
                String body = string.Format(_template.Body, error.SentDate.ToLongDateString(), error.ServerName, error.UserID, error.ModuleCode, error.ModuleID,
                    error.CommunityID, error.Url, error.QueryString, error.Message, error.InnerExceptionMessage, error.ExceptionSource, error.BaseExceptionStackTrace);
                System.Diagnostics.EventLog.WriteEntry("DEBUG", "STO PER INVIARE LA MAIL");
                DataHelpers.SendMessage(_setting, subject, body);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Mail - GenericWebError ", ex.Message);
                throw ex;
            }
        }

        public void SaveFileError(FileError error)
        {
            try
            {
                String subject = string.Format(_template.Subject, error.ComolUniqueID);
                String body = string.Format(_template.Body, error.SentDate.ToLongDateString(), error.UserID, error.CommunityID, error.CommunityFileID,error.BaseFileID,error.NoticeboardFileID,error.ThesisFileID, error.Message);
                DataHelpers.SendMessage(_setting, subject, body);

            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Mail - SaveFileError ", ex.Message);
                throw ex;
            }
        }

        #endregion

        public ManagerMail() { }

        public ManagerMail( MailTemplate template,ErrorSettings setting ) {
            _template = template;
            _setting = setting;
        }     
    }
}