using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
using lm.Comol.Modules.TaskList.Business;
using lm.Comol.Modules.TaskList.Domain;
using lm.Comol.Core.Business;
using lm.Comol.Core.File;

namespace lm.Comol.Modules.TaskList
{
    public class ServiceTaskList : CoreTaskService, iLinkedService
    {
        private const string UniqueCode = "SRVTASK";
        private iApplicationContext _Context;

        #region initClass
        public ServiceTaskList() { }
        public ServiceTaskList(iApplicationContext oContext)
        {
            this.Manager = new BaseModuleManager(oContext.DataContext);
            _Context = oContext;
            this.UC = oContext.UserContext;
        }
        public ServiceTaskList(iDataContext oDC)
        {
            this.Manager = new BaseModuleManager(oDC);
            _Context = new ApplicationContext();
            _Context.DataContext = oDC;
            this.UC = null;
        }

        #endregion



        public int ServiceModuleID()
        {
            return this.Manager.GetModuleID(UniqueCode);
        }

        #region iLinkedService
        public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<ModuleLink> links, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            return new List<dtoItemEvaluation<long>>();
        }
        public void SaveActionExecution(ModuleLink link, Boolean isStarted, Boolean isPassed, short Completion, Boolean isCompleted, Int16 mark, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
        }
        public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {

        }
        public dtoEvaluation EvaluateModuleLink(ModuleLink link, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            return null;
        }
        public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Int32 idCommunity, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            List<StandardActionType> actions = new List<StandardActionType>();
            switch (source.ObjectTypeID)
            {
                case (int)ModuleTasklist.ObjectType.TaskFile:
                case (int)ModuleTasklist.ObjectType.TaskLinkedFile:
                    actions = GetAllowedStandardActionForFile(idUser, source, destination);
                    break;
            }

