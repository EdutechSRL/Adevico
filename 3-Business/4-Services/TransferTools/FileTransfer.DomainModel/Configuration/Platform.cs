using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace FileTransfer.DomainModel.Configuration
{
    [Serializable]
    [MessageContract()]
    public class Platform
    {
        public Platform()
        {
            WCF = new WCFTransferProtocol();
            ActiveTransferProtocol = TransferProtocolType.none;
            MultimediaAnalysis = new MultimediaAnalysisConfig();
            KeepFileStructure = false;
        }

        [XmlAttribute]
        public String Name { get; set; }
        public TransferProtocolType ActiveTransferProtocol { get; set; }
        public String ConnectionString { get; set; }

        public String ScormConnectionString { get; set; }
        
        public MultimediaAnalysisConfig MultimediaAnalysis { get; set; }
        public String MultimediaFilePath { get; set; }        
        
        public String RemoteFilePath { get; set; }
        //public String SourceFilePath { get; set; }
        public String ScormFilePath { get; set; }
        
        //public FTPTransferProtocol Ftp { get; set; }
        //public NetShareTransferProtocol NetShare { get; set; }
        public WCFTransferProtocol WCF { get; set; }

        public Boolean KeepFileStructure { get; set; }
        //public double RulePriorityFrequencyRatio { get; set; }

        //public List<String> DocumentCandidates { get; set; }
        //public List<String> DirectoryCandidates { get; set; }
        public static Platform Default()
        {
            

            Platform p = new Platform()
            {
                Name = "PlatformName",
                ConnectionString = "ConnectionString",
                RemoteFilePath = "RemoteFilePath",
                MultimediaFilePath = "MultimediaFilePath",
                ScormFilePath = "ScormFilePath",
                KeepFileStructure = false,
                //RulePriorityFrequencyRatio = 0.55,
                MultimediaAnalysis = new MultimediaAnalysisConfig()/*,      
                                                                    * 
                SourceFilePath = "SourceFilePath", 
                DestinationFilePath = "DestinationFilePath" */
            };

            return p;
        }
    }
}
