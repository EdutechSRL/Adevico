using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FileTransfer.DomainModel.Configuration
{
    [Serializable]
    public class Config
    {
        public Config()
        {
            Platforms = new List<Platform>();
        }

        public String Extension { get; set; }
        public ImpersonateConfig Impersonate { get; set; }
        public List<Platform> Platforms { get; set; }
        [XmlIgnore()]
        public Dictionary<String, Platform> PlatformsDict
        {
            get
            {
                return Platforms.ToDictionary(x => x.Name);
            }
        }

        //public String SourceDirectory { get; set; }
        public static Config Default()
        {
            Config c = new Config();

            //c.SourceDirectory = "Source Directory";

            c.Extension = ".stored";

            c.Platforms.Add(Platform.Default());

            c.Impersonate = new ImpersonateConfig();

            return c;
        }
    }
}
