using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using lm.ActionDataContract;
using System.Configuration;

namespace lm.ActionPersistence
{
    public class DALBrowserInfo
    {

        public static bool BrowserInfo_Insert(BrowserInfo oBrowserInfo)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString")))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_BrowserInfo_Insert", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GuidSessionID", oBrowserInfo.WorkingSessionID);
                    cmd.Parameters.AddWithValue("@BrowserType", oBrowserInfo.BrowserType);
                    cmd.Parameters.AddWithValue("@Version", oBrowserInfo.Version);
                    cmd.Parameters.AddWithValue("@Language", oBrowserInfo.Language);
                    cmd.Parameters.AddWithValue("@Frames", oBrowserInfo.Frames);
                    cmd.Parameters.AddWithValue("@Tables", oBrowserInfo.Tables);
                    cmd.Parameters.AddWithValue("@Cookies", oBrowserInfo.Cookies);
                    cmd.Parameters.AddWithValue("@JavaApplets", oBrowserInfo.JavaApplets);
                    cmd.Parameters.AddWithValue("@JScriptVersion", oBrowserInfo.JScriptVersion);
                    cmd.Parameters.AddWithValue("@ActiveXControls", oBrowserInfo.ActiveXControls);
                    cmd.Parameters.AddWithValue("@IndirizzoIP", oBrowserInfo.ClientIPAdress);
                    cmd.Parameters.AddWithValue("@ProxyIP", oBrowserInfo.ProxyIPAdress);
                    cmd.Parameters.AddWithValue("@Platform", oBrowserInfo.Platform);
                    cmd.Parameters.AddWithValue("@ScreenPixelsHeight", oBrowserInfo.ScreenPixelsHeight);
                    cmd.Parameters.AddWithValue("@ScreenCharactersWidth", oBrowserInfo.ScreenCharactersWidth);
                    cmd.Parameters.AddWithValue("@IsMobileDevice", oBrowserInfo.IsMobileDevice);
                    cmd.Parameters.AddWithValue("@CanInitiateVoiceCall", oBrowserInfo.CanInitiateVoiceCall);
                    cmd.Parameters.AddWithValue("@W3CDomVersion", oBrowserInfo.W3CDomVersion);
                    cmd.Parameters.AddWithValue("@PersonID", oBrowserInfo.PersonID);
                    cmd.Parameters.AddWithValue("@PersonTypeID", oBrowserInfo.PersonTypeID);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }

                catch (Exception ex)
                {
                    if (conn.State == System.Data.ConnectionState.Open) {
                        conn.Close();
                        conn.Dispose();
                    }
                    ErrorHandler pError = new ErrorHandler();
                    pError.addMessageToPoisonQueue(oBrowserInfo, ex);
                    return false;
                }
            }
           
        }
    }
}
