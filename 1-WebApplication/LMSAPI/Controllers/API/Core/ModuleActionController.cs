using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using Adevico.APIconnection.Core;
using Adevico.APIconnection.Core.Business;
using Adevico.UserStatistics.Service;
using Adevico.WebAPI.Controllers.API;
using COL_BusinessLogic_v2.UCServices;
using lm.Comol.Core.Data;
using lm.Comol.Core.TemplateMessages.Domain;
using LMSAPI.Controllers.API.dto;

namespace LMSAPI.Controllers.API
{
    public class ModuleActionController : BaseApiAuthenticatedController
    {
        /// <summary>
        /// Get ModuleActions
        /// </summary>
        /// <param name="take">First "take" record for user/community</param>
        /// <returns></returns>
        public dtoModuleActionResult Get(
           [FromUri] int take = 0,
           [FromUri] BehaviourCode behaviorCode = BehaviourCode.Community
           )
        {

            int usrId = base.UserContext.CurrentUserID;

            int comId = 0;

            switch (behaviorCode)
            {
                case BehaviourCode.All:
                    comId = -1;
                    break;
                case BehaviourCode.Community:
                    comId = base.UserContext.CurrentCommunityID;
                    break;
                case BehaviourCode.Portal:
                    comId = 0;
                    break;
            }

            
            
            
            //ToDo: verificare che lo carichi correttamente:
            int langId = base.UserContext.CurrentUser.LanguageID;

            dtoModuleActionResult response = new dtoModuleActionResult();

            try
            {
                IDictionary<int, string> modules = CoreService.ModulesGetCommunityDict(langId, (comId <= 0)? 0 : comId);


                

                //Escludo la bacheca...
                IList<Int32> excluded = new List<int>();
                excluded.Add(CoreService.ModulesIdGetFromCode(Services_Bacheca.Codex));

                response.Actions = Service.ModuleActionGet(comId, usrId, modules, take, excluded);


                if (behaviorCode == BehaviourCode.All)
                {
                    IDictionary<int, string> communityNames = CoreService.CommunityGetNames(usrId);
                    response.Actions.ForEach(
                      a =>
                      {
                          a.ServiceName =
                              modules.ContainsKey(a.ServiceId) ? modules[a.ServiceId] : a.ServiceId.ToString();
                      
                          a.CommunityName = (a.CommunityId == 0) ? "portal" :
                              communityNames.ContainsKey(a.CommunityId)? communityNames[a.CommunityId] : "unknow";
                      }
                    );
                }
                else
                {
                    string comName = "";
                    if (behaviorCode == BehaviourCode.Portal || comId == 0)
                    {
                        comName = "Portal";
                    }
                    else
                    {
                        comName = CoreService.CommunityGetName(comId);
                    }

                    response.Actions.ForEach(
                          a =>
                          {
                              a.ServiceName =
                                  modules.ContainsKey(a.ServiceId) ? modules[a.ServiceId] : a.ServiceId.ToString();
                              a.CommunityName = comName;
                          }
                  );
                }
                    
                response.Success = true;

            }
            catch (Exception)
            {
                response.Success = false;
                response.ErrorInfo = GenericError.Internal;
                throw;
            }

            // resp.ErrorInfo = ContextHelper.UserContextLoad(actionContext);

            //resp.Success = (resp.ErrorInfo == GenericError.None);

            CheckResponse(response);

            return response;
        }

        public enum BehaviourCode : int
        {
            /// <summary>
            /// Tutte le comunità
            /// </summary>
            All = -1,
            /// <summary>
            /// Comunità corrente
            /// </summary>
            Community = 1,
            /// <summary>
            /// Solo portale
            /// </summary>
            Portal = 2
        }



        private StatisticsService _service;

        internal StatisticsService Service
        {
            get
            {

                if (_service == null || base.UpdateService)
                {
                    _service = new StatisticsService(base.StatsDataContext);
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
                    _coreService = new CoreAPIService(DataContext);
                }

                return _coreService;
            }
        }
    }
}