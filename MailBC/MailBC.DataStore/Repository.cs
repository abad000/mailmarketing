using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using MailBC.DataStore.Infrastructure;
using MailBC.DataStore.Infrastructure.Specification;

namespace MailBC.DataStore
{
    /// <summary>
    /// 
    /// </summary>
    public class Repository : IRepository
    {
        private DbContext _context;
        private IUnitOfWork _unitOfWork;
        private readonly string _connectionStringName;
        private readonly PluralizationService _pluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));

        /// <summary>
        /// Initializes a new instance of the <see>
        ///                                     <cref>Repository&amp;lt;TEntity&amp;gt;</cref>
        ///                                   </see> class.
        /// </summary>
        public Repository() : this(string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see>
        ///                                     <cref>Repository&amp;lt;TEntity&amp;gt;</cref>
        ///                                   </see> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public Repository(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        /// <summary>
        /// Initializes a new instance of the <see>
        ///                                     <cref>Repository&amp;lt;TEntity&amp;gt;</cref>
        ///                                   </see> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(DbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see>
        ///                                     <cref>Repository&amp;lt;TEntity&amp;gt;</cref>
        ///                                   </see> class.
        /// </summary>
        /// <param name="context"></param>
        public Repository(ObjectContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = new DbContext(context, true);
        }

        /// <summary>
        /// 
        /// </summary>
        private DbContext DbContext
        {
            get
            {
                return _context ?? (_context = string.IsNullOrEmpty(_connectionStringName)
                                        ? DbContextManager.Current
                                        : DbContextManager.CurrentFor(_connectionStringName));
            }
        }  

        #region Implementation of IRepository

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value></value>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork(DbContext)); }
        }

        /// <summary>
        /// Gets entity by key.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="key">The key that represents the identifier of the entity.</param>
        /// <returns></returns>
        public TEntity GetByKey<TEntity>(object key) where TEntity : class
        {
            EntityKey entityKey = GetEntityKey<TEntity>(key);

            object originalItem;
            if (((IObjectContextAdapter) _context).ObjectContext.TryGetObjectByKey(entityKey, out originalItem))
            {
                return (TEntity) originalItem;
            }

            return default(TEntity);
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            /* 
             * From CTP4, I could always safely call this to return an IQueryable on DbContext 
             * then performed any with it without any problem:
             */
            // return DbContext.Set<TEntity>();

            /*
             * But with 4.1 release, when I call GetQuery<TEntity>().AsEnumerable(), there is an exception:
             * ... System.ObjectDisposedException : The ObjectContext instance has been disposed and can no longer be used for operations that require a connection.
             */

            /* Here is a work around: 
             * - cast DbContext to IObjectContextAdapter then get ObjectContext from it
             * - call CreateQuery<TEntity>(entityName) method on the ObjectContext
             * - perform querying on the returning IQueryable, and it works!
             */

            string entityName = GetEntityName<TEntity>();
            return ((IObjectContextAdapter)DbContext).ObjectContext.CreateQuery<TEntity>(entityName);
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Query<TEntity>().Where(predicate);
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria"> </param>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(Query<TEntity>());
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return Query<TEntity>().AsEnumerable();
        }

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
        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            return sortOrder == SortOrder.Ascending
                ? Query<TEntity>()
                        .OrderBy(orderBy)
                        .Skip((pageIndex - 1)*pageSize)
                        .Take(pageSize)
                        .AsEnumerable()
                : Query<TEntity>()
                        .OrderByDescending(orderBy)
                        .Skip((pageIndex - 1)*pageSize)
                        .Take(pageSize)
                        .AsEnumerable();
        }

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
        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            return sortOrder == SortOrder.Ascending
                ? Query(predicate)
                        .OrderBy(orderBy)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .AsEnumerable()
                : Query(predicate)
                        .OrderByDescending(orderBy)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .AsEnumerable();
        }

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
        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(ISpecification<TEntity> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            return sortOrder == SortOrder.Ascending
               ? criteria.SatisfyingEntitiesFrom(Query<TEntity>())
                       .OrderBy(orderBy)
                       .Skip((pageIndex - 1) * pageSize)
                       .Take(pageSize)
                       .AsEnumerable()
               : criteria.SatisfyingEntitiesFrom(Query<TEntity>())
                       .OrderByDescending(orderBy)
                       .Skip((pageIndex - 1) * pageSize)
                       .Take(pageSize)
                       .AsEnumerable();
        }

        /// <summary>
        /// Gets single entity based on matching criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Query<TEntity>().Single<TEntity>(predicate);
        }

        /// <summary>
        /// Gets single entity using specification.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntityFrom(Query<TEntity>());
        }

        /// <summary>
        /// Firsts the specified predicate.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Query<TEntity>().First<TEntity>(predicate);
        }

        /// <summary>
        /// Gets first entity with specification.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TEntity First<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(Query<TEntity>()).First();
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null) throw new ArgumentNullException("entity");

            DbContext.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Attach the specified entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null) throw new ArgumentNullException("entity");

            DbContext.Set<TEntity>().Attach(entity);
        }

        /// <summary>
        /// Saves the specified entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Save<TEntity>(TEntity entity) where TEntity : class
        {
            Add(entity);
            DbContext.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null) throw new ArgumentNullException("entity");

            DbContext.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Deletes one or many entities based on the specified predicate
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        public void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            IEnumerable<TEntity> entities = Find(predicate);

            foreach (TEntity entity in entities)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// Deletes entities which satify specified criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        public void Delete<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            IEnumerable<TEntity> entities = Find(criteria);

            foreach (TEntity entity in entities)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// Updates changes of the existing entity. 
        /// The caller must later call SaveChanges() on the repository explicitly to save the entity to database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            string entityName = GetEntityName<TEntity>();

            object originalItem;
            EntityKey entityKey = ((IObjectContextAdapter)DbContext).ObjectContext.CreateEntityKey(entityName, entity);

            if (((IObjectContextAdapter)DbContext).ObjectContext.TryGetObjectByKey(entityKey, out originalItem))
            {
                ((IObjectContextAdapter)DbContext).ObjectContext.ApplyCurrentValues(entityKey.EntitySetName, entity);
            }
        }

        /// <summary>
        /// Finds entities based on provided predicate.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Query<TEntity>().Where(predicate).AsEnumerable();
        }

        /// <summary>
        /// Finds entities based on provided criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(Query<TEntity>()).AsEnumerable();
        }

        /// <summary>
        /// Finds one entity based on provided predicate.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"> </param>
        /// <returns></returns>
        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Query<TEntity>().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Finds one entity based on provided criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TEntity FindOne<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntityFrom(Query<TEntity>());
        }

        /// <summary>
        /// Counts the specified entities.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public int Count<TEntity>() where TEntity : class
        {
            return Query<TEntity>().Count();
        }

        /// <summary>
        /// Counts entities with the specified predicate.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"> </param>
        /// <returns></returns>
        public int Count<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Query<TEntity>().Count(predicate);
        }

        /// <summary>
        /// Counts entities satifying provided criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public int Count<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(Query<TEntity>()).Count();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        private EntityKey GetEntityKey<TEntity>(object key) where TEntity : class
        {
            string entitySetName = GetEntityName<TEntity>();
            ObjectSet<TEntity> objectSet = ((IObjectContextAdapter)DbContext).ObjectContext.CreateObjectSet<TEntity>();
            string keyPropertyName = objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
            EntityKey entityKey = new EntityKey(entitySetName, new[] { new EntityKeyMember(keyPropertyName, key) });
            return entityKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private string GetEntityName<TEntity>() where TEntity : class
        {
            return string.Format("{0}.{1}",
                ((IObjectContextAdapter)DbContext).ObjectContext.DefaultContainerName,
                _pluralizer.Pluralize(typeof(TEntity).Name));
        }
    }
}