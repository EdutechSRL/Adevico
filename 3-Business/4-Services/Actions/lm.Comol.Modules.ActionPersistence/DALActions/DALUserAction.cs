using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using lm.ActionDataContract;

namespace lm.ActionPersistence
{
    public class DALUserAction
    {
        public static bool UserAction_Insert(UserAction oAction)
        {
            
		  using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString"))){
			 conn.Open();

			 SqlCommand cmd = conn.CreateCommand();
			 SqlTransaction tn;

		      // Start a local transaction.
			 //tn = conn.BeginTransaction("AddAction");

			 // Must assign both transaction object and connection
			 // to Command object for a pending local transaction
			 cmd.Connection = conn;
			// cmd.Transaction = tn;
			 try {
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.CommandText = "sp_Action_Insert";
				cmd.Parameters.AddWithValue("@ID", oAction.ID);
				cmd.Parameters.AddWithValue("@GuidSessionID", oAction.WorkingSessionID);
				cmd.Parameters.AddWithValue("@ModuleID", oAction.ModuleID);
				cmd.Parameters.AddWithValue("@TypeID", oAction.Type);
				cmd.Parameters.AddWithValue("@PersonID", oAction.PersonID);
				cmd.Parameters.AddWithValue("@Data", oAction.ActionDate);
				cmd.Parameters.AddWithValue("@CommunityID", oAction.CommunityID);
				cmd.Parameters.AddWithValue("@IteractionType", oAction.Interaction);
                cmd.Parameters.AddWithValue("@Day", oAction.ActionDate.Day );
                cmd.Parameters.AddWithValue("@Month", oAction.ActionDate.Month );
                cmd.Parameters.AddWithValue("@Year", oAction.ActionDate.Year );
                cmd.Parameters.AddWithValue("@Hour", oAction.ActionDate.Hour);
                cmd.Parameters.AddWithValue("@PersonRoleID", oAction.PersonRoleID);

				cmd.ExecuteNonQuery();

				ObjectAction_Insert(oAction, conn);
                //ObjectAction_Insert(oAction, conn, tn);

				//tn.Commit();
				return true;
			 
			 }
			 catch (Exception ex) {
				//tn.Rollback();
				ErrorHandler pError = new ErrorHandler();
				pError.addMessageToPoisonQueue(oAction, ex);
				return false;
			 
			 }

		  }
        }
        public static void ObjectAction_Insert(UserAction oAction, SqlConnection conn)
        {
            foreach (ObjectAction obj in oAction.ObjectActions)
            {
                SqlCommand cmd = new SqlCommand("sp_ObjectAction_Insert", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserActionID", oAction.ID);
                cmd.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                cmd.Parameters.AddWithValue("@ObjectType", obj.ObjectTypeId);
                cmd.Parameters.AddWithValue("@Value", obj.ValueID);

                cmd.ExecuteNonQuery();
            }
        }
        public static void ObjectAction_Insert(UserAction oAction,SqlConnection conn,SqlTransaction tn)
        {
                foreach (ObjectAction  obj in oAction.ObjectActions)
                {
				SqlCommand cmd = new SqlCommand("sp_ObjectAction_Insert", conn);
				cmd.Transaction = tn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@UserActionID", oAction.ID);
                    cmd.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                    cmd.Parameters.AddWithValue("@ObjectType", obj.ObjectTypeId);
				cmd.Parameters.AddWithValue("@Value", obj.ValueID);

                    cmd.ExecuteNonQuery();      
                }               
        }
    }
}