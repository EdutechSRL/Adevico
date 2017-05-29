using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;
using lm.Comol.Modules.TaskList;
using lm.Comol.Core.BaseModules;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;


namespace lm.Comol.Modules.TaskList
{
    class MultipleTaskUploadPresenter  :  lm.Comol.Core.DomainModel.Common.DomainPresenter 
        //: lm.Comol.Core.DomainModel.Common.DomainPresenter
    {

        private int _ModuleID;
        private ServiceTaskList _Service;

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

        protected virtual IViewMultipleTaskUpload View
        {
            get { return (IViewMultipleTaskUpload)base.View; }
        }

        private ServiceTaskList Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceTaskList(AppContext);
                return _Service;
            }
        }
        public MultipleTaskUploadPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public MultipleTaskUploadPresenter(iApplicationContext oContext, IViewMultipleTaskUpload view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView()
        {
            int IdCommunity = UserContext.CurrentCommunityID;
            long preloadedItemId = View.PreloadedItemID;
            View.CurrentItemCommunityID = IdCommunity;
            if (!this.UserContext.isAnonymous)
            {
                Task item = Service.GetTask(preloadedItemId);
                if (item == null)
                    View.NoPermissionToAddFiles(IdCommunity, ModuleID);
                else
                {
                    IdCommunity = (item.Community == null ? 0 : item.Community.Id);
                    CoreModuleRepository moduleRepository = View.RepositoryPermission(IdCommunity);
                    CoreItemPermission permission = GetItemPermission(IdCommunity, item);
                    if (permission.AllowAddFiles)
                    {
                        View.AllowCommunityUpload = moduleRepository.Administration || moduleRepository.UploadFile;
                        if (moduleRepository.Administration || moduleRepository.UploadFile)
                            View.InitializeCommunityUploader(IdCommunity, moduleRepository);
                        View.InitializeModuleUploader(IdCommunity);
                        View.SetUrlToFileManagement(IdCommunity, item.ID);
                        View.SetUrlToDiary(IdCommunity, item.ID);
                        View.AllowUpload = true;
                        View.SendInitAction(IdCommunity, ModuleID, item.ID);
                        View.CurrentItemID = item.ID;
                        View.CurrentItemCommunityID = IdCommunity;
                    }
                    else
                        View.ReturnToFileManagement(IdCommunity, preloadedItemId);
                }
            }
            else
                View.NoPermissionToAddFiles(IdCommunity, ModuleID);
        }
        public void AddFiles(MultipleUploadResult<dtoUploadedFile> repositoryFiles)
        {
            long IdItem = View.CurrentItemID;
            int IdCommunity = View.CurrentItemCommunityID;
            Task item = Service.GetTask(IdItem);
            if (item == null)
                View.ReturnToItemsList(View.CurrentItemCommunityID);
            else
            {
                if (item.Community != null && IdCommunity != item.Community.Id)
                {
                    IdCommunity = item.Community.Id;
                    View.CurrentItemCommunityID = IdCommunity;
                }
                if (repositoryFiles != null && repositoryFiles.UploadedFile.Count > 0)
                {
                    IList<ModuleActionLink> repositoryList = (from uf in repositoryFiles.UploadedFile select uf.Link).ToList();
                    Service.SaveTaskListFiles(item, IdCommunity, repositoryList, ModuleID, (int)ModuleTasklist.ObjectType.TaskLinkedFile, ModuleTasklist.UniqueID, false);
                    View.AddCommunityFileAction(IdCommunity, ModuleID);
                }

                MultipleUploadResult<dtoModuleUploadedFile> moduleFiles = View.GetUploadedModuleFile(item, (int)ModuleTasklist.ObjectType.Task, ModuleTasklist.UniqueID, (int)ModuleTasklist.ActionType.DownloadTaskFile, ModuleID);
                if (moduleFiles != null && moduleFiles.UploadedFile.Count > 0)
                {
                    IList<ModuleActionLink> moduleList = (from uf in moduleFiles.UploadedFile select uf.Link).ToList();
                    Service.SaveTaskListFiles(item, IdCommunity, moduleList, ModuleID, (int)ModuleTasklist.ObjectType.TaskLinkedFile, ModuleTasklist.UniqueID, false);
                    View.AddCommunityFileAction(IdCommunity, ModuleID);
                }

                if ((repositoryFiles != null && repositoryFiles.NotuploadedFile.Count > 0) || (moduleFiles != null && moduleFiles.NotuploadedFile.Count > 0))
                {
                    View.ReturnToFileManagementWithErrors(IdCommunity, IdItem, moduleFiles.NotuploadedFile, repositoryFiles.NotuploadedFile);
                }
                else
                    View.ReturnToFileManagement(IdCommunity, IdItem);
            }
        }
        private CoreItemPermission GetItemPermission(int IdCommunity, Task item)
        {
            ModuleTasklist module = Service.ServicePermission(UserContext.CurrentUserID, IdCommunity);
            CoreModuleRepository moduleRepository = View.RepositoryPermission(IdCommunity);
            return Service.GetTaskPermission(item, module, moduleRepository);
        }
	}
    }

