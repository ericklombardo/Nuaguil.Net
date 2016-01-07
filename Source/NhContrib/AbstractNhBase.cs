using NHibernate;

namespace Nuaguil.NhContrib
{
   public abstract  class AbstractNhBase
   {

      #region Properties

     protected AbstractNhBase(ISessionFactory sessionFactory)
     {
           _sessionFactory = sessionFactory;
     }

       private readonly ISessionFactory _sessionFactory;

       protected ISessionFactory SessionFactory
       {
           get { return _sessionFactory; }
       }

      protected ISession NHibernateSession
      {
         get
         {
            try
            {
               return _sessionFactory.GetCurrentSession();
            }
            catch (HibernateException ex)
            {
               throw new PersistenceException("Error al obtener la sesión", ex);
            }
         }
      }

      #endregion

   }
}
