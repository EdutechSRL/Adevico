using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using lm.Notification.Core.Domain;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace lm.Notification.Service.DAL
{
    public class NotificationToPersonRepository{
        public static bool Add(PersonNotification oNotification){
            Database oDatabase =  DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection()){
                connection.Open();
                try{
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_NotificationToPersonRepository_Add");
                    oDatabase.AddInParameter(oCommand, "@UniqueID", System.Data.DbType.Guid, oNotification.NotificationUniqueID);
                    oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, oNotification.PersonID);
                    oDatabase.AddInParameter(oCommand, "@ID", System.Data.DbType.Guid, System.Guid.NewGuid());
                    oDatabase.AddInParameter(oCommand, "@Day", System.Data.DbType.DateTime, oNotification.Day);
                    oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, oNotification.CommunityID);
                    oDatabase.AddInParameter(oCommand, "@SentDate", System.Data.DbType.DateTime, oNotification.SentDate);
                    oCommand.Connection=connection;
                    if (oCommand.ExecuteNonQuery()==1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex){
                    System.Diagnostics.EventLog.WriteEntry("NotificationToPersonRepository", ex.Message);
                      return false;
                }
            } 
       }

        public static bool RemoveNotification(Guid NotificationID)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveNotification");
                    oDatabase.AddInParameter(oCommand, "@NotificationID", System.Data.DbType.Guid, NotificationID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("RemoveNotification", ex.Message);
                    return false;
                }
            } 
        }

        public static bool RemoveNotificationForUser(Guid NotificationID, int PersonID)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveNotificationForUser");
                    oDatabase.AddInParameter(oCommand, "@NotificationID", System.Data.DbType.Guid, NotificationID);
                    oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("RemoveNotificationForUser", ex.Message);
                    return false;
                }
            } 
        }

        public static bool RemoveUserNotification(Guid UserNotificationID, int PersonID)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveUserNotification");
                    oDatabase.AddInParameter(oCommand, "@UserNotificationID", System.Data.DbType.Guid, UserNotificationID);
                    oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("RemoveUserNotification", ex.Message);
                    return false;
                }
            } 
        }

        public static bool ReadNotification(Guid NotificationID, int PersonID)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_ReadNotification");
                    oDatabase.AddInParameter(oCommand, "@NotificationID", System.Data.DbType.Guid, NotificationID);
                    oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ReadNotification", ex.Message);
                    return false;
                }
            } 
        }

        public static bool ReadUserNotification(Guid UserNotificationID, int PersonID)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_ReadUserNotification");
                    oDatabase.AddInParameter(oCommand, "@UserNotificationID", System.Data.DbType.Guid, UserNotificationID);
                    oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ReadUserNotification", ex.Message);
                    return false;
                }
            } 
        }

        public static bool ReadUserCommunityNotification(int CommunityID, int PersonID)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_ReadUserCommunityNotification");
                    oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int32, CommunityID);
                    oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ReadUserCommunityNotification", ex.Message);
                    return false;
                }
            } 
        }
        public static bool RemoveUserCommunityNotification(int CommunityID, int PersonID)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveUserCommunityNotification");
                    oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int32, CommunityID);
                    oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int32, PersonID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("RemoveUserCommunityNotification", ex.Message);
                    return false;
                }
            } 
        }
    }
}