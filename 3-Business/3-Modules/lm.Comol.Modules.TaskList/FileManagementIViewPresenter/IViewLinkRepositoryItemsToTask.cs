using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;

namespace lm.Comol.Modules.TaskList
{

    public interface IViewLinkRepositoryItemsToTask : lm.Comol.Core.DomainModel.Common.IViewAddRepositoryItemToModuleItem<long>

    {
        void SetBackToProject(int IdCommunity, long IdItem); //SetBackToCommunityDiary
        void SetBackToTaskUrl(int IdCommunity, long IdItem); //SetBackToItemUrl
        void ReturnToTask(int IdCommunity, long IdItem);
        void ReturnToProject(int IdCommunity, long IdItem);
    }
}
