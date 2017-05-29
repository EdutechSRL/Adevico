using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using lm.ErrorsNotification.DataContract.Service;
using lm.ErrorsNotification.DataContract.Domain;
using lm.ErrorsNotification.Service.Dal;

namespace lm.ErrorsNotification.Service
{
    // NOTE: If you change the class name "ErrorsNotificationService" here, you must also update the reference to "ErrorsNotificationService" in App.config.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ErrorsNotificationService : iErrorsNotificationService, IDisposable
    {

        private String _CacheKeyMailTemplate = "MailTemplate";
        //private String _CacheKeyMailNotificationSettings = "MailNotificationSettings";
        private String _CacheKeyErrorSettings = "ErrorSettings";
        private Dictionary<string, CachedItem<List<MailTemplate>>> _CacheTemplates;
        //private Dictionary<string, CachedItem<List<MailNotificationSettings>>> _CacheMailSettings;
        private Dictionary<string, CachedItem<List<ErrorSettings>>> _CacheErrorSettings;


        private IManagerErrors GetManager(PersistTo type, String ComolUniqueID,ErrorType errorType)
        {
            IManagerErrors manager = null;
            switch (type)
            {
                case PersistTo.Mail:
                    MailTemplate template = (from t in GetCachedTemplates() where t.Type == errorType select t).FirstOrDefault<MailTemplate>();
                    ErrorSettings setting = (from s in GetCachedErrorSettings() where s.ComolUniqueID == ComolUniqueID select s).FirstOrDefault<ErrorSettings>();
                    manager = new ManagerMail(template, setting);
                    break;
                case PersistTo.File:
                    manager = new ManagerFile();
                    break;
                case PersistTo.Database:
                    System.Diagnostics.EventLog.WriteEntry("DEBUG", "PersistTo.Database");
                    manager = new ManagerDatabase();
                    break;
            }
            return manager;
        }

        public ErrorsNotificationService() {
            _CacheTemplates = new Dictionary<string, CachedItem<List<MailTemplate>>>();
            _CacheErrorSettings = new Dictionary<string, CachedItem<List<ErrorSettings>>>();
        }

                #region IDisposable Members

        public void Dispose()
        {
         //   throw new NotImplementedException();
        }

        #endregion

        #region iErrorsNotificationService Members

        void iErrorsNotificationService.sendCommunityModuleError(CommunityModuleError error)
        {
            try
            {
                IManagerErrors manager = GetManager(error.Persist, error.ComolUniqueID, error.Type);
                manager.SaveCommunityModuleError(error);
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(error, ex);
            }
        }

        void iErrorsNotificationService.sendDBerror(DBerror error)
        {
            try
            {
                IManagerErrors manager = GetManager(error.Persist, error.ComolUniqueID, error.Type);
                manager.SaveDBerror(error);

            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(error, ex);
            }
        }

        void iErrorsNotificationService.sendGenericError(GenericError error)
        {
            try
            {
                IManagerErrors manager = GetManager(error.Persist, error.ComolUniqueID, error.Type);
                manager.SaveGenericError(error);

            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(error, ex);
            }
        }

        void iErrorsNotificationService.sendGenericModuleError(GenericModuleError error)
        {
            try
            {
                IManagerErrors manager = GetManager(error.Persist, error.ComolUniqueID, error.Type);
                manager.SaveGenericModuleError(error);

            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(error, ex);
            }
        }

        void iErrorsNotificationService.sendGenericWebError(GenericWebError error)
        {
            try
            {
                IManagerErrors manager = GetManager(error.Persist, error.ComolUniqueID, error.Type);
                manager.SaveGenericWebError(error);
            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(error, ex);
            }
        }

        void iErrorsNotificationService.sendFileError(FileError error)
        {
            try
            {
                IManagerErrors manager = GetManager(error.Persist, error.ComolUniqueID, error.Type);
                manager.SaveFileError(error);

            }
            catch (Exception ex)
            {
                ErrorHandler pError = new ErrorHandler();
                pError.addMessageToPoisonQueue(error, ex);
            }
        }
        #endregion



        List<MailTemplate> GetCachedTemplates()
        {
            List<MailTemplate> templates = GetTemplatesFromCache();
            if (templates==null)
            {
                GenericManager manager = new GenericManager();
                templates = manager.GetTemplates();
                try{
                     _CacheTemplates.Add(_CacheKeyMailTemplate, new CachedItem<List<MailTemplate>>(templates));
                }
                catch (ArgumentException ex)
                {
                    templates = GetTemplatesFromCache();
                }
            }

            return templates;
        }
        private List<MailTemplate> GetTemplatesFromCache()
        {
            List<MailTemplate> templates;
            CachedItem<List<MailTemplate>> cached;
            DateTime DataRead = DateTime.Now.AddMinutes(-15);
            if ((_CacheTemplates.TryGetValue(_CacheKeyMailTemplate, out cached)))
            {
                if (cached == null || cached.InsertedDate < DataRead)
                {
                    GenericManager manager = new GenericManager();
                    templates = manager.GetTemplates();
                    _CacheTemplates[_CacheKeyMailTemplate] = new CachedItem<List<MailTemplate>>(templates);
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

        //List<MailNotificationSettings> GetCachedMailSettings()
        //{
        //    List<MailNotificationSettings> settings;
        //    CachedItem<List<MailNotificationSettings>> cached;
        //    DateTime DataRead = DateTime.Now.AddMinutes(-15);
        //    if ((_CacheMailSettings.TryGetValue(_CacheKeyMailNotificationSettings, out cached)))
        //    {
        //        if (cached == null || cached.InsertedDate < DataRead)
        //        {
        //            GenericManager manager = new GenericManager();
        //            settings = manager.GetMailNotificationSettings();
        //            _CacheMailSettings[_CacheKeyMailNotificationSettings] = new CachedItem<List<MailNotificationSettings>>(settings);
        //        }
        //        else
        //        {
        //            settings = cached.Item;
        //        }
        //    }
        //    else
        //    {
        //        GenericManager manager = new GenericManager();
        //        settings = manager.GetMailNotificationSettings();
        //        _CacheMailSettings.Add(_CacheKeyMailNotificationSettings, new CachedItem<List<MailNotificationSettings>>(settings));
        //    }

        //    return settings;
        //}
        List<ErrorSettings> GetCachedErrorSettings()
        {
            List<ErrorSettings> settings = GetErrorSettingsFromCache();

            if (settings==null){
                GenericManager manager = new GenericManager();
                settings = manager.GetErrorSettings();
                try
                {
                    _CacheErrorSettings.Add(_CacheKeyErrorSettings, new CachedItem<List<ErrorSettings>>(settings));
                }
                catch(ArgumentException ex) {
                    settings = GetErrorSettingsFromCache();
                }
            }

            return settings;
        }
        private List<ErrorSettings> GetErrorSettingsFromCache()
        {
            List<ErrorSettings> settings;
            CachedItem<List<ErrorSettings>> cached;
            if ((_CacheErrorSettings.TryGetValue(_CacheKeyErrorSettings, out cached)))
            {
                DateTime DataRead = DateTime.Now.AddMinutes(-15);
                if (cached == null || cached.InsertedDate < DataRead)
                {
                    GenericManager manager = new GenericManager();
                    settings = manager.GetErrorSettings();
                    _CacheErrorSettings[_CacheKeyErrorSettings] = new CachedItem<List<ErrorSettings>>(settings);
                }
                else
                {
                    settings = cached.Item;
                }
            }
            else
            {
                settings = null;
            }

            return settings;
        
        }
    }
}
