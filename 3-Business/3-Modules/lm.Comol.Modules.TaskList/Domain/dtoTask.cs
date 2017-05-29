using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.TaskList.Domain
{
    class dtoTask
    {
        public long Id { get; set; }
        public int CommunityId { get; set; }
        public CoreItemPermission Permission {get; set;}
        public Task oTask { get; set; }
        //public int LessonNumber {get; set;}
        public string Description { get; set; }
        public IList<iCoreItemFileLinkPermission<long>> FileLinks { get; set; }

        public dtoTask()
        {
        }
        public dtoTask(CoreItemPermission module, Task task)
        {
            Id = task.ID;
            oTask = task;
            Permission = module;
        }
        public dtoTask(int idCommunity,CoreItemPermission module, Task task,  string description)
        {
            Id = task.ID;
            CommunityId = idCommunity;
            oTask = task;
            Permission = module;
            //LessonNumber = lessonNumber;
            if (description == null)
                description = "";
            else if (string.IsNullOrEmpty(description))
                description = "";
            Description = description;
        }
    }
    }

