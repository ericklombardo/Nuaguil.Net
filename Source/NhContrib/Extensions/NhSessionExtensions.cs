using System;
using NHibernate;

namespace Nuaguil.NhContrib.Extensions
{
   public static class NhSessionExtensions
   {

      public static void EncloseInTransaction(this ISessionFactory sessionFactory, Action<ISession> work)
      {
         using (ISession s = sessionFactory.OpenSession())
         using (ITransaction tx = s.BeginTransaction())
         {
            try
            {
               work(s);
               tx.Commit();
            }
            catch(Exception)
            {
               tx.Rollback();
               throw;
            }
         }
      }

      public static void EncloseInTransaction(this ISessionFactory sessionFactory, Action<IStatelessSession> work)
      {
         using (IStatelessSession  s = sessionFactory.OpenStatelessSession())
         using (ITransaction tx = s.BeginTransaction())
         {
            try
            {
               work(s);
               tx.Commit();
            }
            catch(Exception)
            {
               tx.Rollback();
               throw;
            }
         }
      }

      /// <summary>
      /// Invalidate all second-level cache
      /// </summary>
      /// <param name="sessionFactory"></param>
      public static void InvalidateCache(this ISessionFactory sessionFactory)
      {
         sessionFactory.EvictQueries();
         foreach (var collectionMetadata in sessionFactory.GetAllCollectionMetadata())
            sessionFactory.EvictCollection(collectionMetadata.Key);
         foreach (var classMetadata in sessionFactory.GetAllClassMetadata())
            sessionFactory.EvictEntity(classMetadata.Key);
      }

      /// <summary>
      /// Invalidate all second-level cache for an entity
      /// </summary>
      /// <typeparam name="TEntity">Type of Entity</typeparam>
      /// <param name="sessionFactory"></param>
      public static void InvalidateCache<TEntity>(this ISessionFactory sessionFactory)
      {
         sessionFactory.Evict(typeof (TEntity));
      }

      /// <summary>
      /// Invalidate a second-level cache for a specific entity
      /// </summary>
      /// <typeparam name="TEntity">Type of Entity</typeparam>
      /// <param name="sessionFactory">SessionFactory</param>
      /// <param name="entityId">Entity Id</param>
      public static void InvalidateCache<TEntity>(this ISessionFactory sessionFactory,object entityId)
      {
         sessionFactory.Evict(typeof(TEntity),entityId);
      }

   }
}
