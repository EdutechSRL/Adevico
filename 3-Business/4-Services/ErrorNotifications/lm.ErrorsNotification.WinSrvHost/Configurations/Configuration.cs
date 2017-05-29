using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace WinService.Configurations
{
    public class Configuration
    {

        public const String FileName="app.xml";
        public static Config Cfg = Initialize();

        public static Config Initialize()
        {
            return Load();
        }

        public static Config Load()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Config));

                StreamReader sr = new StreamReader(FileName);

                Config cfg = (Config)xs.Deserialize(sr);

                sr.Close();

                return cfg;
            }
            catch
            {
                Config cfg = new Config();

                Save(cfg);

                return cfg;
            }
        }

        public static void Save(Config cfg)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Config));

                StreamWriter sw = new StreamWriter(FileName);

                xs.Serialize(sw, cfg);

                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public static void Save()
        {
            Save(Cfg);
        }
    }
}
