using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using lm.Notification.DataContract.Domain;

namespace lm.Notification.DataContract.Service
{
    [ServiceContract]
    //[KnownType("GetKnownTypes")]
    public interface iManagementService
    {
        [OperationContract]

        #region "Management Templates"
        List<dtoTemplateMessage> AvailableTemplates(int ModuleID, int ActionID, dtoTemplateType tType);

        [OperationContract]
        long SaveTemplate(dtoTemplateMessage template);

        [OperationContract]
        Boolean RemoveTemplate(long templateID);

        [OperationContract]
        List<dtoModule> AvailableModules();
        # endregion

        #region "Management SystemNotification"
            #region "Remove SystemNotification"
            [OperationContract]
            Boolean RemoveNews(System.Guid NotificationID);

            [OperationContract]
            Boolean RemoveCommunityNews(int CommunityID);

            [OperationContract]
            Boolean RemoveUserNews(int PersonID);

            [OperationContract]
            Boolean RemoveUserNewsByCommunity(int PersonID, int CommunityID);
            #endregion

            #region "Remove News Summary"
            [OperationContract]
            Boolean RemoveNewsSummary(System.Guid NotificationID);

            [OperationContract]
            Boolean RemoveCommunityNewsSummary(int CommunityID);

            [OperationContract]
            Boolean RemoveUserNewsSummary(int PersonID);

            [OperationContract]
            Boolean RemoveUserNewsSummaryByCommunity(int PersonID, int CommunityID);
            #endregion


            #region "Community News"

            [OperationContract]
            List<dtoNewsInfo> GetPortalAllNewsInfo(int PersonID, DateTime FromDay);

            [OperationContract]
            int GetPortalAllNewsCount(int PersonID, DateTime FromDay);

            [OperationContract]
            List<dtoNewsInfo> GetCommunityAllNewsInfo(int PersonID, int CommunityID, DateTime FromDay);

            [OperationContract]
            int GetCommunityAllNewsCount(int PersonID, int CommunityID, DateTime FromDay);

            [OperationContract]
            List<CommunityNews> GetNotifications(List<System.Guid> notificationsID,int UserLanguageID, int DefaultLanguageID);


            [OperationContract]
            List<dtoModule> GetModulesWithNews(int PersonID, DateTime StartDay);

            [OperationContract]
            List<dtoModule> GetCommunityModulesWithNews(int PersonID, int CommunityID, DateTime StartDay);

            [OperationContract]
            List<dtoCommunitySummaryNotification> GetCommunitySummary(DateTime StartDay, int PersonID, int CommunityID, int LanguageID);


            [OperationContract]
            List<dtoCommunityWithNews> GetPersonalCommunityWithNews(int PersonID);

            [OperationContract(IsOneWay = true)]
            void UpdateCommunityNewsCount(dtoCommunityWithNews previous);

            [OperationContract]
            List<DateTime> GetWeekDaysWithNews(DateTime StartDay, int PersonID, int CommunityID);

            [OperationContract]
            List<DateTime> GetMonthDaysWithNews(DateTime StartDay, int PersonID, int CommunityID);

            [OperationContract]
            List<CommunityNews> GetCommunityNews(Boolean OnlySummary,DateTime StartDay, int PersonID, int CommunityID, int UserLanguageID, int DefaultLanguageID, int PageSize, int PageIndex);

            [OperationContract]
            int GetCommunityNewsCount(Boolean OnlySummary, DateTime StartDay, int PersonID, int CommunityID, int UserLanguageID, int DefaultLanguageID);

            [OperationContract]
            List<CommunityNews> GetPersonCommunityNews(Boolean OnlySummary, DateTime StartDay, int PersonID, int UserLanguageID, int DefaultLanguageID, int PageSize, int PageIndex);

            [OperationContract]
            int GetPersonCommunityNewsCount(Boolean OnlySummary, DateTime StartDay, int PersonID, int UserLanguageID, int DefaultLanguageID);
            #endregion

        #endregion







       


        //static Type[] GetKnownTypes()
        //{
        //    return new Type[]{typeof(lm.Notification.Core.Domain.NotificatedModule),
        //        typeof(lm.Notification.Core.Domain.ModuleAction)};
        //}
    }
}
