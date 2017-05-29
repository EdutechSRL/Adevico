using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true)]
    public class TaskPermission
    {
      //da controllare:
      public virtual TaskPermissionEnum Permission { get; set; }
   
        public TaskPermission()
        {
            this.Permission = TaskPermissionEnum.None;
        }
        
        public TaskPermission(TaskRole role) {
            switch (role) {
                case TaskRole.ProjectOwner:
                    this.Permission = TaskPermissionEnum.AddFile | TaskPermissionEnum.ManagementUser | TaskPermissionEnum.ProjectDelete
                        | TaskPermissionEnum.TaskCreate | TaskPermissionEnum.TaskDelete | TaskPermissionEnum.TaskSetCategory 
                        | TaskPermissionEnum.TaskSetDeadline | TaskPermissionEnum.TaskSetEndDate | TaskPermissionEnum.TaskSetPriority | TaskPermissionEnum.TaskSetStartDate
                        | TaskPermissionEnum.TaskSetStatus | TaskPermissionEnum.TaskView | TaskPermissionEnum.TreeVisibility;
                //TaskPermissionEnum.TaskDelete | TaskPermissionEnum.TaskAddFile | TaskPermissionEnum.TaskSetDeadline | TaskPermissionEnum.TaskSetCategory | TaskPermissionEnum.TaskSetPriority | TaskPermissionEnum.TaskSetEndDate | TaskPermissionEnum.TaskSetStartDate | TaskPermissionEnum.TaskSetStatus | TaskPermissionEnum.TaskSetCompleteness | TaskPermissionEnum.TaskUpdate | TaskPermissionEnum.TaskDelete | TaskPermissionEnum.TaskCreate; //da finire           
                   break;
                case TaskRole.Manager:
                    this.Permission = TaskPermissionEnum.AddFile | TaskPermissionEnum.ManagementUser 
                       | TaskPermissionEnum.TaskCreate | TaskPermissionEnum.TaskDelete | TaskPermissionEnum.TaskSetCategory 
                       | TaskPermissionEnum.TaskSetDeadline | TaskPermissionEnum.TaskSetEndDate | TaskPermissionEnum.TaskSetPriority | TaskPermissionEnum.TaskSetStartDate
                       | TaskPermissionEnum.TaskSetStatus | TaskPermissionEnum.TaskView | TaskPermissionEnum.TreeVisibility;
                    break;
                case TaskRole.Resource:
                    this.Permission = TaskPermissionEnum.AddFile | TaskPermissionEnum.TaskSetCompleteness 
                      |TaskPermissionEnum.TaskView | TaskPermissionEnum.TreeVisibility;
                    break;
                case TaskRole.Visitor:
                    this.Permission = TaskPermissionEnum.TaskView;
                    break;
                default:
                    this.Permission=TaskPermissionEnum.None;
                    break;
            } 

        }
    }
}

//copia di workbookpermission.vb-cod.es. 

//#Region "Private Person"
//        Private _AddItems As Boolean
//        Private _CreateDiary As Boolean
//        Private _ReadDiary As Boolean
//        Private _DeleteDiary As Boolean
//        Private _ChangeApprovation As Boolean
//        Private _ChangeDiary As Boolean
//        Private _UndeleteDiary As Boolean
//        Private _Admin As Boolean
//#End Region
/* copia ModuleWorbook
 *      Private _CreateWorkBook As Boolean
		Private _CreateGroupWorkBook As Boolean
		Private _ManagementPermission As Boolean
		Private _Administration As Boolean
		Private _AddItemToOther As Boolean
		Private _DeleteItemsFromOther As Boolean
		Private _ChangeOtherWorkBook As Boolean
		Private _ChangeApprovationStatus As Boolean
		Private _ListOtherWorkBooks As Boolean
		Private _ReadOtherWorkBook As Boolean
		Private _DownladAllowed As Boolean
		Private _PrintOtherWorkBook As Boolean*/
//public virtual bool TreeVisibility { get; set; }

////permessi relativi al task
//public virtual bool TaskAddFile{ get; set; }
//public virtual bool TaskSetDeadline{ get; set; }
//public virtual bool TaskSetCategory{ get; set; }
//public virtual bool TaskSetPriority{ get; set; }
//public virtual bool TaskSetEndDate{ get; set; }
//public virtual bool TaskSetStartDate{ get; set; }
//public virtual bool TaskSetStatus{ get; set; }
//public virtual bool TaskSetCompleteness{ get; set; }
//public virtual bool TaskView{ get; set; }
//public virtual bool TaskUpdate{ get; set; }
//public virtual bool TaskDelete{ get; set; }
//public virtual bool TaskCreate{ get; set; }
//public virtual bool TaskAddTask { get; set; }
////permessi relativi al Progetto
//public virtual bool ProjectSetProperties{ get; set; }
//public virtual bool ProjectView{ get; set; }
//public virtual bool ProjectUpdate{ get; set; }
//public virtual bool ProjectDelete { get; set; }
//public virtual bool ProjectCreate { get; set; }
//public virtual bool ProjectAddTask { get; set; }