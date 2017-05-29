using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

namespace lm.Notification.Core.DataLayer
{
    public class BasicSessionMgr
    {
        public static ISessionFactory SessionFactory = CreateFactory();

        public static ISessionFactory CreateFactory()
        {
            try
            {

                Configuration configuration = new Configuration();
                return configuration.Configure().BuildSessionFactory();
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("CreateFactory", ex.Message);
                throw ex;
            }
        }

        public static ISessionFactory CreateFactory(String filename)
        {
            try
            {

                Configuration configuration = new Configuration();
                return configuration.Configure(filename).BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ISession GetSession()
        {
            try
            {
                return SessionFactory.OpenSession();
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("GetSession", ex.Message);
                throw ex;
            }
        }
       
    }
}
