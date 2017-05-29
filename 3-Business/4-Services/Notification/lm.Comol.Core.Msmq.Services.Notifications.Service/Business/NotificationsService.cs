using lm.Comol.Core.Msmq.Services.Notifications.Service.Config;
using lm.Comol.Core.Msmq.Services.Notifications.Service.Exceptions;
using lm.Comol.Core.Notification.DataContract;
using lm.Comol.Core.Notification.Domain;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Notifications.Service
{
      // NOTE: If you change the class name "ErrorsNotificationService" here, you must also update the reference to "ErrorsNotificationService" in App.config.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class NotificationsService : iNotificationsManagerService, IDisposable
    {
        private static ServiceConfig Config = lm.Comol.Core.WinService.Configurations.Configuration<ServiceConfig>.Load(System.Configuration.ConfigurationManager.AppSettings["ServicePath"]);
        public void NotifyActionToModule(string istanceIdentifier, NotificationAction action, int idSenderUser = 0, String ipAddress = "", String proxyIpAddress = "")
        {
            IstanceConfig istance = Config.GetIstanceConfiguration(istanceIdentifier);
            if (istance != null)
            {
                List<GroupMessages> items = null;
                using (ISession session = lm.Comol.Core.Data.SessionDispatcher.NewSession(istance.ConnectionString))
                {
                    if (session != null)
                    {
                        using (InternalNotificationService service = new InternalNotificationService(istance, session))
                        {
                            try
                            {
                                items = service.NotifyActionToModule(action, idSenderUser,ipAddress,proxyIpAddress, istance.Settings);
                                if (items != null)
                                    service.ManageNotifications(items, action.ModuleCode, idSenderUser, ipAddress, proxyIpAddress);

                            }
                            catch (NotificationException nEx)
                            {
                                ErrorHandler.addActionToPoisonQueue(action, nEx);
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler.addActionToPoisonQueue(action, ExceptionType.GenericError,ex);
                            }
                        }
                    }
                    else
                        ErrorHandler.addActionToPoisonQueue(action, ExceptionType.UnableToGetNhibernateSession);
                }
            }
            else
                Exceptions.ErrorHandler.addActionToPoisonQueue(action, ExceptionType.ConfigMising);
        }
        public void NotifyAction(string istanceIdentifier, Notification.Domain.NotificationChannel channel, Notification.Domain.NotificationMode mode, NotificationAction action, int idSenderUser = 0, String ipAddress = "", String proxyIpAddress = "")
        {
            IstanceConfig istance = Config.GetIstanceConfiguration(istanceIdentifier);
            if (istance != null)
            {
                List<dtoModuleNotificationMessage> items = null;
                using (ISession session = lm.Comol.Core.Data.SessionDispatcher.NewSession(istance.ConnectionString))
                {
                    if (session != null)
                    {
                        using (InternalNotificationService service = new InternalNotificationService(istance, session))
                        {
                            try
                            {
                                items = service.NotifyActionToModule(channel,mode,action, idSenderUser,ipAddress,proxyIpAddress, istance.Settings);
                                if (items != null)
                                    service.ManageNotifications(items, action.ModuleCode, idSenderUser, ipAddress, proxyIpAddress);

                            }
                            catch (NotificationException nEx)
                            {
                                ErrorHandler.addActionToPoisonQueue(action, nEx);
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler.addActionToPoisonQueue(action, ExceptionType.GenericError,ex);
                            }
                        }
                    }
                    else
                        ErrorHandler.addActionToPoisonQueue(action, ExceptionType.UnableToGetNhibernateSession);
                }
            }
            else
                Exceptions.ErrorHandler.addActionToPoisonQueue(action, ExceptionType.ConfigMising);
        }

        public void Dispose()
        {
            Config = null;
        }

        //foreach (IstanceConfig c in Config.Istances)
        //{
        //    SrvMailSender.iServiceMailSender remoteMail = null;
        //    System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender> srv = null;
        //    foreach (ModuleConfig moduleConfig in c.MailConfiguration.Modules)
        //    {
        //        try
        //        {
        //            remoteMail = new SrvMailSender.iServiceMailSenderClient(moduleConfig.Binding, moduleConfig.Address);

        //        }
        //        catch (Exception ex)
        //        {
        //            if (remoteMail != null)
        //            {
        //                srv = (System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender>)remoteMail;
        //                srv.Abort();
        //                srv = null;
        //            }
        //        }
        //        if (remoteMail != null)
        //        {
        //            srv = (System.ServiceModel.ClientBase<SrvMailSender.iServiceMailSender>)remoteMail;
        //            srv.Close();
        //            srv = null;
        //        }
        //    }
        //}
    }
}