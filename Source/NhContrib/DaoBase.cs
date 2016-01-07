using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nuaguil.NhContrib.Extensions;
using Nuaguil.Utils.Model.Dto;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Metadata;
using Order = Nuaguil.Utils.Model.Dto.Order;

namespace Nuaguil.NhContrib
{
   public abstract class DaoBase<T, TId> : AbstractNhBase, IDao<T, TId> where T : class
   {

        private IClassMetadata _classMetadata;
        protected Type PersitentType = typeof(T);
        protected Type PersitentTypeId = typeof(TId);

       protected DaoBase(ISessionFactory sessionFactory) : base(sessionFactory)
       {
       }

       /// <summary>
        /// Método para obtener la instancia del tipo T mediante su identificador.
        /// Dispara una excepción si la fila no se encuentra en la base de datos
        /// </summary>
        public T LoadById(TId id, bool shouldLock)
        {
            T entity;

            try
            {
                if (shouldLock)
                {
                    entity = (T)NHibernateSession.Load(PersitentType, id, LockMode.Upgrade);
                }
                else
                {
                    entity = (T)NHibernateSession.Load(PersitentType, id);
                }
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener la entidad", exc);
            }
            
            return entity;
        }

        /// <summary>
        /// Método para obtener la instancia del tipo T mediante su identificador.
        /// Dispara una excepción si la fila no se encuentra en la base de datos
        /// <param name="id">Identificador de la entidad</param>
        /// <returns>Entidad de tipo  T</returns>
        /// </summary>
        public T LoadById(TId id)
        {
            return LoadById(id, false);
        }

        /// <summary>
        /// Método para obtener la instancia del tipo T mediante su identificador.
        /// Retorna null si no se encuentra la fila en la base de datos
        /// </summary>
        public T GetById(TId id, bool shouldLock)
        {
            T entity;

            try
            {
                if (shouldLock)
                {
                    entity = (T)NHibernateSession.Get(PersitentType, id, LockMode.Upgrade);
                }
                else
                {
                    entity = (T)NHibernateSession.Get(PersitentType, id);
                }
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener la entidad", exc);
            }

            return entity;
        }

        /// <summary>
        /// Método para obtener la instancia del tipo T mediante su identificador.
        /// Retorna null si no se encuentra la fila en la base de datos
        /// </summary>
        public T GetById(TId id)
        {
            return GetById(id, false);
        }

        /// <summary>
        /// Remover la instancia de tipo T del cache de la sesión
        /// </summary>
        /// <param name="entity">Entidad a remover del cache</param>
        public void Evict(T entity)
        {
            NHibernateSession.Evict(entity);
        }

        public void Flush()
        {
            NHibernateSession.Flush();
        }

        /// <summary>
        /// Método para obtener todas las instancias de la entidad
        /// </summary>
        public IList<T> GetAll(params Order[] order)
        {
            try
            {
                ICriteria criteria = NHibernateSession.CreateCriteria(PersitentType);

                foreach (Order o in order)
                {
                    criteria.AddOrder(o.GetNhOrder());
                }
                return criteria.List<T>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener las instancias de la entidad", exc);
            }

        }


        public IList<T> GetPagedResult(int start, int max, params Order[] order)
        {
            try
            {
                ICriteria criteria = NHibernateSession.CreateCriteria(PersitentType);

                foreach (Order  o in order)
                {
                    criteria.AddOrder(o.GetNhOrder());
                }
                criteria.SetFirstResult(start);
                criteria.SetMaxResults(max);
                return criteria.List<T>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener las instancias de la entidad según los criterios especificados", exc);
            }
        }

        public PagedResultDto<T> GetPagedResultDto(int start, int max, params Order[] order)
        {
            var result = new PagedResultDto<T>
            {
                Rows = GetPagedResult(start, max, order) as List<T>,
                Total = GetRowCount()
            };

            return result;
        }

