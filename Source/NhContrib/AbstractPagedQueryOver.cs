using System.Linq;
using NHibernate;
using NHibernate.Transform;
using Nuaguil.Utils.Model.Dto;

namespace Nuaguil.NhContrib
{
   /// <summary>
   /// Clase base para implementar consultas con criterios de búsqueda personalizados
   /// creados a partir del objeto QueryOver
   /// </summary>
   /// <typeparam name="TEntity">Tipo de dato de la entidad</typeparam>
   /// <typeparam name="TResult">Tipo de dato del Dto que representa el resultado a devolver</typeparam>
   public abstract class AbstractPagedQueryOver<TEntity,TResult> : AbstractNhBase
   {

      #region Properties

       protected AbstractPagedQueryOver(ISessionFactory sessionFactory) 
           : base(sessionFactory)
       {
       }

       public int FirstResult { get; set; }
      public int MaxResult { get; set; }

      #endregion

      #region Abstracts methods
      
      /// <summary>
      ///  Crea el objeto query
      /// </summary>
      /// <returns></returns>
      protected abstract IQueryOver<TEntity, TEntity> GetQuery();

      #endregion

      /// <summary>
      /// Ejecutar el query
      /// </summary>
      /// <returns></returns>
      public virtual PagedResultDto<TResult> Execute()
      {
         var query = GetQuery();
         
         query
            .TransformUsing(Transformers.AliasToBean<TResult>())
            .Skip(FirstResult)
            .Take(MaxResult);

         var results = query.Future<TResult>();
         var count = query.ToRowCountInt64Query().FutureValue<long>();

         return new PagedResultDto<TResult>
                   {
                      Total = count.Value,
                      Rows = results.ToList()
                   };
      }      

   }
}
