using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Msmq.Services.Notifications.Service.Config;
using lm.Comol.Core.Notification.Domain;
using NHibernate;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
namespace lm.Comol.Core.Msmq.Services.Notifications.Service
{
	public class InternalNotificationService : IDisposable 
	{
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region "Configuration Settings"
            private IstanceConfig IstanceConfig { get; set; }
            private ISession Session { get; set; }
            private lm.Comol.Core.Business.BaseModuleManager manager { get; set; }
            private lm.Comol.Core.Business.BaseModuleManager Manager { 
                get {
                    if (manager==null)
                        manager = new Core.Business.BaseModuleManager(new lm.Comol.Core.Data.DataContext(Session));
                    return manager;
                } 
            }
        #endregion

        public InternalNotificationService(IstanceConfig istance, ISession session)
        {
            Session = session;
            IstanceConfig = istance;
        }

        #region "Manage Notifications"
            public List<GroupMessages> NotifyActionToModule(NotificationAction action, Int32 idSenderUser, String ipAddress, String proxyIpAddress, Notification.Domain.WebSiteSettings webSiteSettings)
            {
                List<GroupMessages> results = null;
                lm.Comol.Core.Notification.Domain.iNotifiableService service = null;
                lm.Comol.Core.Data.DataContext dc = new lm.Comol.Core.Data.DataContext(Session);
                switch (action.ModuleCode)
                {
                    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                        service = new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                        break;

                    case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode:
                        if (IstanceConfig.WebConference != null){
                            switch(IstanceConfig.WebConference.CurrentType){
                                case WebConferenceType.eWorks:
                                    service= new lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.eWService(IstanceConfig.WebConference.GetEWorksParameters(), dc);
                                    break;
                                case WebConferenceType.OpenMeeting:
                                    service= new lm.Comol.Modules.Standard.WebConferencing.Domain.OpenMeetings.oMService(IstanceConfig.WebConference.GetOpenMeetingParameters(), dc);
                                    break;
                            }
                        }
                        break;

                    case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode:
                        //service = new lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(dc);
                        break;
                    case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode:
                        //service = new lm.Comol.Modules.CallForPapers.Business.ServiceRequestForMembership(dc);
                        break;
                    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                         service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                         break;
                     case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID:
                        //service = new lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(dc);
                        break;
                    case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode:
                           ///service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                        break;
                }
                if (service != null)
                    results = service.GetDefaultNotificationMessages(action,idSenderUser, webSiteSettings);
                return results;
            }
            public List<dtoModuleNotificationMessage> NotifyActionToModule(Notification.Domain.NotificationChannel channel, Notification.Domain.NotificationMode mode,NotificationAction action, Int32 idSenderUser, String ipAddress, String proxyIpAddress, Notification.Domain.WebSiteSettings webSiteSettings)
            {
                List<dtoModuleNotificationMessage> results = null;
                lm.Comol.Core.Notification.Domain.iNotifiableService service = null;
                lm.Comol.Core.Data.DataContext dc = new lm.Comol.Core.Data.DataContext(Session);
                switch (action.ModuleCode)
                {
                    case lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode:
                        service = (lm.Comol.Core.Notification.Domain.iNotifiableService)new lm.Comol.Core.BaseModules.Tickets.TicketService(dc);
                        break;

                    case lm.Comol.Modules.Standard.WebConferencing.Domain.ModuleWebConferencing.UniqueCode:
                        if (IstanceConfig.WebConference != null){
                            switch(IstanceConfig.WebConference.CurrentType){
                                case WebConferenceType.eWorks:
                                    service= new lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks.eWService(IstanceConfig.WebConference.GetEWorksParameters(), dc);
                                    break;
                                case WebConferenceType.OpenMeeting:
                                    service= new lm.Comol.Modules.Standard.WebConferencing.Domain.OpenMeetings.oMService(IstanceConfig.WebConference.GetOpenMeetingParameters(), dc);
                                    break;
                            }
                        }
                        break;

                    case lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.UniqueCode:
                        //service = new lm.Comol.Modules.CallForPapers.Business.ServiceCallOfPapers(dc);
                        break;
                    case lm.Comol.Modules.CallForPapers.Domain.ModuleRequestForMembership.UniqueCode:
                        //service = new lm.Comol.Modules.CallForPapers.Business.ServiceRequestForMembership(dc);
                        break;
                    case lm.Comol.Modules.Standard.ProjectManagement.Domain.ModuleProjectManagement.UniqueCode:
                        //service = new lm.Comol.Modules.Standard.ProjectManagement.Business.ServiceProjectManagement(dc);
                         break;
                     case lm.Comol.Core.BaseModules.CommunityDiary.Domain.ModuleCommunityDiary.UniqueID:
                        //service = new lm.Comol.Core.BaseModules.CommunityDiary.Business.ServiceCommunityDiary(dc);
                        break;
                    case lm.Comol.Modules.EduPath.Domain.ModuleEduPath.UniqueCode:
                           ///service = new lm.Comol.Modules.EduPath.BusinessLogic.Service(dc);
                        break;
                }
                if (service != null)
                    results = service.GetNotificationMessages(action, channel, mode,idSenderUser, webSiteSettings);
                return results;
            }



