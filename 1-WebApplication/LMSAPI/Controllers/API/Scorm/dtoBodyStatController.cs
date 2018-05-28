using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adevico.Modules.ScormStat;

namespace LMSAPI.Controllers.API.Scorm
{
    /// <summary>
    /// Corpo richiesta per Play Scorm
    /// </summary>
    public class DtoBodyStatController
    {
        /// <summary>
        /// Elenco id utenti
        /// </summary>
        public List<int> PersonsId { get; set; }

        /// <summary>
        /// Elenco id file
        /// </summary>
        public List<long> FilesId { get; set; }

        private BehaviorCode _bhCode = BehaviorCode.LastPlay;

        /// <summary>
        /// Code su tipo play restituiti. Default: BehaviorCode.LastPlay
        /// </summary>
        public BehaviorCode BehaviorCode
        {
            get { return _bhCode; }
            set { _bhCode = value; }
        }
    }
}