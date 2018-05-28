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
using Adevico.Modules.Repository.Domain;
using Adevico.Modules.Repository.Service;
using Adevico.APIconnection.Core.dto;
using Adevico.APIconnection.Core.Business;
using Adevico.Core.Core.dto;
using lm.Comol.Core.FileRepository.Domain;

namespace LMSAPI.Controllers.API
{
    /// <summary>
    /// Elenco file
    /// </summary>
    public class FileStatsController : BaseApiAuthenticatedController //ApiController // 
    {
        /// <summary>
        /// GET: recupera l'elenco dei file accessibili per l'utente corrente nella comunità corrente.
        /// </summary>
        /// <returns></returns>
        public dtoFileInfoResponse Get(int CommunityId = -1)
        {
            dtoFileInfoResponse response = new dtoFileInfoResponse();
            if (CommunityId == -1 && AppContext != null && AppContext.UserContext != null && AppContext.UserContext.CurrentCommunityID >= 0)
                CommunityId = AppContext.UserContext.CurrentCommunityID;
            
            response = Service.GetCommunityFilesRepo(CommunityId);
            
            CheckResponse(response);
            return response;
        }


        [Route("api/FileStats/{Id}/Downloads")]
        [HttpGet]
        public dtoFileDownloadsResponse GetDownloads(int Id, int VersionId, int CommunityId = -1)
        {
            dtoFileDownloadsResponse response = new dtoFileDownloadsResponse(); 
            response.ErrorInfo = LastError;

            if (Id > 1 && VersionId > 1) {
                if (CommunityId == -1 && AppContext != null && AppContext.UserContext != null && AppContext.UserContext.CurrentCommunityID >= 0)
                    CommunityId = AppContext.UserContext.CurrentCommunityID;

                response = Service.GetCommunityFilesRepoDownloads(CommunityId, Id, VersionId);
            }
            else
            {
                response.ErrorInfo = Adevico.APIconnection.Core.GenericError.InvalidDataInput;
            }

            CheckResponse(response);

            return response;

        }

        /// <summary>
        /// Recupera tutti gli utenti e le relative statistiche, dato un file
        /// </summary>
        /// <param name="Id">Id Item = file</param>
        /// <param name="VersionId">Id versione</param>
        /// <param name="CommunityId">Id Comunità</param>
        /// <param name="OnlyPermitted">SE FALSE, recupera tutti gli utenti della comunità, altrimenti (default) SOLO quelli che hanno permesso di accesso dal repository.</param>
        /// <returns></returns>
        [Route("api/FileStats/{Id}/FilePersons")]
        [HttpGet]
        public dtoPersonsResponse GetPersons(int Id, int VersionId, int CommunityId = -1, bool OnlyPermitted = true)
        {

            dtoPersonsResponse response = new dtoPersonsResponse();
            response.ErrorInfo = LastError;

            int comId = (base.UserContext.CurrentCommunityID > 0)
                ? base.UserContext.CurrentCommunityID
                : base.UserContext.WorkingCommunityID;


            //response = CoreService.PersonGetList(
            //    base.ServiceCode,
            //    comId,
            //    base.UserContext.CurrentUserID,
            //    response,
            //    UserBehaviorCode.OnlyActive);

            response = Service.GetUsersFileAccess(comId, Id, VersionId, response, HasStatisticsRepositoryPermission(), !OnlyPermitted);
            
            CheckResponse(response);

            return response;
        }

        private bool HasStatisticsRepositoryPermission()
        {
            dtoPermissionResponse module = CoreService.PermissionGet(ModuleRepository.UniqueCode);

            //ToDo: verificare se sia necessario/possibile aggiungere anche altri servizi, come:
            // - Statistiche
            // - Gestore comunità

            try
            {
                dtoRepositoryPermissionResponse repomodule = (dtoRepositoryPermissionResponse)module;

                return repomodule.Admin || repomodule.ViewStatistics;
            }
            catch (Exception)
            {
            }

            return false;
        }
        

        private FileInfoService _service;

        internal FileInfoService Service
        {
            get
            {

                if (_service == null || base.UpdateService)
                {
                    _service = new FileInfoService(base.AppContext);
                    //base.UpdateService = false;
                }

                if (_service == null)
                {

                }

                return _service;
            }
        }

        private Adevico.APIconnection.Core.Business.CoreAPIService _coreService { get; set; }

        internal Adevico.APIconnection.Core.Business.CoreAPIService CoreService
        {
            get
            {
                if (_coreService == null || base.UpdateService)
                {
                    _coreService = new CoreAPIService(base.AppContext);
                }

                return _coreService;
            }
        }
    }
}