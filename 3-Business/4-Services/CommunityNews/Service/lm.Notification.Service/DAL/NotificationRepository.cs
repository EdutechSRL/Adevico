using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Notification.Core.DataLayer;
using lm.Notification.Core.Domain;
using lm.Notification.DataContract.Domain;
using NHibernate;
using NHibernate.Linq;

namespace lm.Notification.Service.DAL
{
    public class NotificationRepository
    {
        private ISession session;
        private lm.Notification.Core.DataLayer.DatabaseContext dc;
        private List<TemplateMessage> _PreloadedTempaltes;
        public NotificationRepository(ISession session)
        {
            this.session = session;
            this.dc = new lm.Notification.Core.DataLayer.DatabaseContext(this.session);
            _PreloadedTempaltes = null;
        }
        public NotificationRepository(ISession session,List<TemplateMessage> templates)
        {
            this.session = session;
            this.dc = new lm.Notification.Core.DataLayer.DatabaseContext(this.session);
            _PreloadedTempaltes = templates;
        }

        #region "Manage Templates"
            public List<TemplateMessage> PreLoadTemplates()
        {
            List<TemplateMessage> templates = new List<TemplateMessage>();

            try
            {
                templates = (from TemplateMessage t in dc.Templates orderby t.ModuleCode select t).ToList<TemplateMessage>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("GetTemplates", ex.Message);
            }
            return templates;
        }
            private List<TemplateMessage> GetTemplates(int ActionID, string ModuleCode, TemplateType tType){
                List<TemplateMessage> templates = new List<TemplateMessage>();
                if (_PreloadedTempaltes == null)
                    return GetTemplatesFromDB(ActionID, ModuleCode, tType);
                else {
                    try
                    {
                        templates = (from TemplateMessage t in _PreloadedTempaltes
                                     where
                                         t.ActionID == ActionID && t.ModuleCode == ModuleCode && (int)t.Type == (int)tType
                                     orderby t.LanguageID
                                     select t).ToList<TemplateMessage>();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("GetTemplates", ex.Message);
                    }


                }
                return templates;
            }
            private List<TemplateMessage> GetTemplates(int ActionID, int ModuleID, TemplateType tType){
                List<TemplateMessage> templates = new List<TemplateMessage>();
                if (_PreloadedTempaltes == null)
                    return GetTemplatesFromDB(ActionID, ModuleID, tType);
                try{
                    templates = (from TemplateMessage t in _PreloadedTempaltes
                                 where
                                     t.ActionID == ActionID && t.ModuleID == ModuleID && t.Type == tType
                                 orderby t.LanguageID
                                 select t).ToList<TemplateMessage>();
                }
                catch (Exception ex){
                }
                return templates;
            }

            private List<TemplateMessage> GetTemplatesFromDB(int ActionID, string ModuleCode, TemplateType tType)
            {
                List<TemplateMessage> templates = new List<TemplateMessage>();

                try
                {
                    templates = (from TemplateMessage t in dc.Templates
                                 where
                                     t.ActionID == ActionID && t.ModuleCode == ModuleCode && (int)t.Type == (int)tType
                                 orderby t.LanguageID
                                 select t).ToList<TemplateMessage>();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("GetTemplates", ex.Message);
                }


                return templates;
            }
            private List<TemplateMessage> GetTemplatesFromDB(int ActionID, int ModuleID, TemplateType tType)
            {
                List<TemplateMessage> templates = new List<TemplateMessage>();

                try
                {
                    templates = (from TemplateMessage t in dc.Templates
                                 where
                                     t.ActionID == ActionID && t.ModuleID == ModuleID && t.Type == tType
                                 orderby t.LanguageID
                                 select t).ToList<TemplateMessage>();
                }
                catch (Exception ex)
                {
                }

                return templates;
            }
        #endregion
      
        public void AddNotificationMessage(NotificationToCommunity NotificationTo, List<int> DestinationPersons) {
            System.Guid UniqueID = AddMessage(NotificationTo);

            if (UniqueID != System.Guid.Empty){
                AddNotificatedObjects(UniqueID, NotificationTo.NotificatedObjects);
                List<int> addedUsers = new List<int>();
                DateTime CurrentDay = new DateTime(NotificationTo.SentDate.Year, NotificationTo.SentDate.Month, NotificationTo.SentDate.Day);
                addedUsers = AddUsersToMessage(UniqueID,NotificationTo, CurrentDay, DestinationPersons);

                List<TemplateMessage> summaryTemplates = this.GetTemplates(NotificationTo.ActionID, NotificationTo.ModuleCode, TemplateType.SummaryNotification);
                foreach (int PersonID in addedUsers) {
                    this.UpdateReminderToPerson(NotificationTo, PersonID);
                }
                //foreach (int PersonID in addedUsers)
                //{
                //    this.UpdatePersonSummary(summaryTemplates, NotificationTo, PersonID);
                //}
               
            }
        }

