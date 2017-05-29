using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DataLayer
{
    public static class SessionDispatcher
    {
        private const String ConnectionStringProperty = "connection.connection_string";
        static Dictionary<String, ISessionFactory> Factories = new Dictionary<string, ISessionFactory>();

        public static ISessionFactory Factory(String ConnectionString)
        {
            try
            {
                ISessionFactory factory; //new Configuration().Configure().BuildSessionFactory();
                if (Factories.ContainsKey(ConnectionString))
                {
                    factory = Factories[ConnectionString];
                    if (factory == null)//|| factory.IsClosed == true)
                    {
                        Factories.Remove(ConnectionString);
                        factory = GenerateFactory(ConnectionString);
                    }
                }
                else
                {
                    //factory = new Configuration().Configure().SetProperty(ConnectionStringProperty, ConnectionString).BuildSessionFactory();
                    //Factories.Add(ConnectionString, factory);
                    factory = GenerateFactory(ConnectionString);
                }
                return factory;
            }
            catch (Exception ex)
            {
                if (Factories.ContainsKey(ConnectionString))
                {
                    Factories.Remove(ConnectionString);
                }
                throw;
            }
        }
        /// <summary>
        /// Generate factory for specific DB
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static ISessionFactory GenerateFactory(String connectionString)
        {
            ISessionFactory factory = new Configuration().Configure().SetProperty(ConnectionStringProperty, connectionString).BuildSessionFactory();
            Factories.Add(connectionString, factory);
            return factory;
        }
        /// <summary>
        /// Create Session for specific connection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static ISession NewSession(String connectionString)
        {
            try
            {
                return Factory(connectionString).OpenSession();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
