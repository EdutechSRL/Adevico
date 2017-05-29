using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.Domain
{
    [CLSCompliant(true),Serializable]
   public class dtoTaskDetailWithPermission
    {
        public dtoTaskDetail dtoTaskDetail { get; set; }
        public TaskPermissionEnum Permission { get; set; }

        public dtoTaskDetailWithPermission(dtoTaskDetail TaskDetail, TaskPermissionEnum Permission)
        {
            this.dtoTaskDetail = TaskDetail;
            this.Permission = Permission;
        }

        public dtoTaskDetailWithPermission()
        {
            this.Permission = TaskPermissionEnum.None;
            this.dtoTaskDetail = new dtoTaskDetail();
        }
    }
}
