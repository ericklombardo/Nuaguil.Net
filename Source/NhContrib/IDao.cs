using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using Nuaguil.Utils.Model.Dto;
using Order = Nuaguil.Utils.Model.Dto.Order;

namespace Nuaguil.NhContrib
{
    public interface IDao<T, TId> 
    {
        void Delete(T entity);
        void DeleteById(TId id);
        /// <summary>
        /// Delete an entity by id
        /// </summary>
        /// <param name="id">Value of the Id to delete</param>
        /// <param name="expression">Expression for get the Id property name</param>
        void DeleteById(TId id, Expression<Func<T, TId>> expression);
        void Evict(T entity);
        IList<T> GetAll(params Order[] order);
        IList<T> GetByCriteria(int start, int max, params ICriterion[] criterion);
        IList<T> GetByCriteria(params ICriterion[] criterion);
        IList<T> GetByExample(T exampleInstance, bool excludeZeroes, params string[] propertiesToExclude);
        T GetById(TId id);
        T GetById(TId id, bool shouldLock);
        IList<TT> GetNonManagedEntityByNamedQuery<TT>(string namedQuery, Dictionary<string, object> filter);
        IList<TT> GetNonManagedEntityByNamedQuery<TT>(string namedQuery);
        IList<T> GetPagedResult(int start, int max, params Order[] order);
        PagedResultDto<T> GetPagedResultDto(int start, int max, params Order[] order);
        PagedResultDto<T> GetPagedResultDtoByCriteria(int start, int max, params ICriterion[] criterion);
        long GetRowCount();
        T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude);
        T LoadById(TId id);
        T LoadById(TId id, bool shouldLock);
        TId Save(T entity);
        void SaveOrUpdate(T entity);
        void Update(T entity);
        void Flush();
        T SaveOrUpdateCopy(T entity);
        T Merge(T entity);
       PagedResultDto<TResult> GetPagedResult<TEntity, TResult>(IQueryOver<TEntity, TEntity> query, int start, int max);
    }
}
