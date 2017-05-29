using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList
{
    public interface IViewTaskMultipleUpload : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long PreloadedItemID { get; }
        int PreloadedCommunityID { get; }

        IList<ModuleCommunityPermission<ModuleTasklist>> CommunitiesPermission { get; }
        CoreModuleRepository RepositoryPermission(int communityID);
        long CurrentItemID { get; set; }
        int CurrentItemCommunityID { get; set; }
        bool AllowUpload { set; }
        bool AllowCommunityUpload { set; }
        void SetUrlToDiary(int IdCommmunity, long IdItem);
        void SetUrlToItem(int IdCommmunity, long IdItem);
        void SetUrlToFileManagement(int IdCommmunity, long IdItem);

        void NoPermissionToAddFiles(int IdCommmunity, int IdModule);
        void ReturnToItemsList(int IdCommmunity);
        void ReturnToFileManagement(int IdCommmunity, long IdItem);
        void ReturnToFileManagementWithErrors(int IdCommmunity, long IdItem, List<dtoModuleUploadedFile> NUinternalFile, List<dtoUploadedFile> NUcommunityFile);

        void InitializeCommunityUploader(int IdCommmunity, CoreModuleRepository coreModuleRepository);
        void InitializeModuleUploader(int IdCommmunity);
        MultipleUploadResult<dtoModuleUploadedFile> GetUploadedModuleFile(Task item, int itemTypeId, String moduleCode, int moduleOwnerActionID, int moduleId);

        //   MultipleUploadResult(Of dtoModuleUploadedFile)  
        //  Function AddModuleInternalFiles(ByVal FileType As FileRepositoryType, ByVal ObjectOwner As Object, ByVal ServiceOwner As String, ByVal ServiceOwnerActionID As Integer, ByVal ObjectTypeID As Integer)

        void SendInitAction(int IdCommmunity, int IdModule, long IdItem);
        void AddInternalFileAction(int IdCommmunity, int IdModule);
        void AddCommunityFileAction(int IdCommmunity, int IdModule);
        void NotifyAddFile(int IdCommmunity, long IdItem, DateTime startDate, DateTime endDate, int fileNumber, bool isvisible);
        void NotifyAddFileNoDate(int IdCommmunity, long IdItem, string Title, int FileNumber, bool isvisible);
        //Sub NotifyAddCommunityFile(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal FileID As Long, ByVal FileName As String, ByVal isScorm As Boolean, ByVal UniqueId As System.Guid, ByVal isVisible As Boolean)
        //Sub NotifyAddCommunityFile(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal ItemID As Long, ByVal Title As String, ByVal FileID As Long, ByVal FileName As String, ByVal isScorm As Boolean, ByVal UniqueId As System.Guid, ByVal isVisible As Boolean)
        //Sub NotifyAddInternalFile(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal FileID As System.Guid, ByVal FileName As String, ByVal isScorm As Boolean, ByVal isVisible As Boolean)
        //Sub NotifyAddInternalFile(ByVal CommunityID As Integer, ByVal ItemID As Long, ByVal Title As String, ByVal FileID As System.Guid, ByVal FileName As String, ByVal isScorm As Boolean, ByVal isVisible As Boolean)

    }
}
