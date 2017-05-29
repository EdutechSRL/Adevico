using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewBase: lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);
    }
}