        /// <summary>
        /// Método para cargar cada instancia según sea el criterio <see cref="ICriterion" /> espeficado.
        /// Si el criterio <see cref="ICriterion" /> no es suministrado, se comporta como <see cref="GetAll" />.
        /// </summary>
        public IList<T> GetByCriteria(params ICriterion[] criterion)
        {
            try
            {
                ICriteria criteria = NHibernateSession.CreateCriteria(PersitentType);

                foreach (ICriterion criterium in criterion)
                {
                    criteria.Add(criterium);
                }
                return criteria.List<T>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener las instancias de la entidad según los criterios especificados", exc);
            }
        }

        public IList<T> GetByCriteria(int start,int max,params ICriterion[] criterion)
        {
            try
            {
                ICriteria criteria = NHibernateSession.CreateCriteria(PersitentType);

                foreach (ICriterion criterium in criterion)
                {
                    criteria.Add(criterium);
                }
                criteria.SetFirstResult(start);
                criteria.SetMaxResults(max);
                return criteria.List<T>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener las instancias de la entidad según los criterios especificados", exc);
            }
        }

        public PagedResultDto<T> GetPagedResultDtoByCriteria(int start, int max, params ICriterion[] criterion)
        {
            try
            {
                var result = new PagedResultDto<T>();
                ICriteria criteria = NHibernateSession.CreateCriteria(PersitentType);

                foreach (ICriterion criterium in criterion)
                {
                    criteria.Add(criterium);
                }
                criteria.SetFirstResult(start);
                criteria.SetMaxResults(max);
                
                result.Rows = criteria.List<T>() as List<T>;

                criteria.SetProjection(Projections.RowCountInt64());

                result.Total = criteria.UniqueResult<Int64>();

                return result;

            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener las instancias de la entidad según los criterios especificados", exc);
            }
        }

        public IList<T> GetByExample(T exampleInstance,bool excludeZeroes,params string[] propertiesToExclude)
        {
            try
            {
                ICriteria criteria = NHibernateSession.CreateCriteria(PersitentType);
                Example example = Example.Create(exampleInstance);

                if (excludeZeroes)
                    example.ExcludeZeroes();

                foreach (string propertyToExclude in propertiesToExclude)
                {
                    example.ExcludeProperty(propertyToExclude);
                }

                criteria.Add(example);

                return criteria.List<T>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener las instancias de la entidad según el ejemplo", exc);
            }
        }

        /// <summary>
        /// Looks for a single instance using the example provided.
        /// </summary>
        /// <exception cref="NonUniqueResultException" />
        public T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            try
            {
                IList<T> foundList = GetByExample(exampleInstance, false,propertiesToExclude);

                if (foundList.Count > 1)
                {
                    throw new NonUniqueResultException(foundList.Count);
                }

                if (foundList.Count > 0)
                {
                    return foundList[0];
                }
                return default(T);
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener la instancia de la entidad según el ejemplo especificado", exc);
            }
        }

        public Int64 GetRowCount()
        {
            try
            {
                return NHibernateSession.CreateQuery("select count(*) from " + PersitentType.Name).UniqueResult<Int64>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener el total de filas de la entidad", exc);
            }
        }

        /// <summary>
        /// Obtener una coleccion de tipo TT de acuerdo al named query especificado
        /// para una entidad no administrada(no persistente)
        /// </summary>
        /// <typeparam name="TT">Tipo de dato</typeparam>
        /// <param name="namedQuery">Nombre de la consulta</param>
        /// <returns></returns>
        public IList<TT> GetNonManagedEntityByNamedQuery<TT>(string namedQuery)
        {
            try
            {
                IQuery query = NHibernateSession.GetNamedQuery(namedQuery);
                return GetNonManagedEntity<TT>(query);
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener las instancias de la entidad según el named query especificado", exc);
            }
        }

        public IList<TT> GetNonManagedEntityByNamedQuery<TT>(string namedQuery,Dictionary<String,Object> filter)
        {
            try
            {
                IQuery query = NHibernateSession.GetNamedQuery(namedQuery);
                foreach (string key in filter.Keys)
                {
                    query.SetParameter(key, filter[key]);
                }

                return GetNonManagedEntity<TT>(query);
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener las instancias de la entidad según el named query especificado", exc);
            }
        }

        /// <summary>
        /// Método para salvar una entidad
        /// </summary>
        /// <param name="entity">Entidad a guardar</param>
        /// <returns></returns>
        public TId Save(T entity)
        {
            return (TId) NHibernateSession.Save(entity);
        }

        public T Merge(T entity)
        {
           return NHibernateSession.Merge(entity);
        }

        /// <summary>
        /// Método para agregar o actualizar una entidad
        /// </summary>
        /// <param name="entity">Entidad a agregar/actualizar</param>
        /// <returns></returns>
        public void SaveOrUpdate(T entity)
        {
            NHibernateSession.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Método para agregar o actualizar una entidad
        /// </summary>
        /// <param name="entity">Entidad a agregar/actualizar</param>
        /// <returns></returns>
        public T SaveOrUpdateCopy(T entity)
        {
            return NHibernateSession.Merge(entity);
        }

        /// <summary>
        /// Método para actualizar una instancia de la entidad T
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Update(T entity)
        {
            NHibernateSession.Update(entity);
        }

        /// <summary>
        /// Método para eliminar una instancia de la entidad T
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            try
            {
                NHibernateSession.Delete(entity);
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al eliminar la instancia", exc);
            }
        }

        public void DeleteById(TId id)
        {
            try
            {
                NHibernateSession.CreateQuery(string.Format("delete from {0} where Id = :id ",PersitentType.Name))
                    .SetParameter("id",id)
                    .ExecuteUpdate();
            }
            catch (HibernateException exception)
            {
                throw new PersistenceException("Error al eliminar la instancia", exception);
            }
        }

        public void DeleteById(TId id,Expression<Func<T,TId>> expression )
        {
            try
            {
                var memberExpression = expression.Body as MemberExpression;
                if(memberExpression == null)
                    throw new PersistenceException("Error al obtener el nombre de la propiedad Id");

                NHibernateSession.CreateQuery(string.Format("delete from {0} where {1} = :id ", PersitentType.Name,memberExpression.Member.Name))
                    .SetParameter("id", id)
                    .ExecuteUpdate();
            }
            catch (HibernateException exception)
            {
                throw new PersistenceException("Error al eliminar la instancia", exception);
            }
        }

      public PagedResultDto<TResult> GetPagedResult<TEntity, TResult>(IQueryOver<TEntity, TEntity> query, int start, int max)
      {
          return query.GetPagedResult<TEntity, TResult>(start, max);
      }

        protected IList<TT> GetNonManagedEntity<TT>(IQuery query)
        {
            try
            {
                return query.GetNonManagedEntity<TT>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener una entidad no administrada", exc);
            }
        }

        protected TT GetUniqueNonManagedEntity<TT>(IQuery query)
        {
            try
            {
                return query.GetUniqueNonManagedEntity<TT>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener una entidad no administrada", exc);
            }
        }

        protected IList<TResult> GetNonManagedEntity<TEntity, TResult>(IQueryOver<TEntity, TEntity> query)
        {
            return query.GetNonManagedEntity<TEntity, TResult>();
        }
       
       protected IList<TT> GetNonManagedEntity<TT>(ICriteria criteria)
        {
            try
            {
                return criteria.SetResultTransformer(Transformers.AliasToBean(typeof(TT)))
                            .List<TT>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener una entidad no administrada", exc);
            }
        }

        protected IClassMetadata ClassMetadata
        {
            get { return _classMetadata ?? (_classMetadata = SessionFactory.GetClassMetadata(PersitentType)); }
        }

    }
}