using MailBC.DataStore.Infrastructure.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MailBC.DataStore.Infrastructure.Specification
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Specification<TEntity> : ISpecification<TEntity>
    {
        public Expression<Func<TEntity, bool>> Predicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Specification{TEntity}" /> class.
        /// </summary>
        /// <param name="predicate"></param>
        public Specification(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public Specification<TEntity> And(Specification<TEntity> specification)
        {
            return new Specification<TEntity>(this.Predicate.And(specification.Predicate));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Specification<TEntity> And(Expression<Func<TEntity, bool>> predicate)
        {
            return new Specification<TEntity>(this.Predicate.And(predicate));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public Specification<TEntity> Or(Specification<TEntity> specification)
        {
            return new Specification<TEntity>(this.Predicate.Or(specification.Predicate));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Specification<TEntity> Or(Expression<Func<TEntity, bool>> predicate)
        {
            return new Specification<TEntity>(this.Predicate.Or(predicate));
        }

        #region Implementation of ISpecification<TEntity>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public TEntity SatisfyingEntityFrom(IQueryable<TEntity> query)
        {
            return query.Where(Predicate).SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(Predicate);
        }

        #endregion
    }
}