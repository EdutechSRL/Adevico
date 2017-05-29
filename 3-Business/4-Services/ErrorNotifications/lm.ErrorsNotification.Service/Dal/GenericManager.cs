using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.ErrorsNotification.DataContract.Domain;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace lm.ErrorsNotification.Service.Dal
{
    public class GenericManager
    {
        public List<MailTemplate> GetTemplates()
        {
            List<MailTemplate> oList = new List<MailTemplate>();
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    String query = "SELECT Id,Name,ErrorType,Subject,Body FROM MailTemplate";
                    DbCommand oCommand = oDatabase.GetSqlStringCommand(query);
                    oCommand.Connection = connection;
                    DbDataReader reader= oCommand.ExecuteReader();
                    while (reader.Read()){
                       MailTemplate template = new  MailTemplate();
                        template.Id= (long)reader.GetValue(0);
                        template.Name= (String)reader.GetValue(1);
                        template.Type= (ErrorType)reader.GetValue(2);
                        template.Subject= (String)reader.GetValue(3);
                        template.Body= (String)reader.GetValue(4);
                        oList.Add(template);
                    }
                   
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("-GetTemplates", ex.Message);
                }
            }
          return oList;
        }


        public List<MailNotificationSettings> GetMailNotificationSettings()
        {
            List<MailNotificationSettings> oList = new List<MailNotificationSettings>();
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    String query = "SELECT Id,ComolUniqueID,NotifyedOn,ErrorsCount FROM MailNotificationSettings";
                    DbCommand oCommand = oDatabase.GetSqlStringCommand(query);
                    oCommand.Connection = connection;
                    DbDataReader reader= oCommand.ExecuteReader();
                    while (reader.Read()){
                       MailNotificationSettings setting = new MailNotificationSettings();
                        setting.Id= (long)reader.GetValue(0);
                        setting.ComolUniqueID= (String)reader.GetValue(1);
                        setting.ErrorsCount= (int)reader.GetValue(3);
                        setting.NotifyedOn= (DateTime?)reader.GetValue(2);
                        oList.Add(setting);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ManagerDatabase", ex.Message);
                    throw new Exception(ex.Message, ex);
                }
            }
          return oList;
        }


        public List<ErrorSettings> GetErrorSettings()
        {
            List<ErrorSettings> oList = new List<ErrorSettings>();
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    String query = "SELECT Id,ComolUniqueID,Name,ServerSMTP,SenderMail,SenderName,RealMailSender,ReplyTo,RecipientMail,NotifyAfterErrors,NotificationDelay,UseAuthentication,AccountName,AccountPassword,Port,UseSSL FROM Settings";
                    DbCommand oCommand = oDatabase.GetSqlStringCommand(query);
                    oCommand.Connection = connection;
                    DbDataReader reader= oCommand.ExecuteReader();
                    while (reader.Read()){
                        ErrorSettings setting = new  ErrorSettings();
                        setting.Id= (long)reader.GetValue(0);
                        setting.ComolUniqueID= (String)reader.GetValue(1);
                        setting.Name= (String)reader.GetValue(2);
                        setting.HostSMTP= (String)reader.GetValue(3);
                        setting.SenderMail= (String)reader.GetValue(4);
                        setting.SenderName= (String)reader.GetValue(5);
                        setting.RealMailSender= (String)reader.GetValue(6);
                        setting.ReplyTo= (String)reader.GetValue(7);
                        setting.RecipientMail= (String)reader.GetValue(8);
                        setting.NotifyAfterErrors= (int)reader.GetValue(9);
                        setting.NotificationDelay = (int)reader.GetValue(10);
                        setting.UseAuthentication =(Boolean)reader.GetValue(11);
                        setting.AccountName =(String)reader.GetValue(12);
                        setting.AccountPassword =(String)reader.GetValue(13);
                        setting.Port =(int)reader.GetValue(14);
                        setting.UseSsl =(Boolean)reader.GetValue(15);

                        oList.Add(setting);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("-GetErrorSettings", ex.Message);
                    throw new Exception(ex.Message, ex);
                }
            }
          return oList;
        }
    }
}