using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace lm.Notification.Core.DataLayer
{
    public class Repository<T> : IDisposable
    {
        private ISession session;
        private ITransaction tx;

        //public Repository()
        //{
        //    //session = SessionHelper.GetNewSession();            
        //}

        public Repository(ISession session)
        {
            this.session = session;
        }

        #region IDataContext Members

        #region Persistence functions

        /// <summary>
        /// Adds an object to the repository
        /// </summary>
        /// <param name="item">The object to add</param>
        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            session.Save(item);
            if (!this.IsInTransaction)
            {
                session.Flush();
            }
        }

        /// <summary>
        /// Deletes an object from the repository
        /// </summary>
        /// <param name="item">The object to delete</param>
        public void Delete(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            session.Delete(item);
            if (!this.IsInTransaction)
            {
                session.Flush();
            }
        }

        /// <summary>
        /// Saves an object to the repository
        /// </summary>
        /// <param name="item">The object to save</param>
        public void Save(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            session.Update(item);
            if (!this.IsInTransaction)
            {
                session.Flush();
            }
        }

        #endregion

        #region Transaction management

        /// <summary>
        /// Reports whether this <c>ObjectSpaceServices</c> contain any changes which must be synchronized with the database
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return session.IsDirty();
            }
        }

        /// <summary>
        /// Reports whether this <c>ObjectSpaceServices</c> is working transactionally
        /// </summary>
        public bool IsInTransaction
        {
            get
            {
                return session.Transaction != null && session.Transaction.IsActive;
            }
        }

        /// <summary>
        /// Begins a transaction
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if there is an already active transaction</exception>
        public void BeginTransaction()
        {
            if (this.IsInTransaction)
            {
                throw new InvalidOperationException("A transaction is already opened");
            }
            else
            {
                try
                {
                    tx = session.BeginTransaction();
                }
                catch (Exception ex)
                {
                    throw new DatalayerTransactionException(ex.Message, ex);
                }
            }

        }

        /// <summary>
        /// Commits the active transaction
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if there isn't an active transaction</exception>
        public void Commit()
        {
            if (!this.IsInTransaction)
            {
                throw new InvalidOperationException("Operation requires an active transaction");
            }
            else
            {
                try
                {
                    tx.Commit();
                    tx.Dispose();
                }
                catch (Exception ex)
                {
                    throw new DatalayerTransactionException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Rollbacks the active transaction
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if there isn't an active transaction</exception>        
        public void Rollback()
        {
            if (!this.IsInTransaction)
            {
                throw new InvalidOperationException("Operation requires an active transaction");
            }
            else
            {
                try
                {
                    tx.Rollback();
                    tx.Dispose();
                }
                catch (Exception ex)
                {
                    throw new DatalayerTransactionException(ex.Message, ex);
                }
            }
        }

        #endregion

        /// <summary>
        /// Retrieves all the persisted instances of a given type
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve</typeparam>
        /// <returns>The list of persistent objects</returns>
        public IList<T> GetAll()
        {
            return GetAll(0, 0);
        }

        /// <summary>
        /// Retrieves all the persisted instances of a given type
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve</typeparam>
        /// <returns>The list of persistent objects</returns>
        public IQueryable<T> GetAllQueryable()
        {
            return GetAll(0, 0).AsQueryable<T>();
        }

        public static IQueryable<T> GetAllQueryable(ISession currentSession)
        {
            return GetAll(currentSession, 0, 0).AsQueryable<T>();
        }

        /// <summary>
        /// Retrieves all the persisted instances of a given type
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve</typeparam>
        /// <param name="pageIndex">The index of the page to retrieve</param>
        /// <param name="pageSize">The size of the page to retrieve</param>
        /// <returns>The list of persistent objects</returns>
        public IList<T> GetAll(int pageIndex, int pageSize)
        {
            return GetAll(session, pageIndex, pageSize);
        }

        public static IList<T> GetAll(ISession currentSession, int pageIndex, int pageSize)
        {
            ICriteria criteria = currentSession.CreateCriteria(typeof(T));

            if (pageSize > 0)
            {
                criteria.SetFirstResult(pageIndex * pageSize);
                criteria.SetMaxResults(pageSize);
            }
            return criteria.List<T>();
        }

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier, or null if there is no such persistent instance.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="id">The identifier of the object</param>
        /// <returns>The persistent instance or null</returns>
        public T GetById<IdType>(IdType key)
        {
            return session.Load<T>(key);
        }

        public static T GetById<IdType>(ISession currentSession, IdType key)
        {
            return currentSession.Load<T>(key);
        }

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier, or null if there is no such persistent instance.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="id">The identifier of the object</param>
        /// <returns>The persistent instance or null</returns>
        public T GetById(Object key)
        {
            return session.Load<T>(key);
        }

        /// <summary>
        /// Returns the amount of objects of a given type
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>The amount of objects</returns>
        public int GetCount()
        {
            ICriteria criteria = session.CreateCriteria(typeof(T));
            criteria.SetProjection(Projections.RowCount());
            return (int)criteria.List()[0];
        }


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                session.Dispose();
            }
            finally
            {
            }
        }

        #endregion
    }
}