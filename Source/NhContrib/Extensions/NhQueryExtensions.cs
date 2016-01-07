using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;

namespace Nuaguil.NhContrib.Extensions
{
    public static class QueryExtensions
    {
        public static IQuery SetInt32(this IQuery query, int position, int? val)
        {
            if (val.HasValue)
            {
                query.SetInt32(position, val.Value);
            }
            else
            {
                query.SetParameter(position, null, NHibernateUtil.Int32);
            }

            return query;
        }

        public static IQuery SetInt32(this IQuery query, string name, int? val)
        {
            if (val.HasValue)
            {
                query.SetInt32(name, val.Value);
            }
            else
            {
                query.SetParameter(name, null, NHibernateUtil.Int32);
            }
            return query;
        }

        public static IQuery SetDateTime(this IQuery query, int position, DateTime? val)
        {
            if (val.HasValue)
            {
                query.SetDateTime(position, val.Value);
            }
            else
            {
                query.SetParameter(position, null, NHibernateUtil.DateTime);
            }
            return query;
        }

        public static IQuery SetDateTime(this IQuery query, string name, DateTime? val)
        {
            if (val.HasValue)
            {
                query.SetDateTime(name, val.Value);
            }
            else
            {
                query.SetParameter(name, null, NHibernateUtil.DateTime);
            }
            return query;
        }

        public static  IList<TT> GetNonManagedEntity<TT>(this IQuery query)
        {
            try
            {
                return query.SetResultTransformer(Transformers.AliasToBean(typeof(TT)))
                            .List<TT>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener una entidad no administrada", exc);
            }
        }

        public static TT GetUniqueNonManagedEntity<TT>(this IQuery query)
        {
            try
            {
                return query.SetResultTransformer(Transformers.AliasToBean(typeof(TT)))
                            .UniqueResult<TT>();
            }
            catch (HibernateException exc)
            {
                throw new PersistenceException("Error al obtener una entidad no administrada", exc);
            }
        }

    }
}