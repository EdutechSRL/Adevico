using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Adevico.APIconnection.Core;
using Adevico.APIconnection.Core.Business;
using Adevico.APIconnection.Core.dto;
using Adevico.WebAPI.Controllers.API;
using lm.Comol.Core.Data;
using lm.Comol.Core.DomainModel;

namespace Adevico.WebAPI.Controllers.API
{
    public class CommunityController : BaseApiAuthenticatedController
    {
        // GET api/comunita
        /// <summary>
        /// Recupera l'elenco delle comunità NON ARCHIVIATE a cui l'utente è ISCRITTO.
        /// </summary>
        /// <param name="token">Token di autenticazione, ottenuto in fase di login.</param>
        /// <param name="deviceId">Id dispositivo, per permettere l'accesso da più dispositivi.</param>
        /// <returns></returns>
        /// <remarks>
        /// Comunità a cui 
        /// •	sei iscritto 
        /// •	puoi accedere
        /// •	ToDo: hanno il servizio #SRVZ attivo
        /// •	ToDo: ho permesso XY
        /// </remarks>
        public Adevico.APIconnection.Core.dto.dtoCommunityResponse Get()
        {
            dtoCommunityResponse response = new dtoCommunityResponse();
            response.ErrorInfo = LastError;

            response = coreApiService.CommunityGetList(UserContext.CurrentUserID);
            
            CheckResponse(response);

            return response;
        }

    }
}
