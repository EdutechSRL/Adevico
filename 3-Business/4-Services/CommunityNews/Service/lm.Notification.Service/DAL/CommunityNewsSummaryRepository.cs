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
    public class CommunityNewsSummaryRepository
    {
        public static bool Save(CommunityNewsSummary reminder)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand;
                    if (reminder.ID == System.Guid.Empty)
                    {
                        oCommand = oDatabase.GetStoredProcCommand("sp_CommunityNewsSummaryRepository_Add");
                        reminder.ID = System.Guid.NewGuid();
                    }
                    else
                    {
                        oCommand = oDatabase.GetStoredProcCommand("sp_CommunityNewsSummaryRepository_Update");
                    }
                    oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, reminder.CommunityID);
                    oDatabase.AddInParameter(oCommand, "@CountAction", System.Data.DbType.Int64, reminder.ActionCount);
                    oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, reminder.PersonID);
                    oDatabase.AddInParameter(oCommand, "@ID", System.Data.DbType.Guid, reminder.ID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("CommunityNewsSummaryRepository", ex.Message);
                    return false;
                }
            }
        }

        public static bool Save(long PersonID, long CommunityID)
        {
            Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

            using (DbConnection connection = oDatabase.CreateConnection())
            {
                connection.Open();
                try
                {
                    DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_CommunityNewsSummaryRepository_InsertUpdate");
                    oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, CommunityID);
                    oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, PersonID);
                    oCommand.Connection = connection;
                    if (oCommand.ExecuteNonQuery() == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("NotificationToPersonRepository", ex.Message);
                    return false;
                }
            }
        }
    }
}
