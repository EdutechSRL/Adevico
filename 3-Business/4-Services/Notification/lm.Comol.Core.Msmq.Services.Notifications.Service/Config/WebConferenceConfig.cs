using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Notifications.Service.Config
{
    [Serializable]
    public class WebConferenceConfig
    {
        public Boolean AllowRecord {get;set;}
        public Int32 RecordExpireAfterDay {get;set;}
        public Boolean AllowTrace {get;set;}
        public Int32 TraceExpireAfterDay { get; set; }
        public Boolean UseDataBase {get;set;}
        public Boolean UseProxy {get;set;}
        public String ProxyUrl {get;set;}
        public WebConferenceType CurrentType {get;set;}
        public eWorksSettings eWSettings {get;set;}
        public OpenMeetingSettings OMSettings { get; set; }


        public lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.eWSystemParameter GetEWorksParameters(){
            lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.eWSystemParameter parameters = new lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.eWSystemParameter();
            if (eWSettings!=null){
                parameters.BaseUrl = eWSettings.BaseUrl;
                parameters.CurrentSystem = Modules.Standard.WebConferencing.Domain.wBImplementedSystem.eWorks;
                parameters.MainUserId = eWSettings.MainUserId;
                parameters.MainUserPwd = eWSettings.MainUserPwd;
                parameters.MaxUrlChars = eWSettings.MaxUrlChar;
                //.MeetingsConfiguration = Nothing
                parameters.MeetingsExpirationDay = 0;
                parameters.ProxyUrl = ProxyUrl;
                parameters.RecordCan = AllowRecord;
                parameters.RecordExpirationDay = RecordExpireAfterDay;
                parameters.StatisticsCan = AllowTrace;
                parameters.StatisticsExpirationDay = TraceExpireAfterDay;
                parameters.UseDataBase = UseDataBase;
                parameters.UseProxy = UseProxy;
                parameters.Version = eWSettings.Version;
            }
            return parameters;
        }

        public lm.Comol.Modules.Standard.WebConferencing.Domain.OpenMeetings.oMSystemParameter GetOpenMeetingParameters()
        {
            lm.Comol.Modules.Standard.WebConferencing.Domain.OpenMeetings.oMSystemParameter parameters = new lm.Comol.Modules.Standard.WebConferencing.Domain.OpenMeetings.oMSystemParameter();
            if (eWSettings!=null){
                parameters.BaseUrl = OMSettings.BaseUrl;
                parameters.MainUserLogin = OMSettings.MainUserLogin;
                parameters.MainUserPwd = OMSettings.MainUserPwd;
                parameters.CurrentSystem= Modules.Standard.WebConferencing.Domain.wBImplementedSystem.OpenMeetings;
                //parameters.MeetingsExpirationDay = 0
                parameters.RecordCan = AllowRecord;
                parameters.RecordExpirationDay = RecordExpireAfterDay;
                parameters.StatisticsCan = AllowTrace;
                parameters.StatisticsExpirationDay = TraceExpireAfterDay;
                parameters.UseDataBase = UseDataBase;
            }
            return parameters;
        }
 
    }

    [Serializable]
    public class eWorksSettings
    {
        public String BaseUrl {get;set;}
        public String MainUserId {get;set;}
        public String MainUserPwd {get;set;}
        public Int32 MaxUrlChar {get;set;}
        public String Version {get;set;}
    }

    [Serializable]
    public class OpenMeetingSettings
    {
        public String BaseUrl {get;set;}
        public String MainUserLogin {get;set;}
        public String MainUserPwd {get;set;}
    }

    [Serializable]
    public enum WebConferenceType
    {
        none = 0,
        eWorks = 1,
        OpenMeeting = 2
    }
}