        #region "Inserimento messaggio ed associazione ai relativi utenti"   
            private System.Guid AddMessage(NotificationToCommunity NotificationTo)
        {
            List<TemplateMessage> singleTemplates = this.GetTemplates(NotificationTo.ActionID, NotificationTo.ModuleCode, TemplateType.SingleNotification);


            System.Guid UniqueID = System.Guid.Empty;
            int InsertedMessage = 0;
            foreach (TemplateMessage template in singleTemplates)
            {
                try
                {
                    session.BeginTransaction();
                    NotificationMessage message = this.GetMessage(NotificationTo);
                    string messageToTranslate = string.Format(template.Message, (NotificationTo.ValueParameters).ToArray());

                    message.LanguageID = template.LanguageID;
                    message.Message = messageToTranslate;
                    message.UniqueNotifyID = NotificationTo.UniqueID;
                    message.Template = template;
                    message.SavedDate = DateTime.Now;
                    this.session.SaveOrUpdate(message);

                    session.CommitTransaction();
                    InsertedMessage++;
                }

                catch (Exception ex)
                {
                    session.RollbackTransaction();
                    ErrorHandler pError = new ErrorHandler();
                    pError.addMessageToPoisonQueue(NotificationTo, ex);
                }
            }
            if (InsertedMessage > 0)
                return NotificationTo.UniqueID;
            else
                return System.Guid.Empty;
        }
            private List<int> AddUsersToMessage(System.Guid UniqueID,NotificationToCommunity NotificationTo, DateTime CurrentDay, List<int> DestinationPersons)
        {
            List<int> addedUsers = new List<int>();
            List<PersonNotification> pNotifications = (from int id in DestinationPersons
                                                       select new PersonNotification()
                                                       {
                                                           PersonID = id,
                                                           NotificationUniqueID = UniqueID,
                                                           Day=CurrentDay,
                                                           CommunityID = NotificationTo.CommunityID,
                                                           SentDate = NotificationTo.SentDate 
                                                       }).ToList<PersonNotification>();
            
            foreach (PersonNotification pn in pNotifications){
                try{
                    if (NotificationToPersonRepository.Add(pn)==true)
                        addedUsers.Add(pn.PersonID);
                        //this.session.Save(pn);        
                }
                catch (Exception ex){
                }
            }
            return addedUsers;
        }
            private void AddNotificatedObjects(System.Guid NotificationUniqueID, List<dtoNotificatedObject> NotificatedObjects)
            {
                if (NotificatedObjects != null && NotificatedObjects.Count > 0)
                {
                    foreach (dtoNotificatedObject obj in NotificatedObjects)
                    {
                        try
                        {
                            session.BeginTransaction();
                            NotificatedObject nObj = new NotificatedObject();
                            nObj.FullyQualiFiedName = obj.FullyQualiFiedName;
                            nObj.ModuleCode = obj.ModuleCode;
                            nObj.ModuleID = obj.ModuleID;
                            nObj.ObjectID = obj.ObjectID;
                            nObj.ObjectTypeID = obj.ObjectTypeID;
                            nObj.UniqueNotifyID = NotificationUniqueID;

                            session.Save(nObj);
                            session.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            session.RollbackTransaction();
                        }
                    }

                }
            }
            private NotificationMessage GetMessage(NotificationToCommunity NotificationTo)
            {
                NotificationMessage Message = new NotificationMessage();
                Message.ActionID = NotificationTo.ActionID;
                Message.CommunityID = NotificationTo.CommunityID;
                Message.ModuleCode = NotificationTo.ModuleCode;
                Message.ModuleID = NotificationTo.ModuleID;
                Message.SentDate = NotificationTo.SentDate;
                //     Message.ID = System.Guid.NewGuid();
                Message.isDeleted = false;
                Message.Message = "";
                Message.SavedDate = DateTime.Now;
                Message.Day = new DateTime(NotificationTo.SentDate.Year, NotificationTo.SentDate.Month, NotificationTo.SentDate.Day);
                Message.isDeleted = false;
                // Message.PersonsNotification = new List<PersonNotification>();
                return Message;
            }
        #endregion
            
     
        //----------------------------------------------NOTA--------------------
        // Ad un'ora prestabilità tipo 3.00 la coda non deve essere più svuotata
        // si elimina il summary e si ricalcola completamente.
        // 
        // NB. elimino i record vecchi(più di due mesi)
        //
        // 1.Carico in memoria tutte le notifiche di un utente 
        // 2.Creo un oggetto con l'indirizzamento(comunity,date,modulo,azione)
        //    passo tutti i record e estraggo ed elimino quelli uguali,(aggiungendo 1 al contatore)
        //      
        // 3.salvo il record nel DB e passo al record successivo rimasto fino a quando la lista è vuota
        // 4.passo all'utente sucessivo fino a quando ho finito
        //
        // 5. FINITO..faccio ripartire l'inserimento
        //
        //----------------------------------------------------------------------
        //private void SummaryProcess()
        //{
        //    //Stop estrazione coda

