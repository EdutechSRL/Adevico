using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Adevico.APIconnection.Core;
using Adevico.SocialNoticeBoard;

using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using Adevico.APIconnection.Core.Business;
using Adevico.Core.Core.dto;
using Adevico.Modules.Mooc.Domain;
using COL_BusinessLogic_v2.CL_persona;
using lm.Comol.Modules.EduPath.BusinessLogic;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Modules.EduPath.Domain.DTO;


namespace Adevico.WebAPI.Controllers.API
{
    public class MoocStatController : BaseApiAuthenticatedController
    {
        /// <summary>
        /// Recupera le statistiche dell'utente CORRENTE per i moocs della comunità CORRENTE.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// ToDo: usare oggetto di dominio in OUTPUT
        /// ToDo: Ottimizzare la 50.000 chiamate a roba vecchia...
        /// </remarks>
        public dtoMoocInfoResponse Get()
        {
            //dtoNoticeBoardPlainListResponse response = new dtoNoticeBoardPlainListResponse();
            //response.ErrorInfo = base.LastError;

            //ContextHelper.APIContext.
            IList<dtoEPitemList> paths = new List<dtoEPitemList>();
            dtoMoocInfoResponse response = new dtoMoocInfoResponse();
            response.Moocs = new List<dtoCokadeInfo>();

            COL_Persona UtenteCorrente = new COL_Persona(base.AppContext.UserContext.CurrentUserID);


            int usrId = base.AppContext.UserContext.CurrentUserID;
            int comId = base.AppContext.UserContext.CurrentCommunityID;

            try
            {
                paths = ServiceEP.GetMyEduPaths(usrId,
                comId,
                UtenteCorrente.GetIDRuoloForComunita(comId),
                EpViewModeType.View, true);
            }
            catch (Exception ex)
            {
                string exception = ex.ToString();

            }
            

            //Me.CurrentContext.UserContext.CurrentUserID, CurrentCommunityID, CurrentCommRoleID, Me.ViewModeType, PreloadIsMooc)


            foreach (dtoEPitemList path in paths)
            {
                dtoStatWithWeight statForBar = ServiceEP.ServiceStat.GetPassedCompletedWeight_byActivity(path.Id, usrId,
                    DateTime.Now);


                lm.Comol.Modules.EduPath.Domain.DTO.dtoCokadeInfo mooc = new dtoCokadeInfo();
                mooc.CommunityId = comId;
                mooc.PathId = path.Id;
                mooc.PathName = path.Name;
                mooc.Info = new dtoCokadeMoocInfo();
                mooc.Info.Completion = statForBar.Completion;
                mooc.Info.MinCompletion = statForBar.MinCompletion;
                mooc.Info.mType = path.moocType;
                StatusStatistic moocStatus = ServiceStat.GetEpUserStatus(path.Id, usrId);

                mooc.Info.mookCompleted = CheckStatusStatistic(moocStatus, StatusStatistic.Completed) 
                        || CheckStatusStatistic(moocStatus, StatusStatistic.CompletedPassed)
                        || CheckStatusStatistic(moocStatus, StatusStatistic.Passed);
                
                response.Moocs.Add(mooc);
            }

            response.IsCommunityCokade = ServiceEP.CokadeEnabled(comId);

            if(response.IsCommunityCokade)
            {
                IList<dtoCokadeInfo> MoocsType2 = response.Moocs.Where(c => c.Info.mType == MoocType.Cockade).ToList();
                response.CokadeCompleted = MoocsType2.All(m => m.Info.mookCompleted);
                response.CokadeCompletedGold = MoocsType2.All(m => m.Info.mookCompleted && m.Info.Completion >= 100);
            }

            return response;

        }

        #region "Internal Services"


        private lm.Comol.Modules.EduPath.BusinessLogic.Service _serviceEP;

        internal lm.Comol.Modules.EduPath.BusinessLogic.Service ServiceEP
        {
            get
            {

                if (_serviceEP == null || base.UpdateService)
                {
                    _serviceEP = new lm.Comol.Modules.EduPath.BusinessLogic.Service(base.AppContext);
                    base.UpdateService = false;
                }

                if (_serviceEP == null)
                {

                }

                return _serviceEP;
            }
        }

        private lm.Comol.Modules.EduPath.BusinessLogic.ServiceStat _serviceStat;

        internal lm.Comol.Modules.EduPath.BusinessLogic.ServiceStat ServiceStat
        {
            get
            {

                if (_serviceStat == null || base.UpdateService)
                {
                    _serviceStat = new lm.Comol.Modules.EduPath.BusinessLogic.ServiceStat(new ManagerEP(base.AppContext), ServiceEP);
                    base.UpdateService = false;
                }

                if (_serviceStat == null)
                {

                }

                return _serviceStat;
            }
        }


        private bool CheckStatusStatistic(StatusStatistic actual, StatusStatistic expected)
        {
            return (actual & expected) == expected;
        }
        
        #endregion

    }
}