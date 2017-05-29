using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.File;

namespace lm.Comol.Modules.TaskList.Business
{
    public class CoreTaskService : lm.Comol.Core.Business.CoreServices
    {


        #region "Events"

        public Task GetTask(long ItemID)
        {
            Task oItem = null;
            try
            {
                oItem = Manager.Get<Task>(ItemID);
            }
            catch (Exception ex)
            {
            }
            return oItem;
        }
        public string GetTaskDescription(long ItemID)
        {
            string iResponse = "";

            try
            {
                iResponse = (from p in Manager.Linq<DescriptionEventItem>() where p.Id == ItemID select p.Description).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return iResponse;
        }
        public string GetTaskDescription(Task oItem)
        {
            return GetTaskDescription(oItem.ID);
        }


        //------------------------------------------------------------------------------------------------------------------------------------------------------------------

        #region "File Item Link"
        public lm.Comol.Modules.TaskList.Domain.TaskListFile GetTaskListFile(long IdItemFile)
        {
            TaskListFile oItem = null;
            try
            {
                oItem = Manager.Get<TaskListFile>(IdItemFile);
            }
            catch (Exception ex)
            {
            }
            return oItem;
        }
        public void VirtualDeleteFileEventItemLink(long fileItemID)
        {
            SetVirtualDeleteFileTaskLink(fileItemID, true);
        }

        public void VirtualUndeleteFileItemLink(long fileItemID)
        {
            SetVirtualDeleteFileTaskLink(fileItemID, false);
        }
        public void VirtualDeleteFileEventItemLinks(IList<long> fileItemsID)
        {
            SetVirtualDeleteFileEventItemLinks(fileItemsID, true);
        }
        public void VirtualUndeleteFileEventItemLinks(IList<long> fileItemsID)
        {
            SetVirtualDeleteFileEventItemLinks(fileItemsID, false);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------       
        private void SetVirtualDeleteFileTaskLink(long taskfileID, Boolean delete)
        {
            Person person = Manager.GetPerson(UC.CurrentUserID);
            TaskListFile fileTaskLink = Manager.Get<TaskListFile>(taskfileID);
            if (person != null && fileTaskLink != null)
            {
                try
                {
                    Manager.BeginTransaction();
                    fileTaskLink.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    fileTaskLink.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                    fileTaskLink.TaskOwner.MetaInfo.ModifiedBy  = fileTaskLink.ModifiedBy;
                    fileTaskLink.TaskOwner.MetaInfo.ModifiedOn = fileTaskLink.ModifiedOn.Value;
                    Manager.SaveOrUpdate(fileTaskLink.TaskOwner);
                    Manager.SaveOrUpdate(fileTaskLink);
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }

        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void SetVirtualDeleteFileEventItemLinks(IList<long> fileItemsID, Boolean delete)
        {
            Person person = Manager.GetPerson(UC.CurrentUserID);
            IList<TaskListFile> files = (from f in Manager.Linq<TaskListFile>() where fileItemsID.Contains(f.Id) select f).ToList();
            if (person != null && files.Count > 0)
            {
                try
                {
                    Manager.BeginTransaction();
                    foreach (TaskListFile f in files)
                    {
                        f.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        f.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                        f.TaskOwner.MetaInfo.ModifiedBy = f.ModifiedBy;
                        f.TaskOwner.MetaInfo.ModifiedOn = f.ModifiedOn.Value;
                        Manager.SaveOrUpdate(f.TaskOwner);
                        Manager.SaveOrUpdate(f);
                    }

                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }
        }

        public Boolean UnLinkToCommunityFileFromTask(long fileTaskID)
        {
            Boolean iResponse = false;
            Person person = Manager.GetPerson(UC.CurrentUserID);
            TaskListFile fileTaskLink = Manager.Get<TaskListFile>(fileTaskID);
            if (person != null && fileTaskLink != null && (fileTaskLink.File == null || !fileTaskLink.File.IsInternal))
            {
                try
                {
                    Manager.BeginTransaction();
                    fileTaskLink.TaskOwner.MetaInfo.ModifiedBy = person;
                    fileTaskLink.TaskOwner.MetaInfo.ModifiedOn = DateTime.Now;
                    Manager.SaveOrUpdate(fileTaskLink.TaskOwner);
                    Manager.DeleteGeneric(fileTaskLink);
                    Manager.Commit();
                    iResponse = true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }
            return iResponse;
        }

        public int UnLinkToCommunityFileFromTask(IList<long> fileTasksID)
        {
            int iResponse = 0;
            Person person = Manager.GetPerson(UC.CurrentUserID);
            //        Dim oFilesToRemove As List(Of EventCommunityFile) = (From f In DC.GetCurrentSession.Linq(Of EventCommunityFile)() Where oFilesID.Contains(f.FileCommunity.Id) AndAlso (f.FileCommunity.isVisible _
            //                OrElse (Not f.FileCommunity.isVisible AndAlso (oPermission.Administration OrElse f.FileCommunity.Owner Is Owner))) Select f).ToList
            IList<TaskListFile> files = (from f in Manager.Linq<TaskListFile>() where fileTasksID.Contains(f.Id) select f).ToList();
            if (person != null && files.Count > 0)
            {
                foreach (TaskListFile fileTaskLink in files)
                {
                    if (fileTaskLink.File == null || !fileTaskLink.File.IsInternal)
                    {
                        try
                        {
                            Manager.BeginTransaction();
                            fileTaskLink.TaskOwner.MetaInfo.ModifiedBy = person;
                            fileTaskLink.TaskOwner.MetaInfo.ModifiedOn = DateTime.Now;
                            Manager.SaveOrUpdate(fileTaskLink.TaskOwner);
                            Manager.DeleteGeneric(fileTaskLink);
                            Manager.Commit();
                            iResponse++;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                    }
                }
            }
            return iResponse;
        }

        //public void LinkRepositoryItemToEventItem(CommunityEventItem item, int communityId, List<long> files, int moduleID, int objectTypeId, String moduleCode, Boolean AutoEvaluable)
        //{
        //    //try
        //    //{
        //    //    Person person = Manager.GetPerson(UC.CurrentUserID);
        //    //    if (files.Count > 0)
        //    //    {
        //    //        Manager.BeginTransaction();
        //    //        Community community = Manager.GetCommunity(communityId);
        //    //        foreach (long IdItem in files)
        //    //        {
        //    //            EventItemFile itemfile = new EventItemFile();
        //    //            itemfile.CommunityOwner = community;
        //    //            itemfile.EventOwner = item.EventOwner;
        //    //            itemfile.File = Manager.Get<BaseCommunityFile>(IdItem);
        //    //            itemfile.isVisible = true;
        //    //            itemfile.ItemOwner = item;
        //    //            itemfile.Owner = person;
        //    //            itemfile.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //    //            Manager.SaveOrUpdate(itemfile);

        //    //            ModuleLink link = new ModuleLink(file.Description, file.Permission, file.Action);
        //    //            link.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
        //    //            link.DestinationItem = (ModuleObject)file.ModuleObject;
        //    //            link.AutoEvaluable = AutoEvaluable;
        //    //            link.SourceItem = ModuleObject.CreateLongObject(itemfile.Id, itemfile, objectTypeId, communityId, moduleCode, moduleID);
        //    //            Manager.SaveOrUpdate(link);
        //    //            itemfile.Link = link;
        //    //            Manager.SaveOrUpdate(itemfile);
        //    //            if (typeof(ModuleLongInternalFile) == file.ModuleObject.ObjectOwner.GetType())
        //    //            {
        //    //                ModuleLongInternalFile f = (ModuleLongInternalFile)file.ModuleObject.ObjectOwner;
        //    //                f.ObjectOwner = itemfile;
        //    //                f.ObjectTypeID = objectTypeId;
        //    //                Manager.SaveOrUpdate(f);
        //    //            }

        //    //            item.ModifiedBy = itemfile.CreatedBy;
        //    //            item.ModifiedOn = itemfile.CreatedOn.Value;
        //    //            Manager.SaveOrUpdate(item);
        //    //        }
        //    //        Manager.Commit();
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Manager.RollBack();
        //    //    throw new EventItemFileNotLinked(ex.Message, ex);
        //    //}
        //}


        public void SaveTaskListFile(Task task, int communityId, ModuleActionLink file, int moduleID, int objectTypeId, String moduleCode, Boolean AutoEvaluable)
        {
            IList<ModuleActionLink> files = new List<ModuleActionLink>();
            files.Add(file);
            SaveTaskListFiles(task, communityId, files, moduleID, objectTypeId, moduleCode, AutoEvaluable);
        }
        
        public void SaveTaskListFiles(Task task, int communityId, IList<ModuleActionLink> files, int moduleID, int objectTypeId, String moduleCode, Boolean AutoEvaluable)
        {
            try
            {
                Person person = Manager.GetPerson(UC.CurrentUserID);
                if (files.Count > 0)
                {
                    Manager.BeginTransaction();
                    Community community = Manager.GetCommunity(communityId);
                    foreach (ModuleActionLink file in files)
                    {
                        TaskListFile taskfile = (from f in Manager.GetAll<TaskListFile>(f => f.CommunityOwner == community && f.TaskOwner == task && f.ProjectOwner == task.Project  && f.File == (BaseCommunityFile)file.ModuleObject.ObjectOwner) select f).Skip(0).Take(1).ToList().FirstOrDefault();
                        if (taskfile == null)
                        {
                            taskfile = new TaskListFile();
                            taskfile.CommunityOwner = community;
                            taskfile.ProjectOwner = task.Project;
                            taskfile.File = (BaseCommunityFile)file.ModuleObject.ObjectOwner;
                            taskfile.TaskOwner = task;
                            taskfile.Owner = person;
                            taskfile.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        }
                        else
                            taskfile.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        taskfile.isVisible = true;

                        Manager.SaveOrUpdate(taskfile);

                        ModuleLink link = new ModuleLink(file.Description, file.Permission, file.Action);
                        link.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        link.DestinationItem = (ModuleObject)file.ModuleObject;
                        link.AutoEvaluable = AutoEvaluable;
                        link.SourceItem = ModuleObject.CreateLongObject(taskfile.Id, taskfile, objectTypeId, communityId, moduleCode, moduleID);
                        Manager.SaveOrUpdate(link);
                        taskfile.Link = link;
                        Manager.SaveOrUpdate(taskfile);
                        if (typeof(ModuleLongInternalFile) == file.ModuleObject.ObjectOwner.GetType())
                        {
                            ModuleLongInternalFile f = (ModuleLongInternalFile)file.ModuleObject.ObjectOwner;
                            f.ObjectOwner = taskfile;
                            f.ObjectTypeID = objectTypeId;
                            Manager.SaveOrUpdate(f);
                        }

                        task.MetaInfo.ModifiedBy = taskfile.CreatedBy;
                        task.MetaInfo.ModifiedOn = taskfile.CreatedOn.Value;
                        Manager.SaveOrUpdate(task);
                    }
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                throw new lm.Comol.Core.Event.EventItemFileNotLinked(ex.Message, ex);
            }
        }
        
        public IList<iCoreItemFileLink<long>> GetTaskFilesToPublish(long Idtask, Boolean viewAlsoDeleted)
        {
            Task task = Manager.Get<Task>(Idtask);
            IList<iCoreItemFileLink<long>> files = (from fi in Manager.GetAll<TaskListFile>(fi => fi.TaskOwner == task && fi.File != null && (viewAlsoDeleted || (!viewAlsoDeleted && fi.Deleted == BaseStatusDeleted.None)))
                                                    where fi.File.IsInternal
                                                    select (iCoreItemFileLink<long>)new dtoCoreItemFileLink<long>()
                                                    {
                                                        Deleted = fi.Deleted,
                                                        File = fi.File,
                                                        ItemFileLinkId = fi.Id,
                                                        Link = fi.Link,
                                                        isVisible = fi.isVisible
                                                    }).ToList();

            List<iCoreItemFileLink<long>> orderedFiles =
                (from f in files where f.File != null && f.Link != null orderby f.File.DisplayName select f).ToList();
            return orderedFiles;
        }

        public IList<iCoreItemFileLink<long>> GetTaskFiles(Task task, Boolean viewAlsoDeleted)
        {
            IList<iCoreItemFileLink<long>> files;
            files = (from fi in Manager.GetAll<TaskListFile>(fi => fi.TaskOwner == task && (viewAlsoDeleted || (!viewAlsoDeleted && fi.Deleted == BaseStatusDeleted.None)))
                     select (iCoreItemFileLink<long>)new dtoCoreItemFileLink<long>()
                     {
                         CreatedBy = fi.CreatedBy,
                         CreatedOn = fi.CreatedOn,
                         Deleted = fi.Deleted,
                         File = fi.File,
                         ItemFileLinkId = fi.Id,
                         StatusId = 0,
                         Link = fi.Link,
                         ModifiedBy = fi.ModifiedBy,
                         ModifiedOn = fi.ModifiedOn,
                         Owner = fi.Owner,
                         isVisible = fi.isVisible
                     }).ToList();

            List<iCoreItemFileLink<long>> orderedFiles;
            orderedFiles = (from f in files where f.File == null || f.Link == null orderby f.CreatedOn select f).ToList();
            orderedFiles.AddRange((from f in files where f.File != null && f.Link != null orderby f.File.DisplayName select f).ToList());
            return orderedFiles;
        }

        public List<long> GetItemRepositoryFilesId(Task item, Boolean viewAll)
        {
            List<long> Idfiles = (from fi in Manager.GetIQ<TaskListFile>()
                                  where fi.TaskOwner == item && (viewAll || (!viewAll && fi.Deleted == BaseStatusDeleted.None))
                                   && fi.File != null && fi.Link != null && !fi.File.IsInternal
                                  select fi.File.Id).ToList();
            return Idfiles;
        }

        public List<long> GetCoreTaskFileLinksId(Task task, Boolean viewAll)
        {
            List<long> IdLinks = (from fi in Manager.GetIQ<TaskListFile>()
                                  where fi.TaskOwner == task && (viewAll || (!viewAll && fi.Deleted == BaseStatusDeleted.None))
                                   && fi.File != null && fi.Link != null && !fi.File.IsInternal
                                  select fi.Id).ToList();
            return IdLinks;
        }

        public List<iCoreItemFileLink<long>> GetTaskRepositoryFiles(Task task, Boolean viewAll)
        {
            List<iCoreItemFileLink<long>> files = (from fi in Manager.GetAll<TaskListFile>(fi => fi.TaskOwner == task && (viewAll || (!viewAll && fi.Deleted == BaseStatusDeleted.None))
                                                    && fi.File != null && fi.Link != null && !fi.File.IsInternal)
                                                   select (iCoreItemFileLink<long>)new dtoCoreItemFileLink<long>() { File = fi.File, Link = fi.Link, ItemFileLinkId = fi.Id }).ToList();
            return files;
        }

        public void EditTaskListFileVisibility(long fileTaskID, Boolean visibleForModule, Boolean visibleForRepository)
        {
            EditTaskListFileVisibility(Manager.Get<TaskListFile>(fileTaskID), visibleForModule, visibleForRepository);
        }

        public void EditTaskListFileVisibility(TaskListFile fileTaskLink, Boolean visibleForModule, Boolean visibleForRepository)
        {
            Person person = Manager.GetPerson(UC.CurrentUserID);
            if (person != null && fileTaskLink != null)
            {
                try
                {
                    Manager.BeginTransaction();
                    fileTaskLink.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    fileTaskLink.isVisible = visibleForModule;
                    if (fileTaskLink.File != null & !fileTaskLink.File.IsInternal)
                    {
                        fileTaskLink.File.isVisible = visibleForRepository;
                        fileTaskLink.File.ModifiedBy = person;
                        fileTaskLink.File.ModifiedOn = fileTaskLink.ModifiedOn;
                        Manager.SaveOrUpdate(fileTaskLink.File);
                    }

                    Manager.SaveOrUpdate(fileTaskLink);
                    fileTaskLink.TaskOwner.MetaInfo.ModifiedBy = fileTaskLink.ModifiedBy;
                    fileTaskLink.TaskOwner.MetaInfo.ModifiedOn = fileTaskLink.ModifiedOn.Value;
                    Manager.SaveOrUpdate(fileTaskLink.TaskOwner);
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }

        }

        public void EditFileRepositoryVisibility(long fileTaskID)
        {
            EditFileRepositoryVisibility(Manager.Get<TaskListFile>(fileTaskID));
        }

        public void EditFileRepositoryVisibility(TaskListFile fileTaskLink)
        {
            Person person = Manager.GetPerson(UC.CurrentUserID);
            EditFileTaskVisibility(fileTaskLink);
            if (person != null && fileTaskLink != null && !fileTaskLink.File.IsInternal)
            {
                try
                {
                    Manager.BeginTransaction();
                    fileTaskLink.File.isVisible = fileTaskLink.isVisible;
                    fileTaskLink.File.ModifiedBy = person;
                    fileTaskLink.File.ModifiedOn = fileTaskLink.ModifiedOn;
                    Manager.SaveOrUpdate(fileTaskLink.File);
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }

        }

        public void EditModuleFileTaskVisibility(long fileTaskID)
        {
            EditFileTaskVisibility(Manager.Get<TaskListFile>(fileTaskID));
        }

        private void EditFileTaskVisibility(TaskListFile fileTaskLink)
        {
            Person person = Manager.GetPerson(UC.CurrentUserID);
            if (person != null && fileTaskLink != null)
            {
                try
                {
                    Manager.BeginTransaction();
                    fileTaskLink.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                    fileTaskLink.isVisible = !fileTaskLink.isVisible;
                    Manager.SaveOrUpdate(fileTaskLink);
                    fileTaskLink.TaskOwner.MetaInfo.ModifiedBy = fileTaskLink.ModifiedBy;
                    fileTaskLink.TaskOwner.MetaInfo.ModifiedOn = fileTaskLink.ModifiedOn.Value;
                    Manager.SaveOrUpdate(fileTaskLink.TaskOwner);
                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }

        }

        public void EditModuleFileTasksVisibility(IList<long> list)
        {
            Person person = Manager.GetPerson(UC.CurrentUserID);
            IList<TaskListFile> files = (from f in Manager.Linq<TaskListFile>() where list.Contains(f.Id) select f).ToList();
            if (person != null && files.Count > 0)
            {
                try
                {
                    Manager.BeginTransaction();
                    foreach (TaskListFile f in files)
                    {
                        f.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        f.isVisible = !f.isVisible;
                        Manager.SaveOrUpdate(f);
                        f.TaskOwner.MetaInfo.ModifiedBy = f.ModifiedBy;
                        f.TaskOwner.MetaInfo.ModifiedOn = f.ModifiedOn.Value;
                        Manager.SaveOrUpdate(f.TaskOwner);
                        Manager.Commit();
                    }

                    Manager.Commit();
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
            }
        }

        public void PhisicalDeleteFileTask(long taskID, long taskLinkId, String BaseFilePath)
        {
            IList<TaskListFile> list = new List<TaskListFile>();
            list.Add(Manager.Get<TaskListFile>(taskLinkId));
            PhisicalDeleteFileTask(list, BaseFilePath);
        }

        public void PhisicalDeleteFileTask(IList<TaskListFile> links, String BaseFilePath)
        {
            try
            {
                IList<string> filesToRemove = new List<string>();
                Manager.BeginTransaction();
                string fileName = BaseFilePath + "\\{0}\\{1}.stored";
                foreach (TaskListFile fileLink in links)
                {
                    if (fileLink != null)
                    {
                        if (fileLink.File != null)
                        {
                            filesToRemove.Add(String.Format(fileName, fileLink.File.CommunityOwner == null ? "0" : fileLink.File.CommunityOwner.Id.ToString(), fileLink.File.UniqueID.ToString()));
                            Manager.DeletePhysical(fileLink.File);
                        }

                        if (fileLink.Link != null)
                        {
                            Manager.DeletePhysical(fileLink.Link);
                        }
                        Manager.DeletePhysical(fileLink);
                    }
                }
                Manager.Commit();
                Delete.Files(filesToRemove);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region "Save Event / event Item"

        //private Task CreateGenericEvent(Community community, Task item, CommunityEventType eventType, Person person)
        //{
        //    Task oEventItem = new Task();
        //    oEventItem.Community = community;
        //    //oEventItem.ForEver = false;
        //    //oEventItem.IsMacro = false;
        //    //oEventItem.IsRepeat = false;
        //    //oEventItem.IsVisible = item.IsVisible;
        //    oEventItem.Items = new List<Task>();
        //    oEventItem.Link = item.Link;

        //    if (item.ModifiedOn.Equals(DateTime.MinValue))
        //    {
        //        oEventItem.ModifiedOn = DateTime.Now;
        //        item.ModifiedOn = oEventItem.ModifiedOn;
        //    }
        //    else
        //        oEventItem.ModifiedOn = item.ModifiedOn;
        //    oEventItem.Name = "";
        //    ////DiaryItem.Title
        //    oEventItem.Note = item.Note;
        //    oEventItem.Owner = person;
        //    oEventItem.Place = item.Place;
        //    oEventItem.Year = item.StartDate.Year;
        //    oEventItem.EventType = eventType;
        //    if (item.StartDate.Month < 9)
        //    {
        //        oEventItem.Year = oEventItem.Year - 1;
        //    }
        //    return oEventItem;
        //}

        //public CommunityEvent AddMultipleItems(int communityID, Task dtoItem, String description, int ownerId, CommunityEventType eventType, DateTime startDate, DateTime endDate, List<dtoWeekDay> weekDays)
        //{
        //    CommunityEvent communityEvent = null;
        //    try
        //    {
        //        Person person = Manager.GetPerson(ownerId);
        //        Community community = Manager.GetCommunity(communityID);
        //        if ((community != null && communityID > 0) && person != null)
        //        {
        //            List<dtoWeekDayRecurrence> itemsToInsert = dtoWeekDayRecurrence.CreateRecurrency(startDate, endDate, weekDays);
        //            if (itemsToInsert.Count > 0)
        //            {
        //                Manager.BeginTransaction();
        //                communityEvent = CreateGenericEvent(community, dtoItem, eventType, person);
        //                //iResponse = New CommunityEvent With {.CommunityOwner = oCommunity, .IsVisible = oItem.IsVisible, .EventType = oType, .Link = oItem.Link, .ModifiedOn = Now, .Name = " ", .Note = oItem.Note, .Owner = oCreatedBy, .Place = oItem.Place, .Year = StartDate.Year}
        //                Manager.SaveOrUpdate(communityEvent);
        //                foreach (dtoWeekDayRecurrence recurrence in itemsToInsert)
        //                {
        //                    Task eventItem = new Task() { CommunityOwner = community, EventOwner = communityEvent, Owner = person, CreatedBy = person, CreatedOn = DateTime.Now };
        //                    eventItem.ModifiedBy = eventItem.CreatedBy;
        //                    eventItem.ModifiedOn = eventItem.CreatedOn;
        //                    eventItem.Note = dtoItem.Note;
        //                    eventItem.Place = dtoItem.Place;
        //                    eventItem.Title = dtoItem.Title;
        //                    eventItem.StartDate = recurrence.StartDate;
        //                    eventItem.EndDate = recurrence.EndDate;
        //                    eventItem.ShowDateInfo = dtoItem.ShowDateInfo;
        //                    eventItem.IsVisible = dtoItem.IsVisible;
        //                    eventItem.Link = dtoItem.Link;
        //                    Manager.SaveOrUpdate(eventItem);
        //                    if (!string.IsNullOrEmpty(description))
        //                    {
        //                        DescriptionEventItem descriptionItem = new DescriptionEventItem() { Id = eventItem.Id, Description = description };
        //                        Manager.SaveOrUpdate(descriptionItem);
        //                    }
        //                    communityEvent.Items.Add(eventItem);
        //                }
        //                Manager.SaveOrUpdate(communityEvent);
        //                Manager.Commit();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        communityEvent = null;
        //    }
        //    return communityEvent;
        //}
        //public Task SaveEventItem(int communityID, Task unsavedItem, String description, int ownerId, int savedById, CommunityEventType eventType)
        //{
        //    Task eventItem = null;

        //    Person owner = Manager.GetPerson(ownerId);
        //    Person savedBy = Manager.GetPerson(savedById);
        //    Community community = Manager.GetCommunity(communityID);
        //    CommunityEvent eventOwner = unsavedItem.EventOwner;
        //    try
        //    {
        //        if (owner != null && savedBy != null && (community != null && communityID > 0))
        //        {
        //            Manager.BeginTransaction();
        //            if (unsavedItem.Id == 0)
        //            {
        //                eventItem = new Task();
        //                eventItem.Owner = owner;
        //                eventItem.CreatedBy = savedBy;
        //                eventItem.CreatedOn = DateTime.Now;
        //                eventItem.ModifiedBy = savedBy;
        //                eventItem.ModifiedOn = eventItem.CreatedOn;
        //                eventItem.CommunityOwner = community;
        //            }
        //            else
        //            {
        //                eventItem = Manager.Get<Task>(unsavedItem.Id);
        //                eventItem.ModifiedBy = savedBy;
        //                eventItem.ModifiedOn = DateTime.Now;
        //            }
        //            eventItem.Note = unsavedItem.Note;
        //            eventItem.Place = unsavedItem.Place;
        //            eventItem.Title = unsavedItem.Title;
        //            eventItem.StartDate = unsavedItem.StartDate;
        //            eventItem.EndDate = unsavedItem.EndDate;
        //            eventItem.ShowDateInfo = unsavedItem.ShowDateInfo;
        //            eventItem.IsVisible = unsavedItem.IsVisible;
        //            eventItem.Link = unsavedItem.Link;

        //            if (unsavedItem.Id == 0)
        //            {
        //                eventOwner = CreateGenericEvent(community, eventItem, eventType, savedBy);
        //                Manager.SaveOrUpdate(eventOwner);
        //                eventItem.EventOwner = eventOwner;
        //                if (eventOwner.Items == null)
        //                    eventOwner.Items = new List<Task>();
        //                eventOwner.Items.Add(eventItem);
        //                Manager.SaveOrUpdate(eventItem);
        //                Manager.SaveOrUpdate(eventOwner);
        //            }
        //            else
        //                Manager.SaveOrUpdate(eventItem);

        //            DescriptionEventItem descriptionItem = Manager.Get<DescriptionEventItem>(eventItem.Id);
        //            if (descriptionItem == null && !string.IsNullOrEmpty(description))
        //                descriptionItem = new DescriptionEventItem() { Id = eventItem.Id };
        //            if (descriptionItem != null)
        //            {
        //                descriptionItem.Description = description;
        //                Manager.SaveOrUpdate(descriptionItem);
        //            }
        //            Manager.Commit();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        eventItem = null;
        //    }

        //    return eventItem;
        //}

        protected IEnumerable<Task> CommunityTasksQuery(Community community, Person person, Boolean allVisibleItems)
        {
            IEnumerable<Task> query = (from item in Manager.GetAll<Task>(i => i.Community == community && (allVisibleItems))
                                       orderby item.StartDate, item.MetaInfo.CreatedOn
                                       select item);
            return query;
        }

        //public void EditEventItemVisibility(long IdItem)
        //{
        //    EditEventItemVisibility(Manager.Get<Task>(IdItem));
        //}
        //public void EditEventItemVisibility(Task item)
        //{
        //    Person person = Manager.GetPerson(UC.CurrentUserID);
        //    if (person != null && item != null)
        //    {
        //        try
        //        {
        //            Manager.BeginTransaction();
        //            item.IsVisible = !item.IsVisible;
        //            item.ModifiedBy = person;
        //            item.ModifiedOn = DateTime.Now;
        //            Manager.SaveOrUpdate(item);
        //            Manager.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            Manager.RollBack();
        //        }
        //    }

        //}
        //public Boolean PhisicalDeleteEventItem(long IdItem, String BaseFilePath)
        //{
        //    return PhisicalDeleteEventItem(Manager.Get<Task>(IdItem), BaseFilePath);
        //}
        //public Boolean PhisicalDeleteEventItem(Task item, String BaseFilePath)
        //{
        //    Boolean deleted = false;
        //    try
        //    {
        //        if (item != null)
        //        {
        //            IList<string> filesToRemove = new List<string>();
        //            int IdCommunity = item.CommunityOwner == null ? 0 : item.CommunityOwner.Id;
        //            Manager.BeginTransaction();
        //            string fileName = BaseFilePath + "\\{0}\\{1}.stored";
        //            List<TaskListFile> files = (from f in Manager.GetIQ<TaskListFile>() where f.TaskOwner == item select f).ToList();
        //            List<ModuleLink> links = (from f in files where f.Link != null select f.Link).ToList();
        //            filesToRemove = (from f in files where f.File != null && f.File.IsInternal select string.Format(fileName, IdCommunity, f.File.UniqueID.ToString())).ToList();
        //            Manager.DeletePhysicalList(links);
        //            Manager.DeletePhysicalList((from f in files where f.File != null && f.File.IsInternal select f).ToList());
        //            CommunityEvent eventOwner = item.EventOwner;
        //            eventOwner.Items.Remove(item);

        //            Manager.DeletePhysical(item);
        //            if (eventOwner.Items.Count > 0)
        //                Manager.SaveOrUpdate(eventOwner);
        //            else
        //                Manager.DeletePhysical(eventOwner);


        //            Manager.Commit();
        //            deleted = true;
        //            foreach (String name in filesToRemove)
        //            {
        //                try
        //                {
        //                    System.IO.File.Delete(name);
        //                }
        //                catch (Exception exFile)
        //                {

        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return deleted;
        //}
        #endregion

        #endregion
    }
}