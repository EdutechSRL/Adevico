using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;

namespace lm.Comol.Modules.TaskList 
{
    public interface IViewManagementTaskFile : lm.Comol.Core.DomainModel.Common.IManagementFileView<long> 
    {
        void SetBackToItemsUrl(int idCommunity, long itemId);
        IList<ModuleCommunityPermission<ModuleTasklist>> CommunitiesPermission { get; }
        void SendActionEditFileItemVisibility(int IdCommunity, int IdModule, long IdTaskFileLink, bool isVisible);
    }
}
