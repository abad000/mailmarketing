using System;
using System.Data;
using System.Data.Objects;

namespace MailBC.DataStore.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsInTransaction { get; }

        /// <summary>
        /// 
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isolationLevel"></param>
        void BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// 
        /// </summary>
        void RollBackTransaction();

        /// <summary>
        /// 
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveOptions"></param>
        void SaveChanges(SaveOptions saveOptions);
    }
}
