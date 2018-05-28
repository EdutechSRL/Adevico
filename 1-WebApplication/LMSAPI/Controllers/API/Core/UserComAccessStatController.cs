using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Adevico.APIconnection.Core;
using Adevico.WebAPI.Controllers.API;
using Adevico.UserStatistics.Service;
using LMSAPI.Controllers.API.dto;


namespace LMSAPI.Controllers.API
{
    public class UserComAccessStatController : BaseApiAuthenticatedController
    {
        //
        public dto.dtoUserStatResult Get(
            [FromUri] BehaviourCodeUAS behaviorCode = BehaviourCodeUAS.Last30day,
            [FromUri] DateTime? referenceDateTime = null
            //[FromUri] int refDay = 0,
            //[FromUri] int refMonth = 0,
            //[FromUri] int refYear = 0
            )
        {


            if (behaviorCode == BehaviourCodeUAS.Reference && referenceDateTime == null)
            {
                behaviorCode = BehaviourCodeUAS.Last30day;
            }

            int usrId = base.UserContext.CurrentUserID;
            int comId = base.UserContext.CurrentCommunityID;

            dtoUserStatResult response = new dtoUserStatResult();

            try
            {
                switch (behaviorCode)
                {
                    case BehaviourCodeUAS.All:
                        response.DatesList = Service.UserAccessDateGet(comId, usrId);
                        break;
                    case BehaviourCodeUAS.Reference:
                        DateTime refDate = new DateTime(referenceDateTime.Value.Year, referenceDateTime.Value.Month, referenceDateTime.Value.Day);
                        response.DatesList = Service.UserAccessDateGetPostDate(comId, usrId, refDate);
                        break;
                    case BehaviourCodeUAS.Last4Week:
                        response.DatesList = Service.UserAccessDateGetPostDateLast4Week(comId, usrId);
                        break;
                    default:
                        DateTime nowDate = DateTime.Now.AddDays(-30);
                        response.DatesList = Service.UserAccessDateGetPostDate(comId, usrId, new DateTime(nowDate.Year, nowDate.Month, nowDate.Day));
                        break;
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

        /// <summary>
        /// Limiti temporali
        /// </summary>
        public enum BehaviourCodeUAS : int
        {
            /// <summary>
            /// Tutte i dati disponibili
            /// </summary>
            All = -1,
            /// <summary>
            /// DEFAULT: ultimi 30 giorni
            /// </summary>
            Last30day = 0,
            /// <summary>
            /// Ultime 4*7 giorni: far diventare ultime 4 settimane!
            /// </summary>
            Last4Week = 1,
            /// <summary>
            /// Dalla data indicata
            /// </summary>
            Reference = 2
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
    }
}