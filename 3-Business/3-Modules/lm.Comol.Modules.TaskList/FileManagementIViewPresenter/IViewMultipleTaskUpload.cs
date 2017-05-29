using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;

namespace lm.Comol.Modules.TaskList
{

    public interface IViewMultipleTaskUpload : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long PreloadedItemID { get; set; }


        int CurrentItemCommunityID { get; set; }

          void NoPermissionToAddFiles(int IdCommunity, int ModuleID);
      
          CoreModuleRepository RepositoryPermission(int IdCommunity);
        
          bool AllowCommunityUpload { get; set; }

          void InitializeCommunityUploader(int IdCommunity, CoreModuleRepository moduleRepository);
       
          void InitializeModuleUploader(int IdCommunity);
        
          void SetUrlToFileManagement(int IdCommunity, long p);

           void SetUrlToDiary(int IdCommunity, long p);
        
          bool AllowUpload { get; set; }

          void SendInitAction(int IdCommunity, int ModuleID, long p);
       
          long CurrentItemID { get; set; }

          void ReturnToFileManagement(int IdCommunity, long preloadedItemId);
        
          void ReturnToItemsList(int p);
       
          void AddCommunityFileAction(int IdCommunity, int ModuleID);
       
          MultipleUploadResult<dtoModuleUploadedFile> GetUploadedModuleFile(Task item, int p, string p_2, int p_3, int ModuleID);

          void ReturnToFileManagementWithErrors(int IdCommunity, long IdItem, List<dtoModuleUploadedFile> list, List<dtoUploadedFile> list_2);
       
        }
}
