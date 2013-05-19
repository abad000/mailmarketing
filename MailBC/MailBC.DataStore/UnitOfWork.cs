using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using MailBC.DataStore.Infrastructure;

namespace MailBC.DataStore
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbTransaction _transaction;
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        #region Implementation of IDisposable

        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_disposed)  return;

            _disposed = true;
        }

        #endregion

        #region Implementation of IUnitOfWork

        /// <summary>
        /// 
        /// </summary>
        public bool IsInTransaction
        {
            get { return _transaction != null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isolationLevel"></param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_transaction != null) throw new ApplicationException(@"Cannot begin a new transaction while an existing transaction is still
                                                                       running. Please commit or rollback the existing transaction before
                                                                       starting a new one.");
            OpenConnection();
            _transaction = ((IObjectContextAdapter) _context).ObjectContext.Connection.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RollBackTransaction()
        {
            if (_transaction == null) throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            if (!IsInTransaction) return;

            _transaction.Rollback();
            ReleaseCurrentTransaction();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CommitTransaction()
        {
            if (_transaction == null) throw new ApplicationException("Cannot commit a transaction while there is no transaction running.");

            try
            {
                ((IObjectContextAdapter)_context).ObjectContext.SaveChanges();
                _transaction.Commit();
                ReleaseCurrentTransaction();
            }
            catch
            {
                RollBackTransaction();
                ReleaseCurrentTransaction();
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveChanges()
        {
            if (IsInTransaction) throw new ApplicationException("A transaction is running. Call CommitTransaction instead.");

            ((IObjectContextAdapter)_context).ObjectContext.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveOptions"></param>
        public void SaveChanges(SaveOptions saveOptions)
        {
            if (IsInTransaction) throw new ApplicationException("A transaction is running. Call CommitTransaction instead.");

            ((IObjectContextAdapter)_context).ObjectContext.SaveChanges(saveOptions);
        }

        #endregion

        private void OpenConnection()
        {
            if (((IObjectContextAdapter) _context).ObjectContext.Connection.State == ConnectionState.Open) return;
            ((IObjectContextAdapter) _context).ObjectContext.Connection.Open();
        }

        private void ReleaseCurrentTransaction()
        {
            if (_transaction == null) return;
            _transaction.Dispose();
            _transaction = null;
        }
    }
}