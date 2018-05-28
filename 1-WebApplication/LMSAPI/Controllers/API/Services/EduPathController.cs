using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Adevico.APIconnection.Core;
using Adevico.APIconnection.Core.Business;
using Adevico.Core.Core.dto;
using Adevico.EdupathExtension.DTO;

namespace Adevico.WebAPI.Controllers.API
{
    public class EduPathController : BaseApiAuthenticatedController
    {

        /// <summary>
        /// Elenco dei percorsi formativi a cui l'utente è iscritto.
        /// </summary>
        /// <returns>
        /// Elenco di percorsi comprensivi di comunità, link di accesso se non è bloccato, stato di completamento dell'utente, etc...
        /// </returns>
        public dtoPathPortfolioResponse Get()
        {
            //In questo caso NON mi serve.
            //Servizio GENERICO di PORTALE per l'accesso agli Edupath,
            //che già tiene conto di iscrizioni ed accessi agli stessi.

            ////GetSelectedUserPathsCountUsr_API
            //// Blocco da rendere "generico" su TUTTE le chiamate, esclusa login!!!
            //dtoSocialDashboardPermissionResponse dpr = (dtoSocialDashboardPermissionResponse)CoreService.PermissionGet(
            //    COL_BusinessLogic_v2.UCServices.Services_SocialNoticeboard.Codex,
            //    UserContext.CurrentUserID,
            //    UserContext.CurrentCommunityID,
            //    UserContext.UserTypeID);

            dtoPathPortfolioResponse response = new dtoPathPortfolioResponse();
            response.ErrorInfo = base.LastError;

            if (LastError == GenericError.None)
            {
                response = Service.GetSelectedUserPathsCountUsr_API(response);
            }

            CheckResponse(response);

            return response;
        }

        private Adevico.EdupathExtension.BusinessLogic.ServiceStat _service;
        internal Adevico.EdupathExtension.BusinessLogic.ServiceStat Service
        {
            get
            {

                if (_service == null || base.UpdateService)
                {
                    _service = new Adevico.EdupathExtension.BusinessLogic.ServiceStat(base.AppContext);
                    base.UpdateService = false;
                }

                if (_service == null)
                {

                }

                return _service;
            }
        }


        //private Adevico.APIconnection.Core.Business.CoreAPIService _coreService { get; set; }
        //internal Adevico.APIconnection.Core.Business.CoreAPIService CoreService
        //{
        //    get
        //    {
        //        if (_coreService == null || base.UpdateService)
        //        {
        //            _coreService = new CoreAPIService(DataContext);
        //        }

        //        return _coreService;
        //    }
        //}
    }


    
}