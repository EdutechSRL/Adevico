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
using Adevico.APIconnection.Core;

namespace LMSAPI.Controllers.API
{
    /// <summary>
    /// Controller per la gestione delle iscrizioni e delle scadenze
    /// </summary>
    /// <remarks>
    /// Verificare SE nei response basta l' "ID" response (come intero)
    /// o se viene meglio utilizzare l'enum come parametro aggiuntivo.
    /// </remarks>
    public class SubscriptionController : BaseApiAuthenticatedController
    {

        /// <summary>
        /// Imposta i ruoli di una lista di utenti in una comunità
        /// </summary>
        /// <param name="value">ruolo, comunità ed utenti</param>
        /// <returns>
        /// Response
        /// </returns>
        [Route("api/SetRoles")]
        [HttpPost]
        public dtoBaseResponse SetRoles([FromBody]dtoRoleUpdate value)
        {
            dtoRoleUpdateResponse response = new dtoRoleUpdateResponse();
            response.ErrorInfo = LastError;

            if (LastError != Adevico.APIconnection.Core.GenericError.None)
            {
                return response;
            }

            Adevico.APIconnection.Core.RoleUpdateResponse result = coreApiService.RolesSet(value.CommunityId, value.UsersId, value.NewRoleId);

            if (result == RoleUpdateResponse.NoPermission)
            {
                response.Success = false;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.NoServicePermission;
                response.ServiceErrorCode = (int)RoleUpdateResponse.NoPermission;
                return response;
            }

            response.ServiceErrorCode = (int)result;

            if (result == RoleUpdateResponse.AllUpdated || result == RoleUpdateResponse.SomeUserUpdated)
            {
                response.Success = true;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.None;
                
            }
            else
            {
                response.Success = false;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.InvalidDataInput;
            }

            return response;
            //throw new HttpResponseException(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Imposta i ruoli di una lista di utenti in una comunità
        /// </summary>
        /// <param name="value">ruolo, comunità ed utenti</param>
        /// <returns>
        /// Response
        /// </returns>
        [Route("api/SetExpiryStart")]
        [HttpPost]
        public dtoBaseResponse SetExpiryStart([FromBody]dto.dtoExpiryUpdate value)
        {
            dto.dtoExpiryResult response = new dto.dtoExpiryResult();

            response.ErrorInfo = LastError;

            if (LastError != Adevico.APIconnection.Core.GenericError.None)
            {
                return response;
            }

            Adevico.APIconnection.Core.ExpirationResponse result = coreApiService.SetExpirationStart(value.CommunityId, value.UsersId, value.SetVoid, value.StartDateTime);

            if (result == ExpirationResponse.NoPermission)
            {
                response.Success = false;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.NoServicePermission;
                response.ServiceErrorCode = (int)RoleUpdateResponse.NoPermission;
                return response;
            }

            response.ServiceErrorCode = (int)result;

            if (result == ExpirationResponse.AllUpdated || result == ExpirationResponse.SomeUserUpdated)
            {
                response.Success = true;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.None;

            }
            else
            {
                response.Success = false;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.InvalidDataInput;
            }

            return response;
        }

        /// <summary>
        /// Imposta il responsabile della comunità
        /// </summary>
        /// <param name="CommunityId">Id Comunità</param>
        /// <param name="UserId">Id Iscritto</param>
        /// <returns>Vedi ResponsabileUpdateResponse</returns>
        [Route("api/SetResponsible")]
        [HttpPost]
        public dtoBaseResponse SetResponsible([FromBody]dto.dtoSetResponsibleRequest value)
        {
            dtoBaseResponse response = new dtoBaseResponse();

            response.ErrorInfo = LastError;

            if (LastError != Adevico.APIconnection.Core.GenericError.None)
            {
                return response;
            }

            ResponsabileUpdateResponse result = coreApiService.ResponsabileSet(value.CommunityId, value.UserId);

            if (result == ResponsabileUpdateResponse.NoPermission)
            {
                response.Success = false;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.NoServicePermission;
                response.ServiceErrorCode = (int)RoleUpdateResponse.NoPermission;
                return response;
            }

            response.ServiceErrorCode = (int)result;

            if (result == ResponsabileUpdateResponse.Updated || result == ResponsabileUpdateResponse.isCurrent)
            {
                response.Success = true;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.None;

            }
            else
            {
                response.Success = false;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.InvalidDataInput;
            }

            return response;
        }

        /// <summary>
        /// Abilita, disabilita o cancella un'iscrizione: WIP, al momento non fa nulla
        /// </summary>
        /// <param name="CommunityId">Id comunità</param>
        /// <param name="UsersId">Elenco Id utenti da modificare</param>
        /// <param name="Action">Azione: 
        /// 0 = none (Nessuna azione)
        /// 1 = Enable (Abilita utenti)
        /// 2 = Disable (Disabilita utenti)
        /// 3 = Delete (Cancella iscrizione)
        /// <returns></returns>
        [Route("api/ModifySubscription")]
        [HttpPost]
        public dtoBaseResponse ModifySubscription([FromBody] dto.dtoModifySubscriptionRequest value)
        {
            dtoBaseResponse response = new dtoBaseResponse();
            

            

            response.ErrorInfo = LastError;

            if (LastError != Adevico.APIconnection.Core.GenericError.None)
            {
                return response;
            }

            RoleUpdateResponse result = coreApiService.SubscriptionModify(value.CommunityId, value.UsersId, value.Action);

            if (result == RoleUpdateResponse.NoPermission)
            {
                response.Success = false;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.NoServicePermission;
                response.ServiceErrorCode = (int)RoleUpdateResponse.NoPermission;
                return response;
            }

            response.ServiceErrorCode = (int)result;

            if (result == RoleUpdateResponse.AllUpdated || result == RoleUpdateResponse.SomeUserUpdated)
            {
                response.Success = true;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.None;

            }
            else
            {
                response.Success = false;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.InvalidDataInput;
            }

            return response;
        }


        /// <summary>
        /// Modifica scadenza iscrizione
        /// </summary>
        /// <param name="values">dtoModifyExpiration: </param>
        /// <returns></returns>
        [Route("api/ModifyExpiration")]
        [HttpPost]
        public dtoBaseResponse ModifyExpiration([FromBody] dto.dtoModifyExpiration value)
        {
            dtoBaseResponse response = new dtoBaseResponse();

            response.ErrorInfo = LastError;

            if (LastError != Adevico.APIconnection.Core.GenericError.None)
            {
                return response;
            }

            ExpirationResponse result = coreApiService.SetExpiration(
                value.communityId, 
                value.usersId,
                value.validity,
                value.extendValidity,
                value.startBehaviour);



            if (result == ExpirationResponse.NoPermission)
            {
                response.Success = false;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.NoServicePermission;
                response.ServiceErrorCode = (int)RoleUpdateResponse.NoPermission;
                return response;
            }

            response.ServiceErrorCode = (int)result;

            if (result == ExpirationResponse.AllUpdated || result == ExpirationResponse.SomeUserUpdated)
            {
                response.Success = true;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.None;

            }
            else
            {
                response.Success = false;
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.InvalidDataInput;
            }

            return response;
        }

    }
}
