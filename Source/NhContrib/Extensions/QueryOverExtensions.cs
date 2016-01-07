using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Transform;
using Nuaguil.Utils.Model.Dto;

namespace Nuaguil.NhContrib.Extensions
{
    public static class QueryOverExtensions
    {
        
        public static IList<TResult> GetNonManagedEntity<TEntity, TResult>(this IQueryOver<TEntity, TEntity> query)
        {
            try
            {
                return query.TransformUsing(Transformers.AliasToBean<TResult>())
                            .List<TResult>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener una entidad no administrada", exc);
            }
        }

        public static PagedResultDto<TResult> GetPagedResult<TEntity, TResult>(this IQueryOver<TEntity, TEntity> query, int start, int max)
        {
            query.TransformUsing(Transformers.AliasToBean<TResult>()).Skip(start).Take(max);
            var source = query.Future<TResult>();
            var futureValue = query.ToRowCountInt64Query().FutureValue<long>();

            return new PagedResultDto<TResult>
            {
                Total = futureValue.Value,
                Rows = source.ToList()
            };
        }

        public static IQueryOver<T, T> OrderByAlias<T>(this IQueryOver<T, T> query, Order order)
        {
            query.UnderlyingCriteria.AddOrder(new NHibernate.Criterion.Order(order.Property, order.Direction == OrderDirection.ASC));
            return query;
        }


    }
}