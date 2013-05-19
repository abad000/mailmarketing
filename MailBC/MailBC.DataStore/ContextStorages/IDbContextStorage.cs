using System.Collections.Generic;
using System.Data.Entity;

namespace MailBC.DataStore.ContextStorages
{
    public interface IDbContextStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        DbContext GetDbContextForKey(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objectContext"></param>
        void SetDbContextForKey(string key, DbContext objectContext);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<DbContext> GetAllDbContexts();

        /// <summary>
        /// 
        /// </summary>
        void RemoveAllDbContexts();
    }
}