using System.Linq;

namespace MailBC.DataStore.Infrastructure.Specification
{
    /// <summary>
    /// In simple terms, a specification is a small piece of logic which is independent and give an answer 
    /// to the question “does this match ?”. With Specification, we isolate the logic that do the selection 
    /// into a reusable business component that can be passed around easily from the entity we are selecting.
    /// </summary>
    /// <see cref="http://en.wikipedia.org/wiki/Specification_pattern" />
    /// <typeparam name="TEntity"></typeparam>
    public interface ISpecification<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        TEntity SatisfyingEntityFrom(IQueryable<TEntity> query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query);
    }
}