using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.FileRepository.Domain;

namespace Adevico.Modules.ScormStat.Domain
{
    /// <summary>
    /// Dati relativi al play di un utente.
    /// </summary>
    public class ScormStatPlay
    {
        /// <summary>
        /// Id play
        /// </summary>
        public virtual Int64 Id { get; set; }
        /// <summary>
        /// Id utente
        /// </summary>
        public virtual Int32 PersonId { get; set; }
        /// <summary>
        /// Id file
        /// </summary>
        public virtual Int64 FileId { get; set; }
        /// <summary>
        /// Id versione
        /// </summary>
        public virtual Int64 VersionId { get; set; }

        /// <summary>
        /// Se l'utente ha altri completamenti.
        /// ToDO: VERIFICARE se su VERSIONE o su FILE!!!
        /// </summary>
        public virtual bool AlreadyCompleted { get; set; }


        #region Play data
        /// <summary>
        /// SE il play corrente è stato completato
        /// </summary>
        public virtual bool IsCompleted { get; set; }
        /// <summary>
        /// Stato attuale
        /// </summary>
        public virtual lm.Comol.Core.FileRepository.Domain.PackageStatus Status { get; set; }
        /// <summary>
        /// % di completamento: 0-100.
        /// In casi particolari, ovvero può non avere valori intermedi.
        /// </summary>
        public virtual int PercCompletion { get; set; }
        /// <summary>
        /// Data di fine del play.
        /// </summary>
        public virtual DateTime? EndPlayOn { get; set; }

        #region ScormData
        /// <summary>
        /// Punteggio utente (0 se non previsto dal pacchetto)
        /// </summary>
        public virtual Double PlayScore { get; set; }
        /// <summary>
        /// Tempo TOTALE utente (per versione)
        /// </summary>
        public virtual long PlayTime { get; set; }
        /// <summary>
        /// Attività completate
        /// </summary>
        public virtual long ActivitiesDone { get; set; }
        ///// <summary>
        ///// Numero di play dell'utente.
        ///// ToDo: per VERSIONE o per FILE
        ///// </summary>
        //public virtual long PlayNumber { get; set; }
        #endregion
        #endregion

        /// <summary>
        /// Impostazioni pacchetto (e minimi x completamento)
        /// </summary>
        public virtual ScormStatFilterSettings Settings { get; set; }

        public virtual bool IsCalculated { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual lm.Comol.Core.FileRepository.Domain.ScormStatus LessonStatus { get; set; }

        

    }
}
