using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using lm.Comol.Modules.TaskList.Domain;
using lm.Comol.Core.DomainModel;
//using lm.Module.Base.Modules;


namespace lm.Comol.Modules.TaskList.Domain
    
{
    [CLSCompliant(true)]
    
    public class Task 
        
    {
        public virtual long ID { get; set; }
        public virtual int TaskWBSindex { get; set; }
        public virtual String TaskWBSstring { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String Notes { get; set; } 
        public virtual Community Community { get; set; }
        public virtual MetaData MetaInfo { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? Deadline { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual int Level { get; set; }
        public virtual TaskPriority Priority { get; set; }
        public virtual TaskStatus Status { get; set; }
        public virtual int Completeness { get; set; }
        public virtual TaskCategory Category { get; set; }
        public virtual Task TaskParent { get; set; }
        public virtual Task Project { get; set; }
        public virtual IList<Task> TaskChildren { get; set; }
        //controlla vadano bene i TASKLISTFILE
        public virtual IList<TaskListFile> TaskFiles { get; set; }
        public virtual bool isArchived { get; set; }
        public virtual bool isPersonal { get; set; }
        public virtual bool isPortal { get; set; }
        public virtual String WBS
        { 
            get {
                return TaskWBSstring + TaskWBSindex;
            }
        }

        // public int TaskParentID { get; set; } //per ricostruire l albero senza l'intero object
        //public virtual IList<PredecessionLink> PredecessionLinks { get; set; }
        //public virtual IList<Task> SuccessorLinks { get; set; }
        //public virtual IList<Task> Predecessors { get; set; }

        public Task()
        { 
        }
       
    }
}
