using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Notification.DataContract;
using lm.Notification.DataContract.Service;
using lm.Notification.Service.DAL;
using lm.Notification.DataContract.Domain;
using lm.Notification.Core.Domain;
using lm.Notification.Core.DataLayer;
using NHibernate;
using System.ServiceModel;

namespace lm.Notification.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class NotificationService : iNotificationService, IDisposable
    {
        private ISession session;

        private String _CacheKeyCommunity = "community_{0}";
        private String _CacheKeyUserByCommunityRole = "communityByRoles_{0}_{1}";
        private String _CacheKeyPermission = "permission_{0}_{1}_{2}";
        private String _CacheKeyTemplates = "templates";
        private Dictionary<string, CachedItem<List<int>>> _CacheUsers;
        private Dictionary<string, CachedItem<List<int>>> _CacheUsersRoles;
        private Dictionary<string, CachedItem<List<TemplateMessage>>> _CacheTemplates;
        private SRVcoreCommunityService.CommunityServicesSoapClient _CoreService;
        private SRVcoreCommunityService.CommunityServicesSoapClient CommunityService()
        {
            if (_CoreService == null)
                _CoreService = new SRVcoreCommunityService.CommunityServicesSoapClient();
            return _CoreService;
        }
        // private DataContext dc;

        public NotificationService()
        {
            _CacheUsers = new Dictionary<string, CachedItem<List<int>>>();
            _CacheUsersRoles = new Dictionary<string, CachedItem<List<int>>>();
            _CacheTemplates = new Dictionary<string, CachedItem<List<TemplateMessage>>>();
            _CacheTemplates.Add(_CacheKeyTemplates, new CachedItem<List<TemplateMessage>>(GetTemplatesFromRepository()));
        }

        #region iNotificationService Members

        public void NotifyToCommunity(NotificationToCommunity NotificationCommunity)
        {
            try
            {

                List<int> DestinationPersons = GetUsersByCommunity(NotificationCommunity.CommunityID);//CommunityService().GetCommunityMembersID(NotificationCommunity.CommunityID).ToList<int>();

                using (ISession currentSession = BasicSessionMgr.GetSession())
                {
                    if (currentSession != null)
                    {
                        NotificationRepository oDal = new NotificationRepository(currentSession, GetCachedTemplates());
                        oDal.AddNotificationMessage(NotificationCommunity, DestinationPersons);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(NotificationCommunity, ex);
            }

        }

        public void NotifyToUsers(NotificationToPerson Notification)
        {
            try
            {
                using (ISession currentSession = BasicSessionMgr.GetSession())
                {
                    if (currentSession != null)
                    {
                        NotificationRepository oDal = new NotificationRepository(currentSession, GetCachedTemplates());
                        oDal.AddNotificationMessage(Notification, Notification.PersonsID);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(Notification, ex);
            }

        }

        public void NotifyForPermission(NotificationToPermission Notification)
        {
            try
            {
                List<int> DestinationPersons = GetUsersByPermission(Notification.CommunityID, Notification.ModuleID, Notification.Permission);
                using (ISession currentSession = BasicSessionMgr.GetSession())
                {
                    if (currentSession != null)
                    {
                        NotificationRepository oDal = new NotificationRepository(currentSession, GetCachedTemplates());
                        oDal.AddNotificationMessage(Notification, DestinationPersons);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(Notification, ex);
            }

        }

        public void NotifyForRoles(NotificationToRole Notification)
        {
            try
            {
                List<int> DestinationPersons = GetUsersByRole(Notification.CommunityID, Notification.RolesID);
                using (ISession currentSession = BasicSessionMgr.GetSession())
                {
                    if (currentSession != null)
                    {
                        NotificationRepository oDal = new NotificationRepository(currentSession, GetCachedTemplates());
                        oDal.AddNotificationMessage(Notification, DestinationPersons);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(Notification, ex);
            }

        }

        public void NotifyForPermissionItemGuid(NotificationToItemGuid Notification)
        {
            try
            {
                List<int> DestinationPersons = CommunityService().GetItemGuidMembersID(Notification.CommunityID, Notification.ModuleCode, Notification.ItemID, Notification.ObjectTypeID, Notification.Permission).ToList<int>();
                using (ISession currentSession = BasicSessionMgr.GetSession())
                {
                    if (currentSession != null)
                    {
                        NotificationRepository oDal = new NotificationRepository(currentSession, GetCachedTemplates());
                        oDal.AddNotificationMessage(Notification, DestinationPersons);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(Notification, ex);
            }
        }
        public void NotifyForPermissionItemLong(NotificationToItemLong Notification)
        {
            try
            {
                List<int> DestinationPersons = CommunityService().GetItemLongMembersID(Notification.CommunityID, Notification.ModuleCode, Notification.ItemID, Notification.ObjectTypeID, Notification.Permission).ToList<int>();
                using (ISession currentSession = BasicSessionMgr.GetSession())
                {
                    if (currentSession != null)
                    {
                        NotificationRepository oDal = new NotificationRepository(currentSession, GetCachedTemplates());
                        oDal.AddNotificationMessage(Notification, DestinationPersons);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(Notification, ex);
            }
        }
        public void NotifyForPermissionItemVersionLong(NotificationToItemVersionLong Notification)
        {
            try
            {
                List<int> DestinationPersons = CommunityService().GetItemVersionLongMembersID(Notification.CommunityID, Notification.ModuleCode, Notification.IdItem, Notification.IdVersion, Notification.ObjectTypeID, Notification.Permission).ToList<int>();
                using (ISession currentSession = BasicSessionMgr.GetSession())
                {
                    if (currentSession != null)
                    {
                        NotificationRepository oDal = new NotificationRepository(currentSession, GetCachedTemplates());
                        oDal.AddNotificationMessage(Notification, DestinationPersons);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(Notification, ex);
            }
        }
        
        public void NotifyForPermissionItemInt(NotificationToItemInt Notification)
        {
            try
            {
                List<int> DestinationPersons = CommunityService().GetItemIntMembersID(Notification.CommunityID, Notification.ModuleCode, Notification.ItemID, Notification.ObjectTypeID, Notification.Permission).ToList<int>();
                using (ISession currentSession = BasicSessionMgr.GetSession())
                {
                    if (currentSession != null)
                    {
                        NotificationRepository oDal = new NotificationRepository(currentSession, GetCachedTemplates());
                        oDal.AddNotificationMessage(Notification, DestinationPersons);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(Notification, ex);
            }
        }

        public void RemoveNotification(Guid NotificationID)
        {
            try
            {
                NotificationToPersonRepository.RemoveNotification(NotificationID);
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
            }
        }
        public void RemoveNotificationForUser(Guid NotificationID, int PersonID)
        {
            try
            {
                NotificationToPersonRepository.RemoveNotificationForUser(NotificationID, PersonID);
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
            }
        }
        public void RemoveUserNotification(Guid UserNotificationID, int PersonID)
        {
            try
            {
                NotificationToPersonRepository.RemoveUserNotification(UserNotificationID, PersonID);
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
            }
        }
        public void ReadNotification(Guid NotificationID, int PersonID)
        {
            try
            {
                NotificationToPersonRepository.ReadNotification(NotificationID, PersonID);
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
            }
        }
        public void ReadUserNotification(Guid UserNotificationID, int PersonID)
        {
            try{
                NotificationToPersonRepository.ReadUserNotification(UserNotificationID, PersonID);
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
            }
        }

        public void ReadUserCommunityNotification(int CommunityID, int PersonID)
        {
            try
            {
                NotificationToPersonRepository.ReadUserCommunityNotification(CommunityID, PersonID);
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
            }
        }
        public void RemoveUserCommunityNotification(int CommunityID, int PersonID)
        {
            try
            {
                NotificationToPersonRepository.RemoveUserCommunityNotification(CommunityID, PersonID);
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (session != null)
            {
                if (session.IsOpen)
                    session.Close();
                session.Dispose();
            }
        }

        #endregion


        List<int> GetUsersByPermission(int CommunityID, int ModuleID, int Permission)
        {
            List<int> DestinationPersons;
            String Key = string.Format(_CacheKeyPermission, CommunityID, ModuleID, Permission);
            DateTime DataRead = DateTime.Now.AddMinutes(-15);
            CachedItem<List<int>> cached;
            if ((_CacheUsers.TryGetValue(Key, out cached)))
            {
                if (cached == null || cached.InsertedDate < DataRead)
                {
                    DestinationPersons = CommunityService().GetPermissionMembersID(CommunityID, ModuleID, Permission).ToList<int>();
                    _CacheUsers[Key] = new CachedItem<List<int>>(DestinationPersons);
                }
                else
                {
                    DestinationPersons = cached.Item;
                }
            }
            else
            {
                DestinationPersons = CommunityService().GetPermissionMembersID(CommunityID, ModuleID, Permission).ToList<int>();
                try
                {
                    _CacheUsers.Add(Key, new CachedItem<List<int>>(DestinationPersons));
                }
                catch (Exception ex)
                {
                    _CacheUsers[Key] = new CachedItem<List<int>>(DestinationPersons);
                }
            }

            return DestinationPersons;
        }
        List<int> GetUsersByCommunity(int CommunityID)
        {
            List<int> DestinationPersons;
            CachedItem<List<int>> cached;
            String Key = string.Format(_CacheKeyCommunity, CommunityID);
            DateTime DataRead = DateTime.Now.AddMinutes(-15);
            if ((_CacheUsers.TryGetValue(Key, out cached)))
            {
                if (cached == null || cached.InsertedDate < DataRead)
                {
                    DestinationPersons = CommunityService().GetCommunityMembersID(CommunityID).ToList<int>();
                    _CacheUsers[Key] = new CachedItem<List<int>>(DestinationPersons);
                }
                else
                {
                    DestinationPersons = cached.Item;
                }

            }
            else
            {
                DestinationPersons = CommunityService().GetCommunityMembersID(CommunityID).ToList<int>();
                _CacheUsers.Add(Key, new CachedItem<List<int>>(DestinationPersons));
            }

            return DestinationPersons;
        }

        #region "UserByRole"
            List<int> GetUsersByRole(int CommunityID, List <int> RolesID){
                List<int> DestinationPersons= new List<int>();
                foreach (int RoleID in RolesID) {
                    List<int> usersRole = GetUsersByCommunityRolesFromCache(CommunityID, RoleID);
                    if (usersRole == null)
                    {
                        usersRole = CommunityService().GetCommunityRoleMembersID(CommunityID, RoleID).ToList<int>();
                        try
                        {
                            String Key = string.Format(_CacheKeyUserByCommunityRole, CommunityID, RoleID);
                            _CacheUsersRoles.Add(Key, new CachedItem<List<int>>(usersRole));
                        }
                        catch (ArgumentException ex)
                        {
                            usersRole = GetUsersByCommunityRolesFromCache(CommunityID, RoleID);
                        }
                    }
                    DestinationPersons.AddRange(usersRole);
                }
                return DestinationPersons;
        }

            private List<int> GetUsersByCommunityRolesFromCache(int CommunityID, int RoleID)
            {
                List<int> DestinationPersons;
                CachedItem<List<int>> cached;
                String Key = string.Format(_CacheKeyUserByCommunityRole, CommunityID, RoleID);

                if ((_CacheUsersRoles.TryGetValue(Key, out cached)))
                {
                    DateTime DataRead = DateTime.Now.AddMinutes(-5);
                    if (cached == null || cached.InsertedDate < DataRead)
                    {
                        DestinationPersons = CommunityService().GetCommunityRoleMembersID(CommunityID, RoleID).ToList<int>();
                        _CacheUsersRoles[Key] = new CachedItem<List<int>>(DestinationPersons);
                    }
                    else
                    {
                        DestinationPersons = cached.Item;
                    }
                }
                else
                {
                    DestinationPersons = null;
                }

                return DestinationPersons;

            }
        #endregion

        #region "Template"
        List<TemplateMessage> GetCachedTemplates()
        {
            List<TemplateMessage> templates = GetTemplatesFromCache();

            if (templates == null)
            {
                templates = GetTemplatesFromRepository();
                try
                {
                    _CacheTemplates.Add(_CacheKeyTemplates, new CachedItem<List<TemplateMessage>>(templates));
                }
                catch (ArgumentException ex)
                {
                    templates = GetTemplatesFromCache();
                }
            }

            return templates;
        }
        private List<TemplateMessage> GetTemplatesFromCache()
        {
            List<TemplateMessage> templates;
            CachedItem<List<TemplateMessage>> cached;

            if ((_CacheTemplates.TryGetValue(_CacheKeyTemplates, out cached)))
            {
                DateTime DataRead = DateTime.Now.AddMinutes(-15);
                if (cached == null || cached.InsertedDate < DataRead)
                {
                    templates = GetTemplatesFromRepository();
                    _CacheTemplates[_CacheKeyTemplates] = new CachedItem<List<TemplateMessage>>(templates);
                }
                else
                {
                    templates = cached.Item;
                }
            }
            else
            {
                templates = null;
            }

            return templates;

        }
        List<TemplateMessage> GetTemplatesFromRepository()
        {
            List<TemplateMessage> templates = new List<TemplateMessage>();
            using (ISession currentSession = BasicSessionMgr.GetSession())
            {
                if (currentSession != null)
                {
                    NotificationRepository oDal = new NotificationRepository(currentSession);
                    templates = oDal.PreLoadTemplates();
                }
            }
            return templates;
        }
        #endregion
    }
}