using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using lm.ActionDataContract;

namespace lm.ActionPersistence
{
    public class DALModuleAction
    {
        public static bool ModuleAction_Insert(ModuleAction oAction)
        {

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString"));

            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_ModuleAction_Insert", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GuidSessionID", oAction.WorkingSessionID);
            cmd.Parameters.AddWithValue("@AccessDate", oAction.AccessDate);
            cmd.Parameters.AddWithValue("@LastActionDate", oAction.LastActionDate);
            cmd.Parameters.AddWithValue("@PersonID", oAction.PersonID);
            cmd.Parameters.AddWithValue("@CommunityID", oAction.CommunityID);
            cmd.Parameters.AddWithValue("@ModuleID", oAction.ModuleID );
            cmd.Parameters.AddWithValue("@ExitModule", oAction.isExitModule );
            cmd.Parameters.AddWithValue("@AccessDay", oAction.AccessDate.Day);
            cmd.Parameters.AddWithValue("@AccessMonth", oAction.AccessDate.Month);
            cmd.Parameters.AddWithValue("@AccessYear", oAction.AccessDate.Year);
            cmd.Parameters.AddWithValue("@AccessHour", oAction.AccessDate.Hour);
            cmd.Parameters.AddWithValue("@PersonRoleID", oAction.PersonRoleID);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }

            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(oAction, ex);
                return false;
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }

                cmd.Dispose();
                conn.Dispose();
            }



        }

        public static bool ModuleAction_Update(ModuleAction oAction)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString"));

            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_ModuleAction_Update", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GuidSessionID", oAction.WorkingSessionID);
            cmd.Parameters.AddWithValue("@AccessDate", oAction.AccessDate);
            cmd.Parameters.AddWithValue("@LastActionDate", oAction.LastActionDate);
            cmd.Parameters.AddWithValue("@PersonID", oAction.PersonID);
            cmd.Parameters.AddWithValue("@CommunityID", oAction.CommunityID);
            cmd.Parameters.AddWithValue("@ModuleID", oAction.ModuleID);
            cmd.Parameters.AddWithValue("@ExitModule", oAction.isExitModule);
            cmd.Parameters.AddWithValue("@AccessDay", oAction.AccessDate.Day);
            cmd.Parameters.AddWithValue("@AccessMonth", oAction.AccessDate.Month);
            cmd.Parameters.AddWithValue("@AccessYear", oAction.AccessDate.Year);
            cmd.Parameters.AddWithValue("@AccessHour", oAction.AccessDate.Hour);
            cmd.Parameters.AddWithValue("@PersonRoleID", oAction.PersonRoleID);

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
                pError.addMessageToPoisonQueue(oAction, ex);
                return false;
            }


        }
    }
}