            public void ManageNotifications(List<dtoModuleNotificationMessage> messages, String moduleCode, Int32 idSenderUser, String ipAddress, String proxyIpAddress)
            {
                foreach (NotificationChannel channel in messages.Select(i => i.Channel).Distinct())
                {
                    switch (channel)
                    {
                        case NotificationChannel.Mail:
                            SrvMailSender.iServiceMailSender service = GetMailServiceSender(GetMailQueueConfiguration(moduleCode));
                            if (service != null)
                            {
                                System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender> srv = null;
                                try
                                {
                                    foreach(dtoModuleNotificationMessage message in messages){
                                        service.SendMailFromModuleNotification(IstanceConfig.UniqueIdentifier,idSenderUser, moduleCode, message);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Log(LogLevel.Error, "Error on GetMailServiceSender", ex);
                                    if (service != null)
                                    {
                                        srv = (System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender>)service;
                                        srv.Abort();
                                        srv = null;
                                    }
                                    service = null;
                                    throw new lm.Comol.Core.Msmq.Services.Notifications.Service.Exceptions.NotificationException(ex, Exceptions.ExceptionType.RemoteQueueUnavailable);
                                }
                                finally{
                                    if (service != null)
                                    {
                                        srv = (System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender>)service;
                                        srv.Close();
                                        srv = null;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            public void ManageNotifications(List<GroupMessages> messages, String moduleCode, Int32 idSenderUser, String ipAddress, String proxyIpAddress)
            {
                foreach (NotificationChannel channel in messages.Select(i => i.Channel).Distinct())
                {
                    switch (channel)
                    {
                        case NotificationChannel.Mail:
                            SrvMailSender.iServiceMailSender service = GetMailServiceSender(GetMailQueueConfiguration(moduleCode));
                            if (service != null)
                            {
                                System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender> srv = null;
                                try
                                {
                                    foreach (GroupMessages message in messages)
                                    {
                                        service.SendMailFromGroupNotification(IstanceConfig.UniqueIdentifier, idSenderUser, moduleCode, message);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Log(LogLevel.Error, "Error on GetMailServiceSender", ex);
                                    if (service != null)
                                    {
                                        srv = (System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender>)service;
                                        srv.Abort();
                                        srv = null;
                                    }
                                    service = null;
                                    throw new lm.Comol.Core.Msmq.Services.Notifications.Service.Exceptions.NotificationException(ex, Exceptions.ExceptionType.RemoteQueueUnavailable);
                                }
                                finally{
                                    if (service != null)
                                    {
                                        srv = (System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender>)service;
                                        srv.Close();
                                        srv = null;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        
        #endregion

        #region "Mail Service Settings"
            private MailConfig GetMailQueueConfiguration(String code)
            {
                MailConfig dConfig = null;
                if (IstanceConfig!=null){
                    dConfig = IstanceConfig.GetMailConfig(code);
                    if (dConfig!=null && dConfig.TryToLoadFromDB){

                    }
                }
                return dConfig;
            }

            public SrvMailSender.iServiceMailSender GetMailServiceSender(MailConfig config){
                SrvMailSender.iServiceMailSender remoteService = null;
                System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender> srv = null;
                try
                {
                    remoteService = new SrvMailSender.iServiceMailSenderClient(GetBinding(config.Binding), new System.ServiceModel.EndpointAddress (config.Address));
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Error, "Error on GetMailServiceSender", ex);
                    if (remoteService != null)
                    {
                        srv = (System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender>)remoteService;
                        srv.Abort();
                        srv = null;
                    }
                    remoteService = null;
                    throw new lm.Comol.Core.Msmq.Services.Notifications.Service.Exceptions.NotificationException(ex, Exceptions.ExceptionType.RemoteQueueUnavailable);
                }
                return remoteService;
            }
            private System.ServiceModel.Channels.Binding GetBinding(String name)
            {
                System.ServiceModel.NetMsmqBinding binding = new System.ServiceModel.NetMsmqBinding();
                binding.Security.Mode = System.ServiceModel.NetMsmqSecurityMode.None;
                binding.Security.Transport.MsmqAuthenticationMode = System.ServiceModel.MsmqAuthenticationMode.None;

                //TimeSpan span = new TimeSpan(0, 3, 0);

                //binding.Name = "NoConfigBinding";
                //binding.CloseTimeout = span;
                //binding.OpenTimeout = span;
                //binding.ReceiveTimeout = span;
                //binding.SendTimeout = span;
                //binding.AllowCookies = false;
                //binding.BypassProxyOnLocal = false;
                //binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                //binding.MaxBufferSize = 65536;
                //binding.MaxBufferPoolSize = 524288;
                //binding.MaxReceivedMessageSize = 65536;
                //binding.MessageEncoding = WSMessageEncoding.Text;
                //binding.TextEncoding = Encoding.UTF8;
                //binding.TransferMode = TransferMode.Buffered;
                //binding.UseDefaultWebProxy = true;

                //binding.ReaderQuotas = new XmlDictionaryReaderQuotas();
                //binding.ReaderQuotas.MaxDepth = 32;
                //binding.ReaderQuotas.MaxStringContentLength = 8192;
                //binding.ReaderQuotas.MaxArrayLength = 16384;
                //binding.ReaderQuotas.MaxBytesPerRead = 4096;
                //binding.ReaderQuotas.MaxNameTableCharCount = 16384;

                //binding.Security.Mode = BasicHttpSecurityMode.None;

                //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                //binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                //binding.Security.Transport.Realm = string.Empty;

                //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
                //binding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;

                return binding;
            }
   
        #endregion
        public void Dispose()
        {
            manager = null;
            Session.Dispose();
        }
    }
}