using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace lm.Comol.Core.DataLayer
{
    public class SessionHelper
    {
        private static Configuration cfg;
        private static ISessionFactory sessionfactory;

        //Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        static SessionHelper()
        {

            if (HttpContext.Current != null)
            {
                sessionfactory = FactoryBuilder(HttpContext.Current.Server.MapPath("~/hibernate.cfg.xml.config"));
            }
            else
            {
                sessionfactory = FactoryBuilder();

            }
        }

        public static ISessionFactory FactoryBuilder()
        {
            try
            {
                cfg = new Configuration();
                cfg.Configure();
                return cfg.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString);
                //log.Error(ex.ToString)
                return null;
            }
        }

        public static ISessionFactory FactoryBuilder(string filename)
        {
            try
            {
                cfg = new Configuration();
                cfg.Configure(filename);
                return cfg.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                //Debug.Write(ex.ToString);
                //log.Error(ex.ToString)
                return null;
            }
        }

        public static ISession GetNewSession()
        {
            try
            {
                return sessionfactory.OpenSession();
            }
            catch (Exception ex)
            {
                //Debug.Write(ex.ToString);
                //log.Error(ex.ToString)
                return null;

            }
        }

        public static ISessionFactory GetCurrentFactory()
        {
            try
            {
                return sessionfactory;
            }
            catch (Exception ex)
            {
                //Debug.Write(ex.ToString);
                //log.Error(ex.ToString)
                return null;

            }
        }

        public static ISessionFactory GetFactory()
        {
            try
            {
                return sessionfactory;
            }
            catch (Exception ex)
            {
                //Debug.Write(ex.ToString);
                //log.Error(ex.ToString)
                return null;

            }
        }


    }

}
