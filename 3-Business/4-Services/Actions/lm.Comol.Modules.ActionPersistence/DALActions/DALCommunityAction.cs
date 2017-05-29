using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using lm.ActionDataContract;

namespace lm.ActionPersistence
{
    public class DALCommunityAction
    {
        public static bool CommunityAction_Insert(CommunityAction oCOMAction)
        {

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString"));

            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_CommunityAction_Insert", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GuidSessionID", oCOMAction.WorkingSessionID);
            cmd.Parameters.AddWithValue("@AccessDate", oCOMAction.AccessDate);
            cmd.Parameters.AddWithValue("@LastActionDate", oCOMAction.LastActionDate);
            cmd.Parameters.AddWithValue("@PersonID", oCOMAction.PersonID );
            cmd.Parameters.AddWithValue("@CommunityID", oCOMAction.CommunityID);
            cmd.Parameters.AddWithValue("@ExitCommunity", oCOMAction.isExitCommunity );
            cmd.Parameters.AddWithValue("@AccessDay", oCOMAction.AccessDate.Day );
            cmd.Parameters.AddWithValue("@AccessMonth", oCOMAction.AccessDate.Month );
            cmd.Parameters.AddWithValue("@AccessYear", oCOMAction.AccessDate.Year );
            cmd.Parameters.AddWithValue("@AccessHour", oCOMAction.AccessDate.Hour );
            cmd.Parameters.AddWithValue("@PersonRoleID", oCOMAction.PersonRoleID );

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }

            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(oCOMAction, ex);
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

        public static bool CommunityAction_Update(CommunityAction oCOMAction)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString"));

            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_CommunityAction_Update", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GuidSessionID", oCOMAction.WorkingSessionID);
            cmd.Parameters.AddWithValue("@LastActionDate", oCOMAction.LastActionDate);
            cmd.Parameters.AddWithValue("@ExitCommunity", oCOMAction.isExitCommunity);
            cmd.Parameters.AddWithValue("@CommunityID", oCOMAction.CommunityID);
            cmd.Parameters.AddWithValue("@PersonID", oCOMAction.PersonID );

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
                pError.addMessageToPoisonQueue(oCOMAction, ex);
                return false;
            }


        }
    }
}
