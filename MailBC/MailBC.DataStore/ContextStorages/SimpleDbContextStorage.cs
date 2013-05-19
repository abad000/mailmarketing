using System.Collections.Generic;
using System.Data.Entity;

namespace MailBC.DataStore.ContextStorages
{
    public class SimpleDbContextStorage : IDbContextStorage
    {
        private readonly Dictionary<string, DbContext> _storage = new Dictionary<string, DbContext>();

        #region Implementation of IDbContextStorage

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DbContext GetDbContextForKey(string key)
        {
            DbContext context;
            return !_storage.TryGetValue(key, out context)
                ? null
                : context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="context"> </param>
        public void SetDbContextForKey(string key, DbContext context)
        {
            _storage.Add(key, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DbContext> GetAllDbContexts()
        {
            return _storage.Values;
        }

        public void RemoveAllDbContexts()
        {
            _storage.Clear();
        }

        #endregion
    }
}