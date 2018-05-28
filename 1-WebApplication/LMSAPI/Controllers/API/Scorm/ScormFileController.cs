using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Adevico.WebAPI.Controllers.API;
using Adevico.Modules.ScormStat;
using Adevico.Modules.ScormStat.Domain;
using Adevico.WebAPI.Helper;
using LMSAPI.Business;

namespace LMSAPI.Controllers.API
{
    /// <summary>
    /// Elenco file
    /// </summary>
    public class ScormFileController : BaseApiAuthenticatedController //ApiController // 
    {
        /// <summary>
        /// GET: recupera l'elenco dei file accessibili per l'utente corrente nella comunità corrente.
        /// </summary>
        /// <returns></returns>
        public dtoScormFileListResponse Get()
        {
            if (ContextHelper.APIContext != null && ContextHelper.APIContext.CurrentLinkId <= 0)
            {
                return Service.GetCommunityScormFiles();
            }
            else
            {
                return Service.GetModuleLinkStatistics(ContextHelper.APIContext.CurrentLinkId);
            }
            
        }

        private ScormStatService _service;

        internal ScormStatService Service
        {
            get
            {

                if (_service == null || base.UpdateService)
                {
                    _service = new ScormStatService(base.AppContext);
                    base.UpdateService = false;
                }

                if (_service == null)
                {

                }

                return _service;
            }
        }
    }
}