using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.FileRepository.Domain.ScormSettings;

namespace Adevico.Modules.ScormStat.Domain
{
    public class dtoScormStatPlay
    {
        public dtoScormStatPlay() { }

        public dtoScormStatPlay(ScormStatPlay play)
        {
            Id = play.Id;
            PersonId = play.PersonId;
            FileId = play.FileId;
            VersionId = play.VersionId;
            AlreadyCompleted = play.AlreadyCompleted;
            Status = play.Status;
            PercCompletion = play.PercCompletion;
            EndPlayOn = (play.EndPlayOn != null) ? (DateTime) play.EndPlayOn : play.CreatedOn;

            CheckScore = (play.Settings != null) && play.Settings.CheckScore;
            PlayScore = (play.Settings != null) ? play.PlayScore : 0;
            MinScore = (play.Settings != null && play.Settings.CheckScore) ? (double)play.Settings.MinScore : 0;

            CheckTime = (play.Settings != null) && play.Settings.CheckTime;
            PlayTime = System.TimeSpan.FromSeconds(play.PlayTime); //Conversione
            MinTime = System.TimeSpan.FromSeconds((play.Settings != null && play.Settings.CheckTime) ? play.Settings.MinTime : 0);
            

            ActivitiesDone = play.ActivitiesDone;
            ActivitiesTotal = (play.Settings != null) ? play.Settings.ActivityCount : 1;

            ScormCompletion = play.LessonStatus.ToString();
            
            SettingType = (play.Settings != null) ? play.Settings.FilterType : EvaluationType.FromScormEvaluation;

            //PlayNumber = play.PlayNumber;

            //ToDo: verificare!
            VersionNumber = 0;
            
        }
        /// <summary>
        /// Id play
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Id utente
        /// </summary>
        public Int32 PersonId { get; set; }
        /// <summary>
        /// Id file
        /// </summary>
        public Int64 FileId { get; set; }
        /// <summary>
        /// Id versione
        /// </summary>
        public Int64 VersionId { get; set; }

        /// <summary>
        /// Se l'utente ha altri completamenti.
        /// ToDO: VERIFICARE se su VERSIONE o su FILE!!!
        /// </summary>
        public bool AlreadyCompleted { get; set; }


        #region Play data

        /// <summary>
        /// Stato attuale: valido per il nostro sistema
        /// </summary>
        public lm.Comol.Core.FileRepository.Domain.PackageStatus Status { get; set; }
        
        /// <summary>
        /// % di completamento: 0-100.
        /// In casi particolari, ovvero può non avere valori intermedi.
        /// </summary>
        public int PercCompletion { get; set; }
        
        /// <summary>
        /// Data di fine del play.
        /// </summary>
        public DateTime EndPlayOn { get; set; }

        #region ScormData
        /// <summary>
        /// Punteggio utente
        /// </summary>
        public Double PlayScore { get; set; }
        /// <summary>
        /// Punteggio minimo: 0 SE CheckScore = false
        /// </summary>
        public double MinScore { get; set; }
        /// <summary>
        /// SE lo Score viene valutato
        /// </summary>
        public bool CheckScore { get; set; }


        /// <summary>
        /// Tempo TOTALE utente
        /// </summary>
        public TimeSpan PlayTime { get; set; }
        /// <summary>
        /// Tempo minimo previsto (0 se Checktime = false)
        /// </summary>
        public TimeSpan MinTime { get; set; }
        /// <summary>
        /// Indica se il tempo ha influenza sul risultato
        /// </summary>
        public bool CheckTime { get; set; }

        /// <summary>
        /// Attività completate
        /// </summary>
        public long ActivitiesDone { get; set; }
        /// <summary>
        /// Attività totali versione
        /// </summary>
        public long ActivitiesTotal { get; set; }
        
        ///// <summary>
        ///// Numero di play dell'utente.
        ///// </summary>
        //public long PlayNumber { get; set; }

        /// <summary>
        /// Completamento SCORM: valore restituito dal PACCHETTO
        /// </summary>
        public string ScormCompletion { get; set; }

        /// <summary>
        /// Versione del file su cui è stato effettuato il play
        /// </summary>
        public long VersionNumber { get; set; }
        /// <summary>
        /// Tipo impostazione. ToDo: valutare se INT o STRING
        /// </summary>
        public lm.Comol.Core.FileRepository.Domain.ScormSettings.EvaluationType SettingType { get; set; }
        #endregion
        #endregion
    }

    //public enum SettingType
    //{
    //    Default = 0,
    //    Package = 1,
    //    Activity = 2
    //}
}
