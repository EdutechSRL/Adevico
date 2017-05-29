using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.TaskList.Domain
{
    [Serializable(), CLSCompliant(true)]
    public class TaskListFile : DomainBaseObjectMetaInfo<long>
    {

        #region "Private"
        private Task _ProjectOwner; // Project - Event;
        private Task _TaskOwner; //Task - Item
        private Community _CommunityOwner;
        private Person _Owner;
        private BaseCommunityFile _File;
        private ModuleLink _Link;
        #endregion
        
        private bool _isVisible;

        public virtual Person Owner { get; set; }

        public virtual Task TaskOwner { get; set; }
        public virtual Community CommunityOwner { get; set; }
        public virtual Task ProjectOwner { get; set; }
        public virtual BaseCommunityFile File { get; set; }
        public virtual ModuleLink Link { get; set; }
        public virtual bool isVisible { get; set; }

        public TaskListFile()
        {
        }
    }
}

