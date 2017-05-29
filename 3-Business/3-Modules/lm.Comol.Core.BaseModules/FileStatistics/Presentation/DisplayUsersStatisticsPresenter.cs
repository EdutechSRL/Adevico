using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

using lm.Comol.Core.BaseModules.Scorm.Business;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.BaseModules.FileStatistics.Presentation
{
    public class DisplayUsersStatisticsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        protected virtual IViewDisplayUsersStatistics View
        {
            get { return (IViewDisplayUsersStatistics)base.View; }
        }

        public DisplayUsersStatisticsPresenter(iApplicationContext oContext):base(oContext){
             throw new ArgumentNullException("IcoSession", "Icodeon Session must be setting.");
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public DisplayUsersStatisticsPresenter(iApplicationContext oContext, IViewDisplayUsersStatistics view)
            : base(oContext, view)
        {
            throw new ArgumentNullException("IcoSession", "Icodeon Session must be setting.");
            this.CurrentManager = new BaseModuleManager(oContext);
        }
       
        /// <summary>
        /// Richiamato dalla View gli vengono passati TUTTI i parametri necessari...
        /// </summary>
        public void BindView(IList<StatFileTreeLeaf> Files, IList<Int32> UsersIds, Int32 CurrentUserId)
        {
          
        }


        private ServiceFileStatistics _Service;

        private ServiceFileStatistics Service
        {
                get
                {
                if (_Service == null)
                {
                    NHibernate.ISession ComolSession = base.DataContext.GetCurrentSession();
                    
                    _Service = new ServiceFileStatistics(AppContext, ComolSession);
                }
                return _Service;
            }
        }

       
    }
}
