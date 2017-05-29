using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileTransfer.DomainModel;
using FileTransfer.DomainModel.Configuration;
using System.Xml.Serialization;
using System.IO;

namespace FileTransfer.DomainModel.Configuration
{
    public static class ConfigurationLoader
    {
        

        public static String Filename = "Configuration.xml";
        public static String SchemaFilename = "Configuration.xsd";

        public static Config Configuration = Load();

        public static Config Load()
        {
            String path = System.AppDomain.CurrentDomain.BaseDirectory;

            String filename = Path.Combine(path, Filename);

            Config cfg = new Config();
            //SaveSchema();
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Config));
                using (StreamReader sr = new StreamReader(filename))
                {
                    cfg = (Config)xs.Deserialize(sr);
                }
                
            }
            catch (Exception ex)
            {
                cfg = Config.Default();
                
                
                Save(cfg);
            }


            return cfg;
        }

        public static void Save(Config cfg)
        {
            String path = System.AppDomain.CurrentDomain.BaseDirectory;

            String filename = Path.Combine(path, Filename);

            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Config));
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    xs.Serialize(sw, cfg);
                }
                
            }
            catch (Exception ex)
            {
                
            }
        }

        public static void SaveSchema()
        {

            String path = System.AppDomain.CurrentDomain.BaseDirectory;

            String schemaFilename = Path.Combine(path, SchemaFilename);

            try
            {
                Type type = typeof(Config);

                XmlAttributeOverrides xao = new XmlAttributeOverrides();
                AttachXmlAttributes(xao, type);

                XmlReflectionImporter importer = new XmlReflectionImporter(xao);                
                XmlSchemas schemas = new XmlSchemas();
                XmlSchemaExporter exporter = new XmlSchemaExporter(schemas);
                
                XmlTypeMapping map = importer.ImportTypeMapping(type);
                exporter.ExportTypeMapping(map);
                

                TextWriter tw = new StreamWriter(schemaFilename);
                schemas[0].Write(tw);
                tw.Close();
            }
            catch (Exception ex)
            {

                
            }
            
        }

        public static void AttachXmlAttributes(XmlAttributeOverrides xao, Type t)
        {
            List<Type> types = new List<Type>();
            AttachXmlAttributes(xao, types, t);
        }

        public static void AttachXmlAttributes(XmlAttributeOverrides xao, List<Type> all, Type t)
        {
            if (all.Contains(t))
                return;
            else
                all.Add(t);

            XmlAttributes list1 = GetAttributeList(t.GetCustomAttributes(false));
            xao.Add(t, list1);

            foreach (var prop in t.GetProperties())
            {
                XmlAttributes list2 = GetAttributeList(prop.GetCustomAttributes(false));
                xao.Add(t, prop.Name, list2);
                AttachXmlAttributes(xao, all, prop.PropertyType);
            }
            
        }

        private static XmlAttributes GetAttributeList(object[] attributes)
        {
            XmlAttributes list = new XmlAttributes();
            foreach (var attribute in attributes)
            {
                Type type = attribute.GetType();                
                if (type.Name == "XmlAttributeAttribute") list.XmlAttribute = (XmlAttributeAttribute)attribute;
                else if (type.Name == "XmlIgnoreAttribute") list.XmlIgnore = true;
                else if (type.Name == "XmlArrayAttribute") list.XmlArray = (XmlArrayAttribute)attribute;
                else if (type.Name == "XmlArrayItemAttribute") list.XmlArrayItems.Add((XmlArrayItemAttribute)attribute);
                

            }
            return list;
        }

        public static void Save()
        {
            Save(Configuration);
        }
    }
}