        //    foreach (long p in AllPersons) //tutte le persone del sistma,utenti!!
        //    {
        //        //Lista elemnti riassunto
        //        List<NotificationSummary> summaries = new List<NotificationSummary>();

        //        //Lista dei messsagi di una determinata persona!
        //        List<NotificationMessage> message = (from m in NotificationMessage select m).ToList();
        //        //t in Notification where t. == TemplateType.SingleNotification select t).ToList<TemplateMessage>();
 
        //        foreach (NotificationMessage myM in message)
        //        {
        //            //                          
        //        }


        //    }
            

        //}

            
            private void UpdateReminderToPerson(NotificationToCommunity NotificationTo, int PersonID)
            {
                try
                {
                    CommunityNewsSummaryRepository.Save(PersonID, NotificationTo.CommunityID);
                    //CommunityNewsSummary reminder = (from CommunityNewsSummary rtp in session.Linq<CommunityNewsSummary>()
                    //                                 where rtp.PersonID == PersonID && rtp.CommunityID == NotificationTo.CommunityID
                    //                                 select rtp).FirstOrDefault<CommunityNewsSummary>();
                    //if (reminder == null || reminder.ID == System.Guid.Empty)
                    //{
                    //    reminder = new CommunityNewsSummary();
                    //    reminder.CommunityID = NotificationTo.CommunityID;
                    //    reminder.PersonID = PersonID;
                    //    reminder.ActionCount = 1;
                    //    reminder.LastUpdate = DateTime.Now;
                    //    reminder.LastUserRead = DateTime.MinValue;
                    //}
                    //else
                    //{
                    //    reminder.ActionCount++;
                    //    reminder.LastUpdate = DateTime.Now;
                    //}
                    //CommunityNewsSummaryRepository.Save(reminder);
                }
                catch (Exception ex){}
            }

            #region "Summary"
                private void UpdatePersonSummary(List<TemplateMessage> summaryTemplates, NotificationToCommunity NotificationTo, int PersonID)
                {
                    try
                    {
                        session.BeginTransaction();

                        System.Guid UniqueSummaryID = System.Guid.NewGuid();

                        DateTime Day = new DateTime(NotificationTo.SentDate.Year, NotificationTo.SentDate.Month, NotificationTo.SentDate.Day);


                        List<NotificationSummary> UserSummaries = (from NotificationSummary s in dc.PersonSummaries
                                                                   where s.PersonID == PersonID && s.ModuleID == NotificationTo.ModuleID
                                                                      && s.ActionID == NotificationTo.ActionID && s.Day == Day
                                                                      && s.CommunityID == NotificationTo.CommunityID
                                                                   select s).ToList<NotificationSummary>();

                        System.Guid DBUniqueID = System.Guid.NewGuid();
                        if (UserSummaries.Count > 0)
                            DBUniqueID = UserSummaries[0].UniqueNotifyID;
                        foreach (TemplateMessage template in summaryTemplates)
                        {
                            try
                            {
                                NotificationSummary summary = (from NotificationSummary n in UserSummaries
                                                               where n.LanguageID == template.LanguageID
                                                               select n).FirstOrDefault<NotificationSummary>();
                                if (summary == null || summary.UniqueNotifyID == System.Guid.Empty)
                                {
                                    summary = GetMessageSummary(NotificationTo, UniqueSummaryID, PersonID);
                                    summary.UniqueNotifyID = DBUniqueID;
                                    summary.Template = template;
                                    summary.LanguageID = template.LanguageID;
                                }
                                if (summary.isDeleted)
                                {
                                    summary.isDeleted = false;
                                    summary.TotalMessages = 0;
                                }
                                summary.TotalMessages++;
                                string messageToTranslate = string.Format(template.Message, summary.TotalMessages);

                                summary.Message = messageToTranslate;
                                session.SaveOrUpdate(summary);
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        session.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        session.RollbackTransaction();
                    }
                }
                private NotificationSummary GetMessageSummary(NotificationToCommunity NotificationTo, System.Guid UniqueSummaryID, int PersonID)
        {
            NotificationSummary Summary = new NotificationSummary();
            Summary.ActionID = NotificationTo.ActionID;
            Summary.CommunityID = NotificationTo.CommunityID;
            Summary.ModuleCode = NotificationTo.ModuleCode;
            Summary.ModuleID = NotificationTo.ModuleID;
            Summary.SentDate = NotificationTo.SentDate;
            Summary.isDeleted = false;
            Summary.Message = "";
            Summary.SavedDate = DateTime.Now;
            Summary.Day = new DateTime(NotificationTo.SentDate.Year, NotificationTo.SentDate.Month, NotificationTo.SentDate.Day);
            Summary.isDeleted = false;
            Summary.UniqueNotifyID = UniqueSummaryID;
            Summary.Day = new DateTime(NotificationTo.SentDate.Year, NotificationTo.SentDate.Month, NotificationTo.SentDate.Day);
            Summary.PersonID = PersonID;
            return Summary;
        }
            #endregion
    }
}
