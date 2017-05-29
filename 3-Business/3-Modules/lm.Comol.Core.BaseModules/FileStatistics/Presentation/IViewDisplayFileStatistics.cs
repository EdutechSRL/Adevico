using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileStatistics.Presentation
{
    public interface IViewDisplayFileStatistics : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void InitializeNoPermission(int idCommunity);


        //Al presenter non glie ne frega gran che...
        //...aggiungere pero' ANCHE la session!

        ///// <summary>
        ///// Richiama il SUO presenter e gli passa DIRETTAMENTE i parametri
        ///// </summary>
        ///// <param name="FilesIds"></param>
        ///// <param name="UsersIds"></param>
        //void InitializeView(IList<Int64> FilesIds, IList<Int32> UsersIds); //-> richiama il MIO presenter e gli passa direttamente i parametri


    }
}
