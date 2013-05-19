using System.Data.Entity;

namespace MailBC.DataStore
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDbContextBuilder<T> where T : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T BuildDbContext();
    }
}