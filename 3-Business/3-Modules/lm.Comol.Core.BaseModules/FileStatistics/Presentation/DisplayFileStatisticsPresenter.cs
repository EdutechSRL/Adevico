using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

using lm.Comol.Core.BaseModules.Scorm.Business;
using lm.Comol.Core.Business;
namespace lm.Comol.Core.BaseModules.FileStatistics.Presentation
{
    public class DisplayFileStatisticsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private NHibernate.ISession _IcoSession;

        protected virtual IViewDisplayFileStatistics View
        {
            get { return (IViewDisplayFileStatistics)base.View; }
        }

        #region New
        public DisplayFileStatisticsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
           this.CurrentManager = new BaseModuleManager(oContext);
        }  public DisplayFileStatisticsPresenter(iApplicationContext oContext, IViewDisplayFileStatistics view)
            : base(oContext, view)
        {
            
            this.CurrentManager = new BaseModuleManager(oContext);
        }
      

        #endregion

        
              
       
    }
}
