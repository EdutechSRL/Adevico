using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using System.Reflection;
using lm.Comol.Modules.TaskList.Domain;

namespace lm.Comol.Modules.TaskList.DataLayer
{
    public class BasicSessionMgr
    {
        public static ISessionFactory SessionFactory=CreateFactory();
        //public static ISession Session = SessionFactory.OpenSession();  //Tia@aggiunto da es.Rob_Masch

        public static ISessionFactory CreateFactory()
        {
            try
            {

                Configuration configuration = new Configuration();
                return configuration.Configure().BuildSessionFactory();
            }
            catch (Exception ex)
            {
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
                throw ex;
            }
        }
    }
}
