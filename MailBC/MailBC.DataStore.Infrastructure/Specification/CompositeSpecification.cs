using System.Linq;

namespace MailBC.DataStore.Infrastructure.Specification
{
    /// <summary>
    /// http://devlicio.us/blogs/jeff_perrin/archive/2006/12/13/the-specification-pattern.aspx
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class CompositeSpecification<TEntity> : ISpecification<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly Specification<TEntity> LeftSide;

        /// <summary>
        /// 
        /// </summary>
        protected readonly Specification<TEntity> RightSide;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSpecification{TEntity}" /> class.
        /// </summary>
        /// <param name="leftSide"></param>
        /// <param name="rightSide"></param>
        protected CompositeSpecification(Specification<TEntity> leftSide, Specification<TEntity> rightSide)
        {
            LeftSide = leftSide;
            RightSide = rightSide;
        }

        #region Implementation of ISpecification<TEntity>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public abstract TEntity SatisfyingEntityFrom(IQueryable<TEntity> query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public abstract IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query);

        #endregion
    }
}