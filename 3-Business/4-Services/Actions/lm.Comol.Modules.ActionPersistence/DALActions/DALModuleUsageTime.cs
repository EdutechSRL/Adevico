using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.ActionDataContract;
using System.Data.SqlClient;
using System.Configuration;

namespace lm.ActionPersistence
{
    public class DALModuleUsageTime
    {
        public static bool ModuleUsageTime_Update(ModuleUsageTime oModuleUsageTime)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString"));

            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_ModuleUsageTime_Update", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Counter", 0);
            cmd.Parameters.AddWithValue("@ModuleID", oModuleUsageTime.ModuleID );
		    cmd.Parameters.AddWithValue("@Date", oModuleUsageTime.ActionDate);
            cmd.Parameters.AddWithValue("@CommunityID", oModuleUsageTime.CommunityID );
            cmd.Parameters.AddWithValue("@PersonID", oModuleUsageTime.PersonID );
            cmd.Parameters.AddWithValue("@UsageTime", oModuleUsageTime.UsageTime);
            cmd.Parameters.AddWithValue("@Day", oModuleUsageTime.ActionDate.Day);
            cmd.Parameters.AddWithValue("@Month", oModuleUsageTime.ActionDate.Month);
            cmd.Parameters.AddWithValue("@Year", oModuleUsageTime.ActionDate.Year);
            cmd.Parameters.AddWithValue("@Hour", oModuleUsageTime.ActionDate.Hour);
		  cmd.Parameters.AddWithValue("@ActionNumber", oModuleUsageTime.ActionNumber);
            try
            {
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }

            catch (Exception ex)
            {
                conn.Close();
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(oModuleUsageTime, ex);
            return false;
            }
        }
        
    }
}
