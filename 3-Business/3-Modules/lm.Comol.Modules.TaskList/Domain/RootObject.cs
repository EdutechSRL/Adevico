using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.TaskList.Domain
{
    public class RootObject
    {
        public static string TaskDetailRead(long TaskId) //schermata principale con elenco diari e relativa anteprima files
        {
            return "TaskList/TaskDetail.aspx?CurrentTaskID=" + TaskId.ToString() + "&CurrentViewType=Read&ViewToLoad=TodayTasks&OrderBy=Community&Filter=AllCommunities&PageSize=50&PageIndex=0"; //"Modules/CommunityDiary/CommunityDiary.aspx?CommunityId=" + communityId.ToString();
        }
        public static string TaskDetailEditable( long TaskId) //all interno della schermata succitata, porta la scrollbar all'altezze del diario giusto
        {
            return "TaskList/TaskDetail.aspx?CurrentTaskID=" + TaskId.ToString() + "&CurrentViewType=Editable&ViewToLoad=TodayTasks&OrderBy=Community&Filter=AllCommunities&PageSize=50&PageIndex=0"; //  "Modules/CommunityDiary/CommunityDiary.aspx?CommunityId=" + communityId.ToString() + "#" + itemId.ToString();
        }

    //    public static string EditDiaryItem(int communityId, long itemId) //riporta il tutto nella schermata di modifica del diario in esame 
    //    {
    //        return "Modules/CommunityDiary/DiaryItem.aspx?ItemId=" + itemId.ToString() + "&CommunityId=" + communityId.ToString();
    //    }
        public static string ItemError()
        {
            return "Modules/Common/UploadFileErrors.aspx?ServiceCode=" + ModuleTasklist.UniqueID;
        }
        public static string ItemManagementFiles(long itemId) //
        {
            return "TaskList/ManagementTaskFile.aspx?TaskID=" + itemId.ToString();
        }
        public static string TodayTask()
        {
            return "TaskList/AssignedTasks.aspx?View=TodayTasks&CommunityFilter=AllCommunities&Sorting=None&PageSize=50&Page=0&OrderBy=Community"; 
            //Modules/CommunityDiary/ManagementFile.aspx?ItemId=" + itemId.ToString() + "&CommunityId=" + communityId.ToString();
        }

        public static string LinkFileToItem(long itemId) // pagina di caricamento dal Repository alla voce di diario
        {
            return "TaskList/AddRepositoryFileToTask.aspx?TaskID=" + itemId.ToString();
        }
        public static string MultipleUploadToItem(long itemId)
        {
            return "TaskList/TaskMultipleUpload.aspx?TaskID=" + itemId.ToString();
        }
        public static string PublishUrl()
        {
            return "ServiceCode=" + ModuleTasklist.UniqueID + "&ItemID={0}&LinkID={1}&BackUrl={2}";
        }
      }
}
