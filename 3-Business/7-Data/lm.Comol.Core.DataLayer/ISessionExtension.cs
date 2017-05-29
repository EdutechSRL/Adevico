using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace lm.Comol.Core.DataLayer
{
    public class OrderByClause<T>
    {
        public Expression<Func<T, object>> orderBy { get; set; }
        public Boolean Ascending { get; set; }

        public OrderByClause(Expression<Func<T, object>> orderby, Boolean asc)
        {
            this.Ascending = asc;
            this.orderBy = orderby;
        }

        public OrderByClause(Expression<Func<T, object>> orderby)
        {
            this.Ascending = true;
            this.orderBy = orderby;
        }

        public OrderByClause()
        {
        }
    }

    public static class ISessionExtension
    {

        #region Basic CRUD Extension

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

        #endregion

        #region Transaction Extensions
        /// <summary>
        /// OBSOLETE, avoid use it
        /// </summary>
        /// <param name="s"></param>
        public static void CommitTransaction(this ISession s)
        {
            if (s.Transaction.IsActive)
            {
                s.Transaction.Commit();
            }
        }

        /// <summary>
        /// OBSOLETE, avoid use it
        /// </summary>
        /// <param name="s"></param>
        public static void RollbackTransaction(this ISession s)
        {
            if (s.Transaction.IsActive)
            {
                s.Transaction.Rollback();
            }
        }

        /// <summary>
        /// OBSOLETE, avoid use it
        /// </summary>
        /// <param name="s"></param>
        public static Boolean isInTransaction(this ISession s)
        {
            return s.Transaction.IsActive;
        }
        #endregion

        #region Predicate

        /// <summary>
        /// Get Total Count Elements by Predicate
        /// </summary>
        /// <typeparam name="T">Entity's Type</typeparam>
        /// <param name="session">This Clause</param>
        /// <param name="predicate">Where Predicate</param>
        /// <returns></returns>
        public static long GetTotalElements<T>(this ISession session, Expression<Func<T, bool>> predicate)
        {
            return (from T item in session.Linq<T>() select item).Where(predicate).LongCount();
        }

        /// <summary>
        /// Get By Predicate
        /// </summary>
        /// <typeparam name="T">Entity's Type</typeparam>
        /// <param name="s">This Clause</param>
        /// <param name="predicate">Where Predicate</param>
        /// <returns></returns>
        public static IList<T> GetByPredicate<T>(this ISession s, Expression<Func<T, bool>> predicate)
        {
            IList<T> list;

            list = (from T item in s.Linq<T>() select item).Where(predicate).ToList<T>();

            return list;
        }

        /// <summary>
        /// Get By Predicate Paged
        /// </summary>
        /// <typeparam name="T">Entity's Type</typeparam>
        /// <param name="s">This Clause</param>
        /// <param name="predicate">Where Predicate</param>
        /// <param name="PageIndex">Current Page (1...n)</param>
        /// <param name="PageSize">Page size</param>
        /// <returns></returns>
        public static IList<T> GetByPredicate<T>(this ISession s, Expression<Func<T, bool>> predicate, int PageIndex, int PageSize)
        {
            IList<T> list;

            list = (from T item in s.Linq<T>() select item).Where(predicate).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<T>();

            return list;
        }

        /// <summary>
        /// Get By Predicate Paged with Total Elements
        /// </summary>
        /// <typeparam name="T">Entity's Type</typeparam>
        /// <param name="s">This Clause</param>
        /// <param name="predicate">Where Predicate</param>
        /// <param name="PageIndex">Current Page (1...n)</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="TotalElements">out parameter Total Elements Count</param>
        /// <returns></returns>
        public static IList<T> GetByPredicate<T>(this ISession s, Expression<Func<T, bool>> predicate, int PageIndex, int PageSize, out long TotalElements)
        {

            TotalElements = s.GetTotalElements<T>(predicate);

            return GetByPredicate(s, predicate, PageIndex, PageSize);
        }

        /// <summary>
        /// Get By Predicate Ordered and Paged 
        /// </summary>
        /// <typeparam name="T">Entity's Type</typeparam>
        /// <param name="s">This Clause</param>
        /// <param name="predicate">Where Predicate</param>
        /// <param name="PageIndex">Current Page (1...n)</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="Orderby">Params Array OrderByClause</param>
        /// <returns></returns>
        public static IList<T> GetByPredicate<T>(this ISession s, Expression<Func<T, bool>> predicate, int PageIndex, int PageSize, params OrderByClause<T>[] Orderby)
        {

            IList<T> list;

            var query = (from T item in s.Linq<T>() select item).Where(predicate);

            foreach (OrderByClause<T> item in Orderby)
            {
                if (item.Ascending)
                {
                    query = query.OrderBy(item.orderBy);
                }
                else
                {
                    query = query.OrderByDescending(item.orderBy);
                }
            }

            list = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<T>();

            return list;
        }


        public static IList<T> GetByPredicateMetaInfo<T>(this ISession s, Expression<Func<T, bool>> predicate, int PageIndex, int PageSize, params OrderByClause<T>[] Orderby)
        {

            IList<T> list;

            var query = (from T item in s.Linq<T>().Expand("MetaInfo") select item).Where(predicate);

            foreach (OrderByClause<T> item in Orderby)
            {
                if (item.Ascending)
                {
                    query = query.OrderBy(item.orderBy);
                }
                else
                {
                    query = query.OrderByDescending(item.orderBy);
                }
            }

            list = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<T>();

            return list;
        }

        /// <summary>
        /// Get By Predicate Ordered and Paged with Total Elements
        /// </summary>
        /// <typeparam name="T">Entity's Type</typeparam>
        /// <param name="s">This Clause</param>
        /// <param name="predicate">Where Predicate</param>
        /// <param name="PageIndex">Current Page (1...n)</param>
        /// <param name="PageSize">Page Size</param>
        /// <param name="TotalElements">out parameter Total Elements Count</param>
        /// <param name="Orderby">Params Array OrderByClause</param>
        /// <returns></returns>
        public static IList<T> GetByPredicate<T>(this ISession s, Expression<Func<T, bool>> predicate, int PageIndex, int PageSize, out long TotalElements, params OrderByClause<T>[] Orderby)
        {

            TotalElements = s.GetTotalElements<T>(predicate);

            return GetByPredicate(s, predicate, PageIndex, PageSize, Orderby);
        }

        #endregion

        #region Predicate Concatenation
        /// <summary>
        /// Where Predicate Concatenate (And)
        /// </summary>
        /// <typeparam name="T">Entity's Type</typeparam>
        /// <param name="predicate1">Left Side Predicate (This Clause)</param>
        /// <param name="predicate2">Right Side Predicate</param>
        /// <returns></returns>
        public static Func<T, bool> AndAlso<T>(this Func<T, bool> predicate1, Func<T, bool> predicate2)
        {
            return arg => predicate1(arg) && predicate2(arg);
        }

        /// <summary>
        /// Where Predicate Concatenate (Or)
        /// </summary>
        /// <typeparam name="T">Entity's Type</typeparam>
        /// <param name="predicate1">Left Side Predicate (This Clause)</param>
        /// <param name="predicate2">Right Side Predicate</param>
        /// <returns></returns>
        public static Func<T, bool> OrElse<T>(this Func<T, bool> predicate1, Func<T, bool> predicate2)
        {
            return arg => predicate1(arg) || predicate2(arg);
        }

        #endregion

        #region Expands String GetByPredicate (Obsolote)

        //public static long GetTotalElement<T>(this ISession session, Expression<Func<T, bool>> predicate, String Expands)
        //{
        //    return (from T item in session.Linq<T>().Expand(Expands) select item).Where(predicate).LongCount();
        //}

        //public static IList<T> GetByPredicate<T>(this ISession s, Expression<Func<T, bool>> predicate, String Expands)
        //{
        //    IList<T> list;

        //    list = (from T item in s.Linq<T>().Expand(Expands) select item).Where(predicate).ToList<T>();

        //    return list;
        //} 

        //public static IList<T> GetByPredicate<T>(this ISession s, Expression<Func<T, bool>> predicate, int PageIndex, int PageSize, String Expands)
        //{
        //    IList<T> list;

        //    list = (from T item in s.Linq<T>().Expand(Expands) select item).Where(predicate).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<T>();

        //    return list;
        //}

        //public static IList<T> GetByPredicate<T>(this ISession s, Expression<Func<T, bool>> predicate, int PageIndex, int PageSize, out long TotalElements, String Expands)
        //{

        //    TotalElements = s.GetTotalElement<T>(predicate);

        //    return GetByPredicate(s, predicate, PageIndex, PageSize,Expands);
        //}

        //public static IList<T> GetByPredicate<T>(this ISession s, Expression<Func<T, bool>> predicate, int PageIndex, int PageSize, String Expands, params OrderByClause<T>[] Orderby)
        //{

        //    IList<T> list;

        //    var query = (from T item in s.Linq<T>().Expand(Expands) select item).Where(predicate);

        //    foreach (OrderByClause<T> item in Orderby)
        //    {
        //        if (item.Ascending)
        //        {
        //            query = query.OrderBy(item.orderBy);
        //        }
        //        else
        //        {
        //            query = query.OrderByDescending(item.orderBy);
        //        }
        //    }

        //    list = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<T>();

        //    return list;
        //}

        //public static IList<T> GetByPredicate<T>(this ISession s, Expression<Func<T, bool>> predicate, int PageIndex, int PageSize, out long TotalElements, String Expands, params OrderByClause<T>[] Orderby)
        //{

        //    TotalElements = s.GetTotalElement<T>(predicate);

        //    return GetByPredicate(s, predicate, PageIndex, PageSize, Expands, Orderby);
        //}
        #endregion
    }
}
