using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileTransfer.DomainModel.Configuration
{
    [Serializable]
    public class MultimediaAnalysisConfig
    {
        public MultimediaAnalysisConfig()
        {
            DocumentCandidates = new List<string>();
            DirectoryCandidates = new List<string>();
            RulePriorityFrequencyRatio = 0.55F;
        }

        public List<String> DirectoryCandidates { get; set; }
        public List<String> DocumentCandidates { get; set; }
        public Single RulePriorityFrequencyRatio { get; set; }
    }
}
