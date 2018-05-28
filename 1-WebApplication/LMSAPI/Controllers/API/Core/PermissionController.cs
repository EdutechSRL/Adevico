using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adevico.APIconnection.Core.Business;
using Adevico.Core.Core.dto;
using Adevico.WebAPI.Controllers.API;

namespace LMSAPI.Controllers.API.Core
{
    public class PermissionController : BaseApiAuthenticatedController
    {
        /// <summary>
        /// Get permission from Header data.
        /// If CommunityId == 0, it returns PORTAL permission (by Person Type).
        /// ELSE il returns COMMUNITY permission
        /// </summary>
        /// <returns></returns>
        public dtoPermissionResponse Get([FromUri]string ParServiceCode = "")
        {
            return CoreService.PermissionGet(
                (ParServiceCode != "" ? ParServiceCode : ServiceCode),
                UserContext.CurrentUserID,
                UserContext.CurrentCommunityID);
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
