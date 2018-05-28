using Adevico.WebAPI.Controllers.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adevico.APIconnection.Core;
using Adevico.APIconnection.Core.Business;
using Adevico.APIconnection.Core.dto;
using Adevico.Core.Core.dto;
using Adevico.SocialNoticeBoard.Domain.DTO;
using NHibernate.Dialect.Schema;

//using Adevico.Core.Core.dto;


namespace Adevico.WebAPI.Controllers.API
{
    public class PermissionController : BaseApiAuthenticatedController
    {
        // GET api/persone
        /// <summary>
        /// ritorna lista delle persone nella comunità.
        /// Verificare su SERVIZIO!
        /// </summary>
        /// <param name="behaviorCode">SOLO attivi o TUTTI!</param>
        /// <returns>dtoPartecipante lista delle persone nella comunità</returns>
        /// <remarks>
        /// Quali utenti restituisco?
        ///     •   SOLO gli utenti che sono iscritti alla comunità, attivi ed abilitati e non sono "Invisible" a livello di piattaforma
        ///     Secondo step:
        ///     •	SOLO gli utenti che hanno ALMENO i permessi in lettura del servizio (secondo momento)
        /// </remarks>
        public dtoPersonsResponse Get([FromUri] UserBehaviorCode behaviorCode = UserBehaviorCode.OnlyActive)
        {
            dtoPersonsResponse response = new dtoPersonsResponse();
            response.ErrorInfo = base.LastError;

            //ToDo: verificare successivamente nel caso CommuniId == 0!!!

            int comId = (base.UserContext.CurrentCommunityID > 0)
                ? base.UserContext.CurrentCommunityID
                : base.UserContext.WorkingCommunityID;

            response = CoreService.PersonGetList(
                base.ServiceCode,
                comId,
                base.UserContext.CurrentUserID,
                response,
                behaviorCode);

            //Actionfilters
            CheckResponse(response);

            return response;
        }

        private Adevico.APIconnection.Core.Business.CoreAPIService _coreService { get; set; }

        internal Adevico.APIconnection.Core.Business.CoreAPIService CoreService
        {
            get
            {
                if (_coreService == null || base.UpdateService)
                {
                    _coreService = new CoreAPIService(DataContext);
                }

                return _coreService;
            }
        }
    }
}



//public class dtoPerson
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public string Surname { get; set; }
//    public string Mail { get; set; }
//    public dtoPerson(){}
//}