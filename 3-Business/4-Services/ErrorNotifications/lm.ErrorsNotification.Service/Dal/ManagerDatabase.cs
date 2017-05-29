using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using lm.ErrorsNotification.DataContract.Domain;

namespace lm.ErrorsNotification.Service.Dal
{
    public class ManagerDatabase : IManagerErrors
    {
        public void SaveCommunityModuleError(CommunityModuleError error)
        {
           
               Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_CommunityModuleError_Add");
                    if (error.UniqueID == System.Guid.Empty)
                    {
                        error.UniqueID = System.Guid.NewGuid();
                    }
                    oDatabase.AddInParameter(oCommand, "@Message", System.Data.DbType.String, error.Message);
                    oDatabase.AddInParameter(oCommand, "@InnerExceptionMessage", System.Data.DbType.String, error.InnerExceptionMessage);
                     oDatabase.AddInParameter(oCommand, "@ComolUniqueID", System.Data.DbType.String, error.ComolUniqueID);
                     oDatabase.AddInParameter(oCommand, "@SentDate", System.Data.DbType.DateTime, error.SentDate);
                    oDatabase.AddInParameter(oCommand, "@Day", System.Data.DbType.Date, error.Day);
                    oDatabase.AddInParameter(oCommand, "@UniqueID", System.Data.DbType.Guid, error.UniqueID);
                   oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int32, error.CommunityID);
                    oDatabase.AddInParameter(oCommand, "@ModuleCode", System.Data.DbType.String, error.ModuleCode);
                    oDatabase.AddInParameter(oCommand, "@Discriminator", System.Data.DbType.Int16, 1);
                   
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 0)
                        throw new Exception("no insert: sp_CommunityModuleError_Add");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ManagerDatabase", ex.Message);

                }
            }
        }

        public void SaveDBerror(DBerror error)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_DBerror_Add");
                    if (error.UniqueID == System.Guid.Empty)
                    {
                        error.UniqueID = System.Guid.NewGuid();
                    }
                    oDatabase.AddInParameter(oCommand, "@Message", System.Data.DbType.String, error.Message);
                    oDatabase.AddInParameter(oCommand, "@StackTrace", System.Data.DbType.String, error.StackTrace);
                    oDatabase.AddInParameter(oCommand, "@SQLcommand", System.Data.DbType.String, error.SQLcommand);
                     oDatabase.AddInParameter(oCommand, "@ComolUniqueID", System.Data.DbType.String, error.ComolUniqueID);
                     oDatabase.AddInParameter(oCommand, "@SentDate", System.Data.DbType.DateTime, error.SentDate);
                     oDatabase.AddInParameter(oCommand, "@Day", System.Data.DbType.Date, error.Day);

                    String parameters = "";
                    foreach (String parameter in error.SQLparameters){
                        parameters += parameter + Environment.NewLine;
                    }
                    oDatabase.AddInParameter(oCommand, "@SQLparameters", System.Data.DbType.String, parameters);

                    oDatabase.AddInParameter(oCommand, "@UniqueID", System.Data.DbType.Guid, error.UniqueID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 0)
                       throw new Exception("no insert: sp_DBerror_Add");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ManagerDatabase", ex.Message);

                }
            }
        }

        public void SaveGenericError(GenericError error)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_GenericError_Add");
                    if (error.UniqueID == System.Guid.Empty)
                    {
                        error.UniqueID = System.Guid.NewGuid();
                    }
                    oDatabase.AddInParameter(oCommand, "@Message", System.Data.DbType.String, error.Message);
                    oDatabase.AddInParameter(oCommand, "@InnerExceptionMessage", System.Data.DbType.String, error.InnerExceptionMessage);
                     oDatabase.AddInParameter(oCommand, "@ComolUniqueID", System.Data.DbType.String, error.ComolUniqueID);
                     oDatabase.AddInParameter(oCommand, "@SentDate", System.Data.DbType.DateTime, error.SentDate);
                    oDatabase.AddInParameter(oCommand, "@Day", System.Data.DbType.Date, error.Day);
                    oDatabase.AddInParameter(oCommand, "@UniqueID", System.Data.DbType.Guid, error.UniqueID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 0)
                        throw new Exception("no insert: sp_GenericError_Add");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ManagerDatabase", ex.Message);

                }
            }
        }

        public void SaveGenericModuleError(GenericModuleError error)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_CommunityModuleError_Add");
                    if (error.UniqueID == System.Guid.Empty)
                    {
                        error.UniqueID = System.Guid.NewGuid();
                    }
                    oDatabase.AddInParameter(oCommand, "@Message", System.Data.DbType.String, error.Message);
                    oDatabase.AddInParameter(oCommand, "@InnerExceptionMessage", System.Data.DbType.String, error.InnerExceptionMessage);
                    oDatabase.AddInParameter(oCommand, "@ComolUniqueID", System.Data.DbType.String, error.ComolUniqueID);
                    oDatabase.AddInParameter(oCommand, "@SentDate", System.Data.DbType.DateTime, error.SentDate);
                    oDatabase.AddInParameter(oCommand, "@Day", System.Data.DbType.Date, error.Day);
                    oDatabase.AddInParameter(oCommand, "@UniqueID", System.Data.DbType.Guid, error.UniqueID);
                    oDatabase.AddInParameter(oCommand, "@ModuleCode", System.Data.DbType.String, error.ModuleCode);
                    oDatabase.AddInParameter(oCommand, "@Discriminator", System.Data.DbType.Int16, 0);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 0)
                        throw new Exception("no insert: sp_GenericModuleError_Add");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ManagerDatabase", ex.Message);

                }
            }
        }

        public void SaveGenericWebError(GenericWebError error)
        {
            System.Diagnostics.EventLog.WriteEntry("DEBUG", "STO PER CREARE LA CONNESSIONE AL DB");
            System.Diagnostics.EventLog.WriteEntry("DEBUG", DataHelpers.ConnectionString());
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            System.Diagnostics.EventLog.WriteEntry("DEBUG", "STO PER CREARE LA CONNESSIONE AL DB");
            using (DbConnection connection = oDatabase.CreateConnection())
            {
                 try
                {
                connection.Open();
                System.Diagnostics.EventLog.WriteEntry("DEBUG", "HO APERTO LA CONNESSIONE AL DB");
                }
                 catch (Exception ex)
                 {
                     System.Diagnostics.EventLog.WriteEntry("DEBUG", ex.Message);
                 }
                
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_GenericWebError_Add");
                    if (error.UniqueID == System.Guid.Empty)
                    {
                        error.UniqueID = System.Guid.NewGuid();
                    }
                    oDatabase.AddInParameter(oCommand, "@Message", System.Data.DbType.String, error.Message);
                    oDatabase.AddInParameter(oCommand, "@InnerExceptionMessage", System.Data.DbType.String, error.InnerExceptionMessage);
                    oDatabase.AddInParameter(oCommand, "@ComolUniqueID", System.Data.DbType.String, error.ComolUniqueID);
                    oDatabase.AddInParameter(oCommand, "@SentDate", System.Data.DbType.DateTime, error.SentDate);
                    oDatabase.AddInParameter(oCommand, "@Day", System.Data.DbType.Date, error.Day);
                    oDatabase.AddInParameter(oCommand, "@UniqueID", System.Data.DbType.Guid, error.UniqueID);
                    oDatabase.AddInParameter(oCommand, "@ModuleCode", System.Data.DbType.String, error.ModuleCode);
                    oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int32, error.CommunityID);
                    oDatabase.AddInParameter(oCommand, "@ModuleID", System.Data.DbType.Int32, error.ModuleID);
                    oDatabase.AddInParameter(oCommand, "@UserID", System.Data.DbType.Int32, error.UserID);
                    oDatabase.AddInParameter(oCommand, "@ServerName", System.Data.DbType.String, error.ServerName);
	                oDatabase.AddInParameter(oCommand, "@Url", System.Data.DbType.String, error.Url);
                    oDatabase.AddInParameter(oCommand, "@QueryString", System.Data.DbType.String, error.QueryString);
                    oDatabase.AddInParameter(oCommand, "@ExceptionSource", System.Data.DbType.String, error.ExceptionSource);
                    oDatabase.AddInParameter(oCommand, "@BaseExceptionStackTrace", System.Data.DbType.String, error.BaseExceptionStackTrace);

                    oCommand.Connection = connection;
                    System.Diagnostics.EventLog.WriteEntry("DEBUG", "ASTO PER INSERIRE UN ERRORE");
                    if (oCommand.ExecuteNonQuery() == 0)
                        throw new Exception("no insert: sp_GenericWebError_Add");

                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ManagerDatabase - GenericWebError", ex.Message);
                    throw new Exception("no insert: sp_GenericWebError_Add");
                }
            }
        }

        public void SaveFileError(FileError error)
        {
             Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_FileError_Add");
                    if (error.UniqueID == System.Guid.Empty)
                    {
                        error.UniqueID = System.Guid.NewGuid();
                    }
                    oDatabase.AddInParameter(oCommand, "@Message", System.Data.DbType.String, error.Message);
                    oDatabase.AddInParameter(oCommand, "@ComolUniqueID", System.Data.DbType.String, error.ComolUniqueID);
                    oDatabase.AddInParameter(oCommand, "@SentDate", System.Data.DbType.DateTime, error.SentDate);
                    oDatabase.AddInParameter(oCommand, "@Day", System.Data.DbType.Date, error.Day);
                    oDatabase.AddInParameter(oCommand, "@UniqueID", System.Data.DbType.Guid, error.UniqueID);
                    oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int32, error.CommunityID);
                    oDatabase.AddInParameter(oCommand, "@UserID", System.Data.DbType.Int32, error.UserID);
                    oDatabase.AddInParameter(oCommand, "@CommunityFileID", System.Data.DbType.Int64, error.CommunityFileID);
                    oDatabase.AddInParameter(oCommand, "@NoticeboardFileID", System.Data.DbType.Int64, error.NoticeboardFileID);
                    oDatabase.AddInParameter(oCommand, "@ThesisFileID", System.Data.DbType.Int32, error.ThesisFileID);
                    oDatabase.AddInParameter(oCommand, "@BaseFileID", System.Data.DbType.Guid, error.BaseFileID);

                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 0)
                        throw new Exception("no insert: sp_FileError_Add");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ManagerDatabase - SaveFileError", ex.Message);

                }
            }
        }

    }
}
