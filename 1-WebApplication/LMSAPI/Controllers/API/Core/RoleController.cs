using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adevico.APIconnection.Core.Business;
using Adevico.APIconnection.Core.dto;
using Adevico.Core.Core.dto;
using Adevico.WebAPI.Controllers.API;

namespace LMSAPI.Controllers.API
{
    /// <summary>
    /// Controller per il recupero dei ruoli di una comunità.
    /// ToDo: verifica su IdComunità: SOLO comunità corrente?
    /// </summary>
    /// <remarks>
    /// Eventualmente togliere quello che non serve o usare una funzione unica.
    /// ToDo: TESTARE!!!
    /// </remarks>
    public class RoleController : BaseApiAuthenticatedController
    {
        ///// <summary>
        ///// Recupera la lista di ruoli disponibili per la comunità corrente,
        ///// nella lingua corrente.
        ///// </summary>
        ///// <param name="behaviourCode">Tutti o solo disponibili</param>
        ///// <returns></returns>
        //public dtoRoleResponse Get(
        //    [FromUri]Adevico.APIconnection.Core.Business.CoreAPIService.BehaviourCode behaviourCode = CoreAPIService.BehaviourCode.Subcribed)
        //{
        //    return Get(UserContext.CurrentCommunityID, UserContext.Language.Id, behaviourCode);
        //}

        ///// <summary>
        ///// Recupera la lista di ruoli disponibili per la comunità indicata,
        ///// nella lingua corrente.
        ///// </summary>
        ///// <param name="communityId">Id comunità da cui recuperare i ruoli</param>
        ///// <param name="behaviourCode">Tutti o solo disponibili</param>
        ///// <returns></returns>
        //public dtoRoleResponse Get([FromUri]int communityId, [FromUri]Adevico.APIconnection.Core.Business.CoreAPIService.BehaviourCode behaviourCode = CoreAPIService.BehaviourCode.Subcribed)
        //{
        //    return Get(communityId, UserContext.Language.Id, behaviourCode);
        //}

        /// <summary>
        /// Recupera la lista di ruoli disponibili per la comunità indicata,
        /// nella lingua indicata.
        /// </summary>
        /// <param name="communityId">Id Comunità</param>
        /// <param name="languageId">Id Lingua (cambia con la piattaforma)</param>
        /// <param name="behaviourCode">Tutti o solo disponibili</param>
        /// <returns></returns>
        public dtoRoleResponse Get(
            [FromUri]int communityId = 0,
            [FromUri]int languageId = 0,
            [FromUri]Adevico.APIconnection.Core.Business.CoreAPIService.BehaviourCode behaviourCode = CoreAPIService.BehaviourCode.Subcribed)
        {
            if (languageId <= 0)
            {
                if (UserContext.Language == null)
                    languageId = coreApiService.LanguageGet(0).Id; //Lingua default di sistema, se l'ID non viene trovato o è <= 0.
                else
                {
                    languageId = UserContext.Language.Id;
                }
            }
                

            if (communityId <= 0)
                communityId = UserContext.CurrentCommunityID;

            dtoRoleResponse response = new dtoRoleResponse();
            response.ErrorInfo = LastError;
            
            response = coreApiService.RolesGet(behaviourCode,
                communityId, languageId,
                UserContext.CurrentUserID);

            CheckResponse(response);

            return response;
        }

       

        
    }
}
