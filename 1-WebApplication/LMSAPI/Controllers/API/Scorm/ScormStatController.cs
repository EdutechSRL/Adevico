using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Adevico.Modules.ScormStat;
using Adevico.Modules.ScormStat.Domain;
using Adevico.WebAPI.Controllers.API;
using LMSAPI.Controllers.API.Scorm;
using Newtonsoft.Json.Linq;

namespace LMSAPI.Controllers.API
{
    /// <summary>
    /// Recupero play
    /// </summary>
    public class ScormStatController : BaseApiAuthenticatedController //ApiController // 
    {
        /// <summary>
        /// POST: recupero TUTTI i dati di PLAY degli elementi indicati.
        /// </summary>
        /// <param name="body">
        /// Oggetto composto da:
        ///     - Elenco ID Person di cui voglio le statistiche
        ///     - Elenco ID File di cui voglio le statistiche
        /// </param>
        /// <returns>
        /// 
        /// Response con l'elenco dei play e relativi dati.
        /// 
        /// Enum utilizzati:
        ///	INT (da localizzare):
        ///	public enum EvaluationType
        ///    {
        ///        FromScormEvaluation = 0,
        ///        CustomForPackage = 1,
        ///        CustomForActivities = 2,
        ///    }
        ///	
        ///	INT (da localizzare):
        ///	public enum PackageStatus
        ///    {
        ///        notstarted = 0,
        ///        started = 1,
        ///        completed = 2,
        ///        passed = 4,
        ///        completedpassed = 6,
        ///        failed = 8,
        ///        completedfailed = 10,
        ///    }
        ///	
        ///	STRING (Scorm: no local):
        ///	public enum ScormStatus
        ///    {
        ///        none = 0,
        ///        failed = 1,
        ///        completed = 2,
        ///        incomplete = 3,
        ///        browsed = 4,
        ///        unknow = 5,
        ///        not_attempted = 6,
        ///        passed = 7,
        ///        notattempted = 8,
        ///        started = 9,
        ///    }
        /// </returns>
        /// <remarks>
        ///     ToDo: NON viene effettuato un "controllo" sull'accesso ai singoli dati per l'utente.
        ///     E' possibile inserire i seguenti controlli:
        ///     - accesso alle statistiche per il singolo file (Eviterei)
        ///     - ToDo: Elenco utenti: fattibile. Controllo il RUOLO e permessi dell'utente. SE non ha accesso all'elenco utenti/statistiche degli utenti, mostro SOLO per l'utente corrente.
        /// 
        /// </remarks>
        public dtoScormPlayResponse Post(
            [FromBody] DtoBodyStatController body
            )
        {
            //ToDo: check permission

            return Service.GetPlayResponse(body.PersonsId, body.FilesId, body.BehaviorCode);
        }

        private Adevico.Modules.ScormStat.ScormStatService _service;

        internal Adevico.Modules.ScormStat.ScormStatService Service
        {
            get
            {

                if (_service == null || base.UpdateService)
                {
                    _service = new Adevico.Modules.ScormStat.ScormStatService(base.AppContext);
                    base.UpdateService = false;
                }

                if (_service == null)
                {

                }

                return _service;
            }
        }
    }


}