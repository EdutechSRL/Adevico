using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FileTransfer.DomainModel.Configuration
{
    [Serializable]
    public class WCFTransferProtocol
    {
        public WCFTransferProtocol()
        {
            this.Name = "WCF";
            this.Active = false;
            this.PhysicalPath = "PhysicalPath";
        }

        public bool Active { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        public String PhysicalPath { get; set; }
    }
}
