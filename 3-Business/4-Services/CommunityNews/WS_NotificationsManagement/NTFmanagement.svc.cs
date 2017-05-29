using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using lm.Notification.Core.Domain;
using lm.Notification.Core.DataLayer;
using lm.Notification.DataContract.Service;
using NHibernate;
using NHibernate.Linq;
using Helpers;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using lm.Notification.DataContract.Domain;

namespace WS_NotificationsManagement
{
    // NOTE: If you change the class name "NTFmanagement" here, you must also update the reference to "NTFmanagement" in Web.config.
    //[KnownType("GetKnownTypes")]
    public class NTFmanagement : iManagementService
    {
        NTFmanagement() {
           // HibernatingRhinos.NHibernate.Profiler.Appender.NHibernateProfiler.Initialize(); 
        }

        #region Management Templates
            public List<dtoTemplateMessage> AvailableTemplates(int ModuleID, int ActionID, dtoTemplateType tType)
            {
                List<dtoTemplateMessage> templates = null;
                using (ISession session = NHSessionHelper.GetSession())
                {
                    DatabaseContext dc = new lm.Notification.Core.DataLayer.DatabaseContext(session);
                    try
                    {
                        List<TemplateMessage> l = (from TemplateMessage t in dc.Templates
                                                   where
                                                       (ActionID == -9999 || t.ActionID == ActionID) && t.ModuleID == ModuleID && (int)t.Type == (int)tType
                                                   orderby t.LanguageID
                                                   select t).ToList<TemplateMessage>();
                        templates = (from TemplateMessage t in l
                                     orderby t.LanguageID
                                     select new dtoTemplateMessage(t)).ToList<dtoTemplateMessage>();
                    }
                    catch (Exception ex)
                    {
                        templates = new List<dtoTemplateMessage>();
                    }


                    return templates;
                }
            }

            public long SaveTemplate(dtoTemplateMessage template)
            {
                using (ISession session = NHSessionHelper.GetSession())
                {
                    try
                    {
                        session.BeginTransaction();
                        DatabaseContext dc = new lm.Notification.Core.DataLayer.DatabaseContext(session);
                        TemplateMessage t = session.Get<TemplateMessage>(template.ID);

                        if (t == null || t.ID == 0)
                        {
                            t = new TemplateMessage();
                            t.ActionID = template.ActionID;
                            t.LanguageID = template.LanguageID;
                            t.ModuleCode = template.ModuleCode;
                            t.ModuleID = template.ModuleID;
                            t.Type = (TemplateType)template.Type;
                        }
                        t.Name = template.Name;
                        t.Message = template.Message;
                        if (template.Message == "" && t.ID == 0)
                            return 0;
                        else if (template.Message == "" && t.ID > 0)
                            session.Delete(t);
                        else
                            session.SaveOrUpdate(t);
                        session.CommitTransaction();
                        return t.ID;
                    }
                    catch (Exception ex)
                    {
                        session.RollbackTransaction();
                        return 0;
                    }

                }
            }

            public Boolean RemoveTemplate(long templateID)
            {
                using (ISession session = NHSessionHelper.GetSession())
                {
                    try
                    {
                        session.BeginTransaction();
                        DatabaseContext dc = new lm.Notification.Core.DataLayer.DatabaseContext(session);
                        TemplateMessage t = session.Get<TemplateMessage>(templateID);

                        if (t != null && t.ID > 0)
                        {
                            session.Delete(t);
                            session.CommitTransaction();
                            return true;
                        }
                        return false;

                    }
                    catch (Exception ex)
                    {
                        session.RollbackTransaction();
                        return false;
                    }

                }
            }

            public List<dtoModule> AvailableModules()
            {
                List<dtoModule> modules = null;
                using (ISession session = NHSessionHelper.GetSession())
                {
                    try
                    {
                        List<NotificatedModule> n = (from NotificatedModule m in session.Linq<NotificatedModule>()
                                                     orderby m.Name
                                                     select m).ToList<NotificatedModule>();

                        modules = (from NotificatedModule m in n
                                   select new dtoModule(m)).ToList<dtoModule>();

                        //modules = (from NotificatedModule m in session.Linq<NotificatedModule>()
                        //           orderby m.Name
                        //           select new dtoModule(m) ).ToList<dtoModule>();
                    }
                    catch (Exception ex)
                    {
                        modules = new List<dtoModule>();
                    }
                    return modules;
                }

            }
        #endregion


