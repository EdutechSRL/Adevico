using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace lm.Notification.Core.DataLayer
{
    public static class ISessionExtension
    {
        public static ObjectType Load<ObjectType, IdType>(this ISession s, IdType id)
        {
            return s.Load<ObjectType>(id);
        }

        public static void Save<T>(this ISession s, T item)
        {
            s.SaveOrUpdate(item);
        }

        public static void Delete<T>(this ISession s, T item)
        {
            s.Delete(item);
        }

        public static IList<T> GetAll<T>(this ISession s) where T : class
        {
            return GetAll<T>(s, 0, 0);
        }

        public static IList<T> GetAll<T>(this ISession s, int pageIndex, int pageSize) where T : class
        {
            ICriteria c = s.CreateCriteria<T>();
            if (pageSize > 0)
            {
                c.SetFirstResult(pageSize * pageIndex);
                c.SetMaxResults(pageSize);
            }
            return c.List<T>();
        }

        public static void CommitTransaction(this ISession s)
        {
            if (s.Transaction.IsActive)
            {
                s.Transaction.Commit();
            }
        }

        public static void RollbackTransaction(this ISession s)
        {
            if (s.Transaction.IsActive)
            {
                s.Transaction.Rollback();
            }
        }

        public static Boolean isInTransaction(this ISession s)
        {
            return s.Transaction.IsActive;
        }
    }
}
