using MailBC.DataStore.Infrastructure.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MailBC.DataStore.Infrastructure
{
    /// <summary>
    /// The Repository Interface
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value></value>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Gets entity by key.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="key">The key that represents the identifier of the entity.</param>
        /// <returns></returns>
        TEntity GetByKey<TEntity>(object key) where TEntity : class;

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria"> </param>
        /// <returns></returns>
        IQueryable<TEntity> Query<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets entities based on the specified order by.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TOrderBy"></typeparam>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class;

        /// <summary>
        /// Gets entities based on the specified criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TOrderBy"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class;

        /// <summary>
        /// Gets entities which satifies a specification.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TOrderBy"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Get<TEntity, TOrderBy>(ISpecification<TEntity> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class;

        /// <summary>
        /// Gets single entity based on matching criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Single<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Gets single entity using specification.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;

        /// <summary>
        /// Firsts the specified predicate.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Gets first entity with specification.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TEntity First<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Add<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Attach the specified entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Attach<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Saves the specified entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Save<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Deletes one or many entities based on the specified predicate
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Deletes entities which satify specified criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        void Delete<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;

        /// <summary>
        /// Updates changes of the existing entity. 
        /// The caller must later call SaveChanges() on the repository explicitly to save the entity to database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Finds entities based on provided predicate.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Finds entities based on provided criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;

        /// <summary>
        /// Finds one entity based on provided predicate.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"> </param>
        /// <returns></returns>
        TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Finds one entity based on provided criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TEntity FindOne<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;

        /// <summary>
        /// Counts the specified entities.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        int Count<TEntity>() where TEntity : class;

        /// <summary>
        /// Counts entities with the specified predicate.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"> </param>
        /// <returns></returns>
        int Count<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Counts entities satifying provided criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        int Count<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;
    }
}