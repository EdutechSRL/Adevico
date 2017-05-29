using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using lm.ActionDataContract;

namespace lm.ActionPersistence
{
    public class DALLoginAction
    {
        public static bool LoginAction_Insert(LoginAction oLoginAction)
        {

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString"));

            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_LoginAction_Insert", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GuidSessionID", oLoginAction.WorkingSessionID);
		    cmd.Parameters.AddWithValue("@LoginDate", oLoginAction.LoginDate );
		    cmd.Parameters.AddWithValue("@LastActionDate", oLoginAction.LoginDate);
            cmd.Parameters.AddWithValue("@PersonID", oLoginAction.PersonID);
            cmd.Parameters.AddWithValue("@SessionClosed",oLoginAction.isWorkingSessionClosed );
            cmd.Parameters.AddWithValue("@ActionNumber", oLoginAction.ActionNumber );
            cmd.Parameters.AddWithValue("@LoginDay", oLoginAction.LoginDate.Day );
            cmd.Parameters.AddWithValue("@LoginMonth", oLoginAction.LoginDate.Month );
            cmd.Parameters.AddWithValue("@LoginYear", oLoginAction.LoginDate.Year );
            cmd.Parameters.AddWithValue("@LoginHour", oLoginAction.LoginDate.Hour);
            cmd.Parameters.AddWithValue("@PersonRoleID", oLoginAction.PersonRoleID);

		  try
		  {
			 cmd.ExecuteNonQuery();
		 return true;
		  }

		  catch (Exception ex)
		  {
			 ErrorHandler pError = new ErrorHandler();
			 pError.addMessageToPoisonQueue(oLoginAction, ex);
			 return false;
		  }
		  finally { 
			if (conn.State!= System.Data.ConnectionState.Closed) {
				 conn.Close();
			}

			cmd.Dispose();
			conn.Dispose();
		  }



        }

        public static bool LoginAction_Update(LoginAction oLoginAction)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString"));

            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_LoginAction_Update", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SessionID", oLoginAction.WorkingSessionID);
		    cmd.Parameters.AddWithValue("@LastActionDate", oLoginAction.LastActionDate);
		    cmd.Parameters.AddWithValue("@SessionClosed", oLoginAction.isWorkingSessionClosed);

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
                pError.addMessageToPoisonQueue(oLoginAction, ex);
                return false;
            }


        }
    }
}