            return actions;
        }
        private List<StandardActionType> GetAllowedStandardActionForFile(int idUser, ModuleObject source, ModuleObject destination)
        {
            List<StandardActionType> actions = new List<StandardActionType>();
            Person person = Manager.GetPerson(idUser);
            TaskListFile taskFile = Manager.Get<TaskListFile>(source.ObjectLongID);
            if (taskFile != null && taskFile.Link != null && destination.ObjectLongID == taskFile.File.Id && destination.FQN == taskFile.File.GetType().FullName)
            {
                int IdCommunity = taskFile.CommunityOwner == null ? 0 : taskFile.CommunityOwner.Id;
                ModuleTasklist modulePermission = ServicePermission(idUser, IdCommunity);
                CoreModuleRepository moduleRepository = GetCoreModuleRepository(idUser, IdCommunity);
                CoreItemPermission itemPermission = GetTaskPermission(person, taskFile.TaskOwner, modulePermission, moduleRepository);
                if (taskFile.File != null)
                {
                    if ((taskFile.File.IsInternal && itemPermission.AllowEdit) ||
                      (!taskFile.File.IsInternal && (moduleRepository.Administration || (moduleRepository.Edit && taskFile.File.Owner == person))))
                        actions.Add(StandardActionType.EditMetadata);
                    if (AllowViewFileFromLink(modulePermission, itemPermission, taskFile, person))
                    {
                        actions.Add(StandardActionType.Play);
                        actions.Add(StandardActionType.ViewPersonalStatistics);
                    }
                    if (taskFile.File.Owner == person || (taskFile.File.IsInternal && itemPermission.AllowEdit) || (!taskFile.File.IsInternal && (moduleRepository.Administration || moduleRepository.Edit)))
                        actions.Add(StandardActionType.ViewAdvancedStatistics);
                }
            }
            return actions;
        }
        public bool AllowActionExecution(ModuleLink link, Int32 idUser, Int32 idCommunity, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            Person person = Manager.GetPerson(idUser);
            switch (link.SourceItem.ObjectTypeID)
            {
                case (int)ModuleTasklist.ObjectType.Project:
                    return AllowViewProject(idUser, idCommunity, idRole);
                case (int)ModuleTasklist.ObjectType.Task:
                    return AllowViewTask(link.SourceItem.ObjectLongID, idUser, idCommunity, idRole);
                case (int)ModuleTasklist.ObjectType.TaskFile:
                    return AllowDownloadFileOfItemDescription(link.SourceItem.ObjectLongID, idUser, idCommunity, idRole);
                case (int)ModuleTasklist.ObjectType.TaskLinkedFile:
                    return AllowDownloadFileLinkedToItem(link.SourceItem.ObjectLongID, idUser, idCommunity, idRole);
                default:
                    return false;
            }
        }
        public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            Boolean iResponse = false;
            switch (actionType)
            {
                case StandardActionType.EditMetadata:
                    iResponse = AllowEditMetadata(idUser, idRole, source, destination);
                    break;
                case StandardActionType.EditPermission:
                    break;
                case StandardActionType.ViewAdvancedStatistics:
                    break;
                case StandardActionType.ViewPersonalStatistics:
                    break;
            }
            return iResponse;
        }
        private Boolean AllowEditMetadata(int UserID, int RoleID, ModuleObject source, ModuleObject destination)
        {
            Boolean iResponse = false;
            if (source.ObjectTypeID == (int)ModuleTasklist.ObjectType.TaskLinkedFile)
            {
                Person person = Manager.GetPerson(UserID);
                TaskListFile taskFile = Manager.Get<TaskListFile>(source.ObjectLongID);
                if (taskFile != null && taskFile.Link != null && destination.ObjectLongID == taskFile.File.Id && destination.FQN == taskFile.File.GetType().FullName)
                {
                    int IdCommunity = taskFile.CommunityOwner == null ? 0 : taskFile.CommunityOwner.Id;
                    ModuleTasklist modulePermission = ServicePermission(UserID, IdCommunity);
                    CoreModuleRepository moduleRepository = GetCoreModuleRepository(UserID, IdCommunity);
                    CoreItemPermission itemPermission = GetTaskPermission(person, taskFile.TaskOwner, modulePermission, moduleRepository);
                    iResponse = (taskFile.File.IsInternal && itemPermission.AllowEdit) ||
                        (!taskFile.File.IsInternal && (moduleRepository.Administration || (moduleRepository.Edit && taskFile.File.Owner == person)));
                }
            }
            return iResponse;
        }
        private Boolean AllowViewProject(int UserID, int CommunityID, int RoleID)
        {
            Boolean iResponse = false;
            long permission = (long)ModuleTasklist.Base2Permission.ViewCommunityProjects | (long)ModuleTasklist.Base2Permission.Administration;
            iResponse = Manager.HasModulePermission(UserID, RoleID, CommunityID, this.ServiceModuleID(), permission);
            return iResponse;
        }
        private Boolean AllowViewTask(long itemId, int UserID, int communityID, int RoleID)
        {
            Boolean iResponse = false;
            Task item = GetTask(itemId);
            Person person = Manager.GetPerson(UserID);
            if (item != null)
            {
                communityID = item.Community == null ? 0 : item.Community.Id;
                lm.Comol.Modules.TaskList.ModuleTasklist modulePermission = ServicePermission(UserID, communityID);
                CoreModuleRepository moduleRepository = GetCoreModuleRepository(UserID, communityID);
                CoreItemPermission itemPermission = GetTaskPermission(person, item, modulePermission, moduleRepository);
                iResponse = (itemPermission.AllowView);
            }
            return iResponse;
        }
        private Boolean AllowDownloadFileOfItemDescription(long itemLinkId, int UserID, int CommunityID, int RoleID)
        {
            return AllowViewTask(itemLinkId, UserID, CommunityID, RoleID);
        }
        private Boolean AllowDownloadFileLinkedToItem(long itemFileLinkId, int UserID, int communityID, int RoleID)
        {
            Boolean iResponse = false;
            TaskListFile taskFile = Manager.Get<TaskListFile>(itemFileLinkId);
            Person person = Manager.GetPerson(UserID);
            if (taskFile != null && taskFile.TaskOwner != null && taskFile.File != null && taskFile.Link != null)
            {
                Task task = taskFile.TaskOwner;
                communityID = task.Community == null ? 0 : task.Community.Id;
                lm.Comol.Modules.TaskList.ModuleTasklist modulePermission = ServicePermission(UserID, communityID);
                CoreModuleRepository moduleRepository = GetCoreModuleRepository(UserID, communityID);
                CoreItemPermission itemPermission = GetTaskPermission(person, task, modulePermission, moduleRepository);

                //permission.Download = itemFileLink.File.IsDownloadable && itemPermissions.AllowViewFiles;
                //permission.Play = (itemFileLink.File.isSCORM || itemFileLink.File.isVideocast) && itemPermissions.AllowViewFiles;

                iResponse = AllowViewFileFromLink(modulePermission, itemPermission, taskFile, person);
            }
            return iResponse;
        }

        private Boolean AllowViewFileFromLink(ModuleTasklist modulePermission, CoreItemPermission itemPermission, TaskListFile taskFile, Person person)
        {
            Boolean iResponse = false;
            iResponse = itemPermission.AllowViewFiles && (taskFile.isVisible || taskFile.Owner == person || taskFile.TaskOwner.MetaInfo.CreatedBy == person || modulePermission.Administration);
            return iResponse;
        }

        public void PhisicalDeleteCommunity(Int32 idCommunity, Int32 idUser, String baseFilePath, String baseThumbnailPath)
        {
            PhisicalDeleteTask(idCommunity, baseFilePath);
        }
        public void PhisicalDeleteRepositoryItem(long idFileItem, Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {

        }

        public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(long objectId, Int32 objectTypeId, Dictionary<Int32, string> translations, Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            StatTreeNode<StatFileTreeLeaf> node = null;
            Person person = Manager.Get<Person>(idUser);

            switch (objectTypeId)
            {
                case (int)ModuleTasklist.ObjectType.Task:
                    Task item = Manager.Get<Task>(objectId);
                    if (item != null)
                        idCommunity = (item.Community == null) ? 0 : item.Community.Id;
                    break;
                case (int)ModuleTasklist.ObjectType.TaskFile:
                    //item = Manager.Get<CommunityEventItem>(objectId);
                    //if (item != null)
                    //    IdCommunity = (item.CommunityOwner == null) ? 0 : item.CommunityOwner.Id;
                    break;
                case (int)ModuleTasklist.ObjectType.TaskLinkedFile:
                    TaskListFile taskFile = Manager.Get<TaskListFile>(objectTypeId);
                    if (taskFile != null)
                        idCommunity = (taskFile.CommunityOwner == null) ? 0 : taskFile.CommunityOwner.Id;
                    break;
            }
            //ModuleTasklist moduleTasklist = ServicePermission(IdUser, IdCommunity);
            //if (moduleTasklist.Administration || moduleTasklist.ViewTaskList || moduleTasklist.UploadFile) //moduleTasklist.Edit ||
            //{
            //    CoreModuleRepository repository = GetCoreModuleRepository(IdUser, IdCommunity);
            //    switch (objectTypeId)
            //    {
            //        case (int)ModuleTasklist.ObjectType.Task:
            //            Task item = Manager.Get<Task>(objectId);
            //            node = LoadDiaryItemForStatistics(item, person, moduleTasklist, repository, translations);
            //            break;
            //        case (int)ModuleTasklist.ObjectType.TaskLinkedFile:
            //            TaskListFile eventItemFile = Manager.Get<TaskListFile>(objectTypeId);
            //            if (eventItemFile != null && eventItemFile.ItemOwner != null)
            //                node = LoadDiaryItemForStatistics(eventItemFile.ItemOwner, person, moduleTasklist, repository, translations);
            //            break;
            //        default:

            //            node = LoadDiaryForStatistics(Manager.Get<Community>(IdCommunity), person, moduleTasklist, repository, translations);
            //            break;
            //    }
            //}
            //else
            //    node = CreateTaskListTreeNode(Manager.Get<Community>(IdCommunity), translations); 
            return node;
        }

        #region "Load Files for statistics"
        //private StatTreeNode<StatFileTreeLeaf> LoadDiaryForStatistics(Community community, Person person, ModuleTasklist moduleDiary, CoreModuleRepository repository, Dictionary<int, string> translations)
        //    {
        //        StatTreeNode<StatFileTreeLeaf> rootNode = CreateDiaryTreeNode(community, translations);
        //        List<dtoTask> items = GetDtoTasksForStatistics(community, person, moduleDiary,repository,moduleDiary.Administration);
        //        foreach (dtoTask item in items)
        //        {
        //            StatTreeNode<StatFileTreeLeaf> itemNode = CreateDiaryItemTreeNode(item.EventItem,item.LessonNumber,translations);
        //            itemNode.Leaves = (from i in item.FileLinks where i.ItemFileLink !=null && i.ItemFileLink.File!=null select CreateStatFileTreeLeaf(i.ItemFileLink.File, i.Permission.ViewPersonalStatistics, i.Permission.ViewStatistics)).ToList();
        //            rootNode.Nodes.Add(itemNode);
        //        }
        //        return rootNode;
        //    }
        //private StatTreeNode<StatFileTreeLeaf> LoadDiaryItemForStatistics(CommunityEventItem item, Person person, ModuleTasklist moduleDiary, CoreModuleRepository repository, Dictionary<int, string> translations)
        //    {
        //        StatTreeNode<StatFileTreeLeaf> rootNode = CreateDiaryItemTreeNode(item,-1, translations);
        //        int lesson = -1;
        //        dtoDiaryItem dtoItem = CreateDtoTaskForStatistics(person,item,moduleDiary.Administration,moduleDiary,repository,ref lesson);
        //        rootNode.Leaves = (from i in dtoItem.FileLinks where i.ItemFileLink !=null && i.ItemFileLink.File!=null select CreateStatFileTreeLeaf(i.ItemFileLink.File, i.Permission.ViewPersonalStatistics, i.Permission.ViewStatistics)).ToList();
        //        return rootNode;
        //    }
        private StatTreeNode<StatFileTreeLeaf> CreateTaskListTreeNode(Community community, Dictionary<int, string> translations)
        {
            StatTreeNode<StatFileTreeLeaf> node = new StatTreeNode<StatFileTreeLeaf>() { Id = 0, isVisible = true, NodeObjectTypeId = (int)ModuleTasklist.ObjectType.Project };
            node.Name = (community == null) ? translations[(int)TreeItemsTranslations.PortalDiaryName] : string.Format(translations[(int)TreeItemsTranslations.DiaryName], community.Name);
            node.ToolTip = (community == null) ? translations[(int)TreeItemsTranslations.PortalDiaryNameToolTip] : string.Format(translations[(int)TreeItemsTranslations.DiaryNameToolTip], community.Name);

            return node;
        }
        private StatTreeNode<StatFileTreeLeaf> CreateTaskTreeNode(CommunityEventItem item, int lessonNumber, Dictionary<int, string> translations)
        {
            StatTreeNode<StatFileTreeLeaf> node = new StatTreeNode<StatFileTreeLeaf>() { Id = 0, isVisible = true, NodeObjectTypeId = (int)ModuleTasklist.ObjectType.Task };

            node.Name = (item.ShowDateInfo) ?
                        string.Format(translations[(int)TreeItemsTranslations.StandardDiaryItemName], (lessonNumber < 1) ? "" : lessonNumber.ToString())
                        :
                        string.Format(translations[(int)TreeItemsTranslations.NoDateDiaryItemName], (lessonNumber < 1) ? "" : lessonNumber.ToString());
            node.ToolTip = (item.ShowDateInfo) ?
                        string.Format(translations[(int)TreeItemsTranslations.StandardDiaryItemNameToolTip], (lessonNumber < 1) ? "" : lessonNumber.ToString(), item.StartDate.ToString("dd/MM/YY"), item.StartDate.ToString("hh:mm"), item.EndDate.ToString("hh:mm"))
                        :
                        string.Format(translations[(int)TreeItemsTranslations.NoDateDiaryItemNameToolTip], (lessonNumber < 1) ? "" : lessonNumber.ToString());

            return node;
        }
        private StatFileTreeLeaf CreateStatFileTreeLeaf(BaseCommunityFile item, Boolean viewPersonal, Boolean viewAdvanced)
        {
            StatFileTreeLeaf leaf = new StatFileTreeLeaf()
            {
                Id = item.Id,
                Extension = item.Extension,
                isVisible = item.isVisible,
                LinkId = 0,
                Name = item.DisplayName,
                ToolTip = "",
                isScorm = item.isSCORM,
                UniqueID = item.UniqueID,
                DownloadCount = item.Downloads,
                Type = (viewPersonal) ? StatTreeLeafType.Personal : StatTreeLeafType.None
            };
            if ((viewAdvanced))
                leaf.Type = (leaf.Type == StatTreeLeafType.None) ? StatTreeLeafType.Advanced : leaf.Type | StatTreeLeafType.Advanced;
            return leaf;
        }
        private List<dtoTask> GetDtoTasksForStatistics(Community community, Person person, ModuleTasklist module, CoreModuleRepository repository, Boolean allVisibleItems)
        {
            List<dtoTask> items = new List<dtoTask>();
            ////CommunityEventType eventType = GetDiaryEventType();
            //if (community != null && person != null)
            //{
            //    int lessonNumber = 1;
            //    items = (from item in CommunityTasksQuery(community, person, allVisibleItems).ToList()
            //             select CreateDtoTaskForStatistics(person, item, allVisibleItems, module, repository, ref lessonNumber)).ToList();
            //}
            return items;
        }
        #endregion


        #endregion

        #region "Permission"
        public lm.Comol.Modules.TaskList.ModuleTasklist ServicePermission(int personId, int communityId)
        {
            ModuleTasklist module = new ModuleTasklist();
            Person person = Manager.GetPerson(personId);
            if (communityId == 0)
                module = ModuleTasklist.CreatePortalmodule(person.TypeID);
            else
            {
                module = new ModuleTasklist(Manager.GetModulePermission(personId, communityId, ServiceModuleID()));
            }
            return module;
        }
        public CoreModuleRepository GetCoreModuleRepository(int personId, int communityId)
        {
            CoreModuleRepository module = new CoreModuleRepository();
            Person person = Manager.GetPerson(personId);
            if (communityId == 0)
                module = CoreModuleRepository.CreatePortalmodule(person.TypeID);
            else
            {
                module = new CoreModuleRepository(Manager.GetModulePermission(personId, communityId, Manager.GetModuleID(CoreModuleRepository.UniqueID)));
            }
            return module;
        }

        #endregion

        #region "Management Item"
        //Public Function GetDiaryItemsListWithPermission(ByVal UserID As Integer, ByVal CommunityID As Integer, ByVal ModulePermission As ModuleCommunityDiary, ByVal Ascending As Boolean, ByVal ItemVisibility As ObjectStatus) As List(Of dtoDiaryItem)
        //Dim oReturnList As List(Of dtoDiaryItem) = New List(Of dtoDiaryItem)
        //If CommunityID > 0 AndAlso Not oCommunity Is Nothing OrElse CommunityID = 0 Then
        //    Dim oList As List(Of CommunityEventItem) = Me.GetDiaryItems(oCommunity, oPerson, ItemVisibility)
        //    If Not IsNothing(oList) AndAlso oList.Count > 0 Then
        //        Dim LessionID As Integer = 1

        //        oReturnList = (From di In oList Select Me.CreateDTOdiaryItem(oPerson, di, ModulePermission, LessionID)).ToList

        //        If Not Ascending Then
        //            oReturnList = oReturnList.OrderByDescending(Function(i) i.LessonNumber).ToList
        //        End If
        //    End If

        //End If
        //Return oReturnList

        public List<dtoDiaryItem> GetDtoDiaryItems(int idCommunity, Boolean ascendingLesson, ModuleTasklist module, CoreModuleRepository repository, Boolean allVisibleItems)
        {
            List<dtoDiaryItem> items = new List<dtoDiaryItem>();
            //Person person = Manager.GetPerson(UC.CurrentUserID);
            ////CommunityEventType eventType = GetDiaryEventType();
            //Community community = Manager.GetCommunity(idCommunity);
            //if (community!=null && person != null){
            //    int lessnoNumber = 1;
            //    items = ( from item in CommunityTasksQuery(community ,person,allVisibleItems).ToList() 
            //              select CreateDtoDiaryItem(person,item,allVisibleItems,module,repository,ref lessnoNumber)).ToList();
            //    if (!ascendingLesson)
            //        items = items.OrderByDescending(i=> i.LessonNumber).ToList();
            //}
            return items;
        }
        public List<Task> GetDiaryItems(Community community, Person person, Boolean allVisibleItems)
        {
            //CommunityEventType eventType = GetDiaryEventType();
            //var query = (from item in Manager.GetAll<CommunityEventItem>(i => i.CommunityOwner == community && i.EventOwner != null && i.EventOwner.EventType == eventType
            //                 && (allVisibleItems || (i.IsVisible || (!i.IsVisible && i.Owner == person))))
            //             orderby item.StartDate, item.CreatedOn 
            //             select item);
            return CommunityTasksQuery(community, person, allVisibleItems).ToList();
        }

        private dtoTask CreateDtoTask(Person person, Task item, Boolean allVisibleItems, ModuleTasklist module, CoreModuleRepository repository, ref int lessionID)
        {
            dtoTask dtoItem = new dtoTask();
            dtoItem.CommunityId = (item.Community == null ? 0 : item.Community.Id);
            dtoItem.Description = (from d in Manager.Linq<DescriptionEventItem>() where d.Id == item.ID select d.Description).FirstOrDefault();
            dtoItem.oTask = item;
            dtoItem.Permission = GetTaskPermission(person, item, module, repository);
            dtoItem.FileLinks = GetCoreItemFileLinkPermission(dtoItem.Permission, (from fl in GetTaskFiles(item, false) where (allVisibleItems || (fl.isVisible || (!fl.isVisible && fl.Owner == person))) select fl).ToList(), null, person);
            dtoItem.Id = item.ID;
            //dtoItem.LessonNumber = lessionID;


            // // Posso vedere la voce 
            // iResponse.Permission = new CoreItemPermission();
            // //iResponse.Permission.Delete = ModulePermission.Administration || (ModulePermission.DeleteItem && isItemOwner);
            // iResponse.Permission.AllowViewFiles = module.Administration || module.ViewDiaryItems;
            // iResponse.Permission.AllowEdit = module.Administration || (module.Edit && isItemOwner);
            //// iResponse.Permission.UnDelete = ModulePermission.Administration || (ModulePermission.DeleteItem && isItemOwner);
            // iResponse.Permission.AllowView = module.Administration || module.ViewDiaryItems || (module.Edit && isItemOwner);

            // TO INSERT FOR VIRTUAL DELETE !!

            // iResponse.EventItem = item;
            // iResponse.LessonNumber = lessionID;

            lessionID += 1;
            return dtoItem;
            //oHeaderTitle,
        }
        private dtoTask CreateDtoTaskForStatistics(Person person, Task item, Boolean allVisibleItems, ModuleTasklist module, CoreModuleRepository repository)
        {
            dtoTask dtoItem = new dtoTask();
            dtoItem.oTask.Project = item;
            dtoItem.Permission = GetTaskPermission(person, item, module, repository);
            dtoItem.FileLinks = GetCoreItemFileLinkPermission(dtoItem.Permission, (from fl in GetTaskFiles(item, false) where (allVisibleItems || (fl.isVisible || (!fl.isVisible && fl.Owner == person))) select fl).ToList(), null, person);
            dtoItem.Id = item.ID;
            //dtoItem.LessonNumber = lessionID;

            //lessionID += 1;
            return dtoItem;
        }
        public CoreItemPermission GetItemPermissionFromLink(long IdLink)
        {
            CoreItemPermission permission = new CoreItemPermission();
            ModuleLink link = Manager.Get<ModuleLink>(IdLink);
            if (link == null)
                return permission;
            else
            {
                TaskListFile taskFileLink = (from ifl in Manager.GetAll<TaskListFile>(ifl => ifl.Link == link && ifl.Deleted == BaseStatusDeleted.None) select ifl).Skip(0).Take(1).ToList().FirstOrDefault();
                if (taskFileLink == null || taskFileLink.TaskOwner == null)
                    return permission;
                else
                {
                    int IdCommunity = taskFileLink.TaskOwner.Community == null ? 0 : taskFileLink.TaskOwner.Community.Id;
                    ModuleTasklist moduleDiary = ServicePermission(UC.CurrentUserID, IdCommunity);
                    CoreModuleRepository repository = GetCoreModuleRepository(UC.CurrentUserID, IdCommunity);
                    permission = GetTaskPermission(taskFileLink.TaskOwner, moduleDiary, repository);
                    return permission;
                }
            }
        }
        public int GetCommunityIdFromTaskLink(long IdLink)
        {
            int IdCommunity = -1;
            ModuleLink link = Manager.Get<ModuleLink>(IdLink);
            if (link != null)
            {
                Community community = (from ifl in Manager.GetAll<TaskListFile>(f => f.Link == link) select ifl.CommunityOwner).Skip(0).Take(1).ToList().FirstOrDefault();
                if (community != null)
                    IdCommunity = community.Id;
            }
            return IdCommunity;
        }
        public CoreItemPermission GetTaskPermission(Task oTask, ModuleTasklist Permission, CoreModuleRepository repository)
        {
            Person oPerson = Manager.GetPerson(UC.CurrentUserID);
            return GetTaskPermission(oPerson, oTask, Permission, repository);
        }

        public CoreItemPermission GetTaskPermission(Person person, Task oItem, ModuleTasklist Permission, CoreModuleRepository repository)
        {
            CoreItemPermission iResponse = new CoreItemPermission();

            iResponse.AllowView = Permission.Administration || Permission.ViewTaskList;
            iResponse.AllowViewFiles = (Permission.ViewTaskList && (oItem.MetaInfo.CreatedBy == person)) || Permission.Administration;
            //iResponse. = Permission.Administration;
            iResponse.AllowAddFiles = (oItem.MetaInfo.CreatedBy == person) || Permission.Administration || Permission.UploadFile;
            iResponse.AllowDelete = (oItem.MetaInfo.CreatedBy == person) || Permission.Administration;
            iResponse.AllowEdit = (oItem.MetaInfo.CreatedBy == person) || Permission.Administration;
            iResponse.AllowVirtualDelete = iResponse.AllowDelete;
            iResponse.AllowUnDelete = (oItem.MetaInfo.CreatedBy == person) || Permission.Administration;
            iResponse.AllowViewStatistics = iResponse.AllowViewFiles;
            iResponse.AllowFilePublish = (repository != null && (repository.Administration || repository.UploadFile));
            return iResponse;
        }

        //public CommunityEventType GetDiaryEventType()
        //{
        //   return Manager.Get<CommunityEventType>((int)StandardEventType.Lesson);
        //}

        public Boolean PhisicalDeleteTask(int IdCommunity, String BaseFilePath)
        {
            Boolean deleted = false;
            try
            {
                //CommunityEventType eventType = GetDiaryEventType();
                Manager.BeginTransaction();
                Community community = Manager.GetCommunity(IdCommunity);
                List<String> filesToRemove = new List<String>();
                string fileName = BaseFilePath + IdCommunity.ToString() + "\\{0}\\{1}.stored";
                if (community != null || IdCommunity == 0)
                {
                    List<Task> tasks = (from e in Manager.GetIQ<Task>() where e.Community == community select e).ToList();
                    List<TaskListFile> files = (from f in Manager.GetIQ<TaskListFile>() where f.CommunityOwner == community select f).ToList();
                    List<ModuleLink> links = (from f in files where f.Link != null select f.Link).ToList();
                    filesToRemove = (from f in files where f.File != null && f.File.IsInternal select string.Format(fileName, IdCommunity, f.File.UniqueID.ToString())).ToList();
                    Manager.DeletePhysicalList(links);
                    Manager.DeletePhysicalList((from f in files where f.File != null && f.File.IsInternal select f).ToList());
                    Manager.DeletePhysicalList(tasks);
                }
                Manager.Commit();

                Delete.Files(filesToRemove);

            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }
            return deleted;
        }
        #endregion



        //internal CommunityEventItem GetEventItem(long preloadedItemId)
        //{
        //    throw new NotImplementedException();
        //}


    }
}