        #region "Remove SystemNotification"
            public Boolean RemoveNews(Guid NotificationID)
            {
                using (ISession session = NHSessionHelper.GetSession())
                {
                    try
                    {
                        session.BeginTransaction();
                        NotificationMessage nm = session.Get<NotificationMessage>(NotificationID);

                        if (nm != null && nm.ID != System.Guid.Empty)
                        {
                            session.Delete(nm);
                            session.CommitTransaction();
                            return true;
                        }
                        return false;

                    }
                    catch (Exception ex)
                    {
                        session.RollbackTransaction();
                        return false;
                    }

                }
            }
            public Boolean RemoveCommunityNews(int CommunityID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveCommunityNotifications");
                        oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, CommunityID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("NotificationToPersonRepository", ex.Message);
                        return false;
                    }
                } 
            }
            public Boolean RemoveUserNews(int PersonID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveUserNotifications");
                        oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, PersonID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("NotificationToPersonRepository", ex.Message);
                        return false;
                    }
                } 
            }
            public Boolean RemoveUserNewsByCommunity(int PersonID, int CommunityID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveUserCommunityNotifications");
                        oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, PersonID);
                        oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, CommunityID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("NotificationToPersonRepository", ex.Message);
                        return false;
                    }
                } 
            }
        #endregion

        #region "Remove Summary"
            public Boolean RemoveNewsSummary(Guid NotificationID)
            {
                using (ISession session = NHSessionHelper.GetSession())
                {
                    try
                    {
                        session.BeginTransaction();
                        NotificationSummary nm = session.Get<NotificationSummary>(NotificationID);

                        if (nm != null && nm.ID != System.Guid.Empty)
                        {
                            session.Delete(nm);
                            session.CommitTransaction();
                            return true;
                        }
                        return false;

                    }
                    catch (Exception ex)
                    {
                        session.RollbackTransaction();
                        return false;
                    }

                }
            }
            public Boolean RemoveCommunityNewsSummary(int CommunityID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveCommunityNewsSummary");
                        oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, CommunityID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("RemoveCommunityNewsSummary", ex.Message);
                        return false;
                    }
                }
            }
            public Boolean RemoveUserNewsSummary(int PersonID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveUserNewsSummary");
                        oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, PersonID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("RemoveUserNewsSummary", ex.Message);
                        return false;
                    }
                }
            }
            public Boolean RemoveUserNewsSummaryByCommunity(int PersonID, int CommunityID)
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(DataHelpers.ConnectionString());

                using (DbConnection connection = oDatabase.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        DbCommand oCommand = oDatabase.GetStoredProcCommand("sp_RemoveUserNewsSummaryByCommunity");
                        oDatabase.AddInParameter(oCommand, "@PersonID", System.Data.DbType.Int64, PersonID);
                        oDatabase.AddInParameter(oCommand, "@CommunityID", System.Data.DbType.Int64, CommunityID);
                        oCommand.Connection = connection;
                        oCommand.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("RemoveUserNewsSummaryByCommunity", ex.Message);
                        return false;
                    }
                }
            }
        #endregion

            #region "Community News"
                public List<dtoCommunityWithNews> GetPersonalCommunityWithNews(int PersonID)
                {
                    List<dtoCommunityWithNews> reminders = null;
                    using (ISession session = NHSessionHelper.GetSession())
                    {
                        try
                        {
                            reminders = (from CommunityNewsSummary r in session.Linq<CommunityNewsSummary>()
                                         where r.PersonID == PersonID && r.ActionCount > 0
                                         orderby r.CommunityID
                                         select new dtoCommunityWithNews(r)).ToList<dtoCommunityWithNews>();
                        }
                        catch (Exception ex)
                        {
                            reminders = new List<dtoCommunityWithNews>();
                        }
                        return reminders;
                    }
                }
                public void UpdateCommunityNewsCount(dtoCommunityWithNews previous)
                {
                    CommunityNewsSummary reminder = null;
                    using (ISession session = NHSessionHelper.GetSession())
                    {
                        session.BeginTransaction();
                        try
                        {
                            reminder = (from CommunityNewsSummary r in session.Linq<CommunityNewsSummary>()
                                         where r.PersonID == previous.PersonID && r.CommunityID == previous.CommunityID
                                         orderby r.CommunityID
                                        select r).FirstOrDefault<CommunityNewsSummary>();
                            if (reminder != null) {
                                if (previous.LastUpdate== reminder.LastUpdate){
                                    reminder.ActionCount = 0;
                                }
                                else if (previous.ActionCount < reminder.ActionCount) {
                                    reminder.ActionCount = reminder.ActionCount - previous.ActionCount;
                                }
                                reminder.LastUpdate = DateTime.Now;

                                
                                session.SaveOrUpdate(reminder);
                                session.CommitTransaction();
                            }
                        }
                        catch (Exception ex)
                        {
                            session.RollbackTransaction();
                        }
                    }
                
                }

                public List<DateTime> GetWeekDaysWithNews(DateTime StartDay, int PersonID, int CommunityID)
                {
                    List<DateTime> WeekDays = null;
                    using (ISession session = NHSessionHelper.GetSession())
                    {
                        try
                        {
                            DateTime PreviousWeek = StartDay.AddDays(-7);
                           WeekDays = (from NotificationSummary ns in session.Linq<NotificationSummary>()
                                            where ns.PersonID == PersonID && (CommunityID<0 || ns.CommunityID == CommunityID)
                                            && (ns.Day >= PreviousWeek || ns.Day <= StartDay)
                                         orderby ns.Day ascending
                                            select ns.Day).Distinct().ToList<DateTime>();
                        }
                        catch (Exception ex)
                        {
                            WeekDays = new List<DateTime>();
                        }
                    }

                    return WeekDays;
                }

                public List<DateTime> GetMonthDaysWithNews(DateTime StartDay, int PersonID, int CommunityID)
                {
                    List<DateTime> MonthDays = null;
                    using (ISession session = NHSessionHelper.GetSession())
                    {
                        try
                        {
                            DateTime PreviousMonth = StartDay.AddMonths(-1);
                            MonthDays = (from NotificationSummary ns in session.Linq<NotificationSummary>()
                                        where ns.PersonID == PersonID && (CommunityID < 0 || ns.CommunityID == CommunityID)
                                        && (ns.Day >= PreviousMonth || ns.Day <= StartDay)
                                        orderby ns.Day ascending
                                        select ns.Day).Distinct().ToList<DateTime>();
                        }
                        catch (Exception ex)
                        {
                            MonthDays = new List<DateTime>();
                        }
                    }

                    return MonthDays;
                }

                public List<CommunityNews> GetCommunityNews(bool OnlySummary, DateTime StartDay, int PersonID, int CommunityID, int UserLanguageID, int DefaultLanguageID, int PageSize, int PageIndex)
                {
                    return new List<CommunityNews>();
                }

                public int GetCommunityNewsCount(bool OnlySummary, DateTime StartDay, int PersonID, int CommunityID, int UserLanguageID, int DefaultLanguageID)
                {
                    return 0;
                }

                public List<CommunityNews> GetPersonCommunityNews(bool OnlySummary, DateTime StartDay, int PersonID, int UserLanguageID, int DefaultLanguageID, int PageSize, int PageIndex)
                {
                    return new List<CommunityNews>();
                }

                public int GetPersonCommunityNewsCount(bool OnlySummary, DateTime StartDay, int PersonID, int UserLanguageID, int DefaultLanguageID)
                {
                    return 0;
                }

            #endregion



                #region iManagementService Members


                public List<dtoCommunitySummaryNotification> GetCommunitySummary(DateTime StartDay, int PersonID, int CommunityID, int LanguageID)
                {
                    throw new NotImplementedException();
                }

                #endregion

                #region iManagementService Members


                public List<dtoModule> GetModulesWithNews(int PersonID, DateTime StartDay)
                {
                    throw new NotImplementedException();
                }

                public List<dtoModule> GetCommunityModulesWithNews(int PersonID, int CommunityID, DateTime StartDay)
                {
                    throw new NotImplementedException();
                }

                #endregion
    }
}
