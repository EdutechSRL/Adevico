using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;




namespace Adevico.Modules.Faq.Business
{
    public class FAQService : BaseCoreServices
    {
        #region initClass

        protected iApplicationContext _Context;

        public FAQService()
        {
            
        }

        public FAQService(iApplicationContext oContext)
            : base(oContext.DataContext)
        {
            _Context = oContext;
            Manager = new BaseModuleManager(oContext.DataContext);
            UC = oContext.UserContext;
        }

        public FAQService(iDataContext oDC)
            : base(oDC)
        {
            Manager = new BaseModuleManager(oDC);
            _Context = new ApplicationContext { DataContext = oDC };
        }

        #endregion
    }
}
