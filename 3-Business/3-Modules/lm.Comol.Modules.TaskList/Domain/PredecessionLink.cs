using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;



namespace lm.Comol.Modules.TaskList.Domain
{
    public class PredecessionLink
    {
        public virtual int ID { get; set; }
        public virtual Task SuccessorTask { get; set; }  
        public virtual Task PredecessorTask { get; set; }  
        public virtual MetaData MetaInfo { get; set; }



        //public int DependentTaskID { get; set; }
        ///public DateTime PredecessionCreationDate { get; set; }
        //public Person PredecessorCreator { get; set; }
        // public int PredecessorCreatorID { get; set; }
        // public int PredecessorTaskID { get; set; }

        public PredecessionLink() {
        }
    }
}
