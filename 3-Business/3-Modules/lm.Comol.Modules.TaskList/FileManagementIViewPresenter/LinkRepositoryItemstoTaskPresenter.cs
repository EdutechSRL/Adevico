using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.TaskList
{
    public class LinkRepositoryItemstoTaskPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter

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

        protected virtual IViewLinkRepositoryItemsToTask View
        {
            get { return (IViewLinkRepositoryItemsToTask)base.View; }
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

     public LinkRepositoryItemstoTaskPresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }

     public LinkRepositoryItemstoTaskPresenter(iApplicationContext oContext, IViewLinkRepositoryItemsToTask view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView()
		{
			int IdCommunity = UserContext.CurrentCommunityID;
            View.CurrentItemCommunityID=IdCommunity;
            View.CurrentItemID=0;
            View.AllowCommunityLink=false;
            if (UserContext.isAnonymous)
                View.NoPermissionToManagementFiles(IdCommunity,ModuleID);
            else{
                Task item = Service.GetTask(View.PreloadedItemID);
                if (item==null){
                    View.SetBackToProject(IdCommunity,0);
                    View.NoPermissionToManagementFiles(IdCommunity,ModuleID);
                }
                else{
                    IdCommunity = (item.Community == null ? 0 : item.Community.Id);
                    View.CurrentItemCommunityID=IdCommunity;
                    View.CurrentItemID=item.ID;
                    CoreModuleRepository moduleRepository = Service.GetCoreModuleRepository(UserContext.CurrentUserID,IdCommunity);
                    CoreItemPermission permission = GetTaskPermission(IdCommunity,item,moduleRepository);
                    if (permission.AllowAddFiles){
                        View.AllowCommunityLink = (moduleRepository.Administration || moduleRepository.UploadFile || moduleRepository.ListFiles || moduleRepository.DownLoad);
                        //List<long> IdFiles = Service.GetItemRepositoryFilesId(item,false);
                        View.InitializeFileSelector(IdCommunity, Service.GetTaskRepositoryFiles(item, permission.AllowEdit && permission.AllowAddFiles), (moduleRepository.Administration), (moduleRepository.Administration));
                        View.SetBackToManagementUrl(IdCommunity,item.ID);
                    }
                    else if (permission.AllowEdit)
                    {
                        View.ReturnToTask(IdCommunity, item.ID);
                    }
                    else
                    {
                        View.SetBackToProject(IdCommunity, 0);
                        View.NoPermissionToManagementFiles(IdCommunity, ModuleID);
                    }
                }
            }
        }
        private CoreItemPermission GetTaskPermission(int IdCommunity, Task item,CoreModuleRepository moduleRepository )
        {
            ModuleTasklist module = Service.ServicePermission(UserContext.CurrentUserID, IdCommunity);
            return Service.GetTaskPermission(item, module, moduleRepository);
        }
        public void UpdateFileLink(List<ModuleActionLink> selectedFilesID, List<long> linksId){
            Task item = Service.GetTask(View.CurrentItemID);
            if (item != null)
            {
                int IdCommunity = (item.Community  == null ? 0 : item.Community.Id);
                CoreModuleRepository moduleRepository = Service.GetCoreModuleRepository(UserContext.CurrentUserID,IdCommunity);
                CoreItemPermission permission = GetTaskPermission(IdCommunity,item,moduleRepository);
                //CHIEDERE NICOLA
                List<long> assignedItemFileLinksId = Service.GetCoreTaskFileLinksId(item, permission.AllowEdit && permission.AllowAddFiles);
                List<long> IdToRemove = (from ai in assignedItemFileLinksId where !linksId.Contains(ai) select ai).ToList();
                
                if (IdToRemove!= null && IdToRemove.Count>0)
                    Service.UnLinkToCommunityFileFromTask(IdToRemove);
                if (selectedFilesID != null && selectedFilesID.Count > 0) {
                    Service.SaveTaskListFiles(item, IdCommunity, selectedFilesID, ModuleID, (int)ModuleTasklist.ObjectType.TaskLinkedFile, ModuleTasklist.UniqueID, false);
                }

                View.ReturnToFileManagement(IdCommunity, item.ID);
            }
            else
                View.ReturnToProject(View.CurrentItemCommunityID, 0);
        }


    }
}
