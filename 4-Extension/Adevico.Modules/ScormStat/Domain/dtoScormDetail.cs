using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.ScormStat.Domain
{
    public class dtoScormPlayDetail
    {
        long  FileId { get; set; }
        string FileName { get; set; }
        long VersionId { get; set; }
        long VersionNumber { get; set; }
        string FilterType { get; set; }
        DateTime FilterCreated { get; set; }

        int PersonId { get; set; }
        string PersonName { get; set; }

        int ActivitiesTotal { get; set; }
        int ActivitiesDone { get; set; }

        int PlayTotal { get; set; }
        TimeSpan TimeTotal { get; set; }
        TimeSpan TimeMin { get; set; }
        bool CheckTime { get; set; }

        double ScoreTotal { get; set; }
        double ScoreMin { get; set; }
        bool CheckScore { get; set; }



    }
}
