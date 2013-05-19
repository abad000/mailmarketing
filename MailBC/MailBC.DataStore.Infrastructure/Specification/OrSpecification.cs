using MailBC.DataStore.Infrastructure.Extensions;
using System.Linq;

namespace MailBC.DataStore.Infrastructure.Specification
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class OrSpecification<TEntity> : CompositeSpecification<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrSpecification{TEntity}" /> class.
        /// </summary>
        /// <param name="leftSide"></param>
        /// <param name="rightSide"></param>
        public OrSpecification(Specification<TEntity> leftSide, Specification<TEntity> rightSide)
            : base(leftSide, rightSide)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override TEntity SatisfyingEntityFrom(IQueryable<TEntity> query)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(LeftSide.Predicate.Or(RightSide.Predicate));
        }
    }
}