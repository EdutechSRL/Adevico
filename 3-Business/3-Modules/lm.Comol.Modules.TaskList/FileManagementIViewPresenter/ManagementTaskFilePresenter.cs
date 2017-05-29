using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
using lm.Comol.Core.BaseModules.CommunityDiary.Business;
using lm.Comol.Modules.TaskList.Domain;
using COL_BusinessLogic_v2.UCServices;
using lm.Comol.Core.BaseModules;
using lm.Comol.Core.Business;
using lm.Comol.Core.Event;


namespace lm.Comol.Modules.TaskList
{
    public class ManagementTaskFilePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private int _ModuleID;
        private lm.Comol.Modules.TaskList.ServiceTaskList _Service;

        private int ModuleID
        {
            get
            {
                if (_ModuleID <= 0)
                {
                    _ModuleID = this.Service.ServiceModuleID();
                }
                return _ModuleID;
            }
        }

        public virtual BaseModuleManager CurrentManager { get; set; }

        protected virtual lm.Comol.Modules.TaskList.IViewManagementTaskFile View
        {
            get { return (lm.Comol.Modules.TaskList.IViewManagementTaskFile)base.View; }
        }

        private lm.Comol.Modules.TaskList.ServiceTaskList Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceTaskList(AppContext);
                return _Service;
            }
        }

        public ManagementTaskFilePresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public ManagementTaskFilePresenter(iApplicationContext oContext, IViewManagementTaskFile view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView()
        {
            int CommunityId = UserContext.CurrentCommunityID;
            if (UserContext.isAnonymous)
            {
                View.NoPermissionToManagementFiles(CommunityId,ModuleID);
            }

            else{
                //CommunityEventItem oItem = Service.GetEventItem(this.View.PreloadedItemID);
                Task oTask = Service.GetTask(this.View.PreloadedItemID);
                if (oTask == null)
                    this.View.NoPermissionToManagementFiles(CommunityId, this.ModuleID);
                else
                {
                    View.ItemID = oTask.ID;
                    lm.Comol.Modules.TaskList.ModuleTasklist  module = new  ModuleTasklist();
                    if ((oTask.Community  != null)){
                        CommunityId = oTask.Community.Id;
                        module = (from p in View.CommunitiesPermission where p.ID == oTask.Community.Id select p.Permissions).FirstOrDefault();
                        if (module==null)
                            module = new  ModuleTasklist();
                    }
                    else{
                        CommunityId = 0;
                        module = ModuleTasklist.CreatePortalmodule(UserContext.UserTypeID);
                    }

                    CoreModuleRepository moduleRepository = View.RepositoryPermission(CommunityId);
                    CoreItemPermission oPermission = Service.GetTaskPermission(oTask, module, moduleRepository);
                    if (oPermission.AllowAddFiles || oPermission.AllowEdit)
                    {
                        if (oTask.Community  == null)
                        {
                            View.AllowCommunityUpload = false;
                            View.AllowCommunityLink = false;
                        }
                        View.AllowCommunityUpload = oPermission.AllowEdit && (moduleRepository.Administration || moduleRepository.UploadFile);
                        View.AllowCommunityLink = oPermission.AllowEdit && (moduleRepository.Administration || moduleRepository.UploadFile || moduleRepository.ListFiles || moduleRepository.DownLoad);
                        if (oPermission.AllowEdit && (moduleRepository.Administration || moduleRepository.UploadFile))
                        {
                            View.InitializeCommunityUploader(0, CommunityId, moduleRepository);
                        }
                        if (oPermission.AllowEdit) {
                            View.InitializeModuleUploader(CommunityId);
                        }
                        View.AllowUpload = oPermission.AllowAddFiles;
                      //  this.View.BackToDiary = CommunityID;
                        View.SetBackToItemsUrl(CommunityId, oTask.ID);
                        View.SetBackToItemUrl(CommunityId, oTask.ID);
                        View.SetMultipleUploadUrl(oTask.ID);
                        LoadTaskFiles(oTask, oPermission);
                    }
                    else
                    {
                        this.View.ReturnToItemsList(CommunityId, oTask.ID);
                    }
                }

            }
            View.ItemCommunityId = CommunityId;
        }

       
        //Public Sub AddInternalFile(ByVal InternalFiles As List(Of lm.Comol.Core.DomainModel.BaseFile))
        //    Dim oItem As CommunityEventItem = Me.CurrentManager.GetEventItem(Me.View.ItemID)
        //    If IsNothing(oItem) OrElse IsNothing(InternalFiles) OrElse InternalFiles.Count = 0 Then
        //        Me.View.ReturnToFileManagement(Me.View.ItemID)
        //    Else
        //        Dim CommunityId As Integer = 0
        //        If Not oItem.CommunityOwner Is Nothing Then
        //            CommunityId = oItem.CommunityOwner.Id
        //        End If
        //        Me.CurrentManager.AddInternalFilesToItem(Me.View.ItemID, InternalFiles, Me.UserContext.CurrentUserID)
        //        Me.View.AddInternalFileAction(CommunityId, Me.ModuleID)

        //        If InternalFiles.Count > 0 Then
        //            '  Me.View.NotifyAddInternalFile(isPersonal, CommunityId, oItem.WorkBookOwner.Id, oItem.WorkBookOwner.Title, oItem.Id, oItem.Title, oItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
        //        End If

                
        //    End If
        //End Sub
        public void ReloadManagementFileView() {
            int CommunityId = UserContext.CurrentCommunityID;
            if (UserContext.isAnonymous)
                View.NoPermissionToManagementFiles(CommunityId, ModuleID);
            else { 
            
            }
        }
      

        #region "File Management"
            public void LoadTaskFiles(Task task,CoreItemPermission moduleTask)
            {
               //  = View.RepositoryPermission(CommunityId);
                //    CoreItemPermission oPermission
                IList<iCoreItemFileLink<long>> files = Service.GetTaskFiles(task, moduleTask.AllowEdit);
                View.LoadFilesToManage(task.ID, moduleTask, files, lm.Comol.Modules.TaskList.Domain.RootObject.PublishUrl());
            
            }
            public void AddInternalFile()
            {
                Task oTask = Service.GetTask(View.ItemID);
                if (oTask == null)
                    View.ReturnToItemsList(View.ItemCommunityId, View.ItemID);
                else
                {
                    int communityId =0;
                    ModuleActionLink actionLink = View.GetUploadedModuleFile(oTask, (int)ModuleTasklist.ObjectType.Task, ModuleTasklist.UniqueID, (int)ModuleTasklist.ActionType.DownloadTaskFile, ModuleID);
                    try
                    {
                        Service.SaveTaskListFile(oTask, View.ItemCommunityId, actionLink, ModuleID, (int)ModuleTasklist.ObjectType.TaskLinkedFile, ModuleTasklist.UniqueID,false );
                        if (oTask.Community != null)
                            communityId = oTask.Community.Id;
                        View.AddModuleFileAction(communityId, ModuleID);
                    }
                    catch (EventItemFileNotLinked ex)
                    {

                    }
                    View.ReturnToFileManagement(communityId,oTask.ID);
                }
            }
            public void AddCommunityFile(MultipleUploadResult<dtoUploadedFile> uploadedFiles, Boolean isForAllMembers)
            {
                Task oTask = Service.GetTask(View.ItemID);
                if (oTask == null || uploadedFiles == null || uploadedFiles.UploadedFile.Count==0){
                    if (uploadedFiles.UploadedFile.Count==0)
                        View.ReturnToItemsList(View.ItemCommunityId, View.ItemID);
                    else
                        View.ReturnToFileManagementWithErrors(null, uploadedFiles.NotuploadedFile);
                    }  
                else
                {
                    int communityId=0;
                    if (oTask.Community !=null)
                        communityId =oTask.Community.Id;

                    IList<ModuleActionLink> actionLinks = (from f in uploadedFiles.UploadedFile select f.Link).ToList();
                    Service.SaveTaskListFiles(oTask, View.ItemCommunityId, actionLinks, ModuleID, (int)ModuleTasklist.ObjectType.TaskLinkedFile, ModuleTasklist.UniqueID, false);
                    View.AddCommunityFileAction(communityId,ModuleID);
                  //  If CommunityFiles.UploadedFile.Count > 0 Then
                 //       '    Me.View.NotifyAddCommunityFile(isPersonal, CommunityId, oItem.WorkBookOwner.Id, oItem.WorkBookOwner.Title, oItem.Id, oItem.Title, oItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                 //   End If
                    if (uploadedFiles.NotuploadedFile.Count==0){
                        if (isForAllMembers)
                            View.ReturnToFileManagement(communityId,oTask.ID);
                        else
                            View.LoadEditingPermission(uploadedFiles.UploadedFile[0].File.Id, communityId, uploadedFiles.UploadedFile[0].File.FolderId, oTask.ID);
                    }
                    else
                        View.ReturnToFileManagementWithErrors(null, uploadedFiles.NotuploadedFile);
                }
            }

            public void UnlinkRepositoryItem(long itemID, long itemLinkId){
                Service.UnLinkToCommunityFileFromTask(itemLinkId);
                View.ReturnToFileManagement(View.ItemCommunityId,itemID);
            }
            public void VirtualDelete(long itemID, long itemLinkId){
                Service.VirtualDeleteFileEventItemLink(itemLinkId);
                View.ReturnToFileManagement(View.ItemCommunityId,itemID);
            }
            public void VirtualUndelete(long itemID, long itemLinkId){
                Service.VirtualUndeleteFileItemLink(itemLinkId);
                View.ReturnToFileManagement(View.ItemCommunityId,itemID);
            }
            public void PhisicalDelete(long itemID, long itemLinkId, String BaseFilePath){
                Service.PhisicalDeleteFileTask(itemID, itemLinkId, BaseFilePath);
                View.ReturnToFileManagement(View.ItemCommunityId,itemID);
            }
            public void EditFileVisibility(long itemID, long itemLinkId)
            {
                Service.EditModuleFileTaskVisibility(itemLinkId);
                View.ReturnToFileManagement(View.ItemCommunityId, itemID);
            }
            public void EditFileRepositoryVisibility(long itemID, long itemLinkId)
            {
                Service.EditFileRepositoryVisibility(itemLinkId);
                View.ReturnToFileManagement(View.ItemCommunityId, itemID);
            }
            public void EditFileItemVisibility(long itemID, long LinkId, Boolean visibleForModule, Boolean visibleForRepository)
            {
                int IdCommunity = View.ItemCommunityId;
                TaskListFile fileLink = Service.GetTaskListFile(LinkId);
                if (fileLink != null && fileLink.TaskOwner != null)
                {
                    Service.EditTaskListFileVisibility(fileLink, visibleForModule, visibleForRepository);
                    //if (fileLink.ItemOwner.ShowDateInfo)
                    //    View.NotifyEdit(IdCommunity, fileLink.ItemOwner.Id, fileLink.ItemOwner.StartDate, fileLink.ItemOwner.EndDate, fileLink.ItemOwner.IsVisible);
                    //else
                    //    View.NotifyEditNoDate(IdCommunity, fileLink.ItemOwner.Id, fileLink.ItemOwner.IsVisible);
                    View.SendActionEditFileItemVisibility(IdCommunity, ModuleID, LinkId, fileLink.isVisible);
                }
                View.ReturnToFileManagement(View.ItemCommunityId, itemID);
            }
        #endregion

    }
}
