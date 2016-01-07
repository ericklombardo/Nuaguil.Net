using System.Data;
using NHibernate.Connection;
using Nuaguil.Utils.DesignByContract;

namespace Nuaguil.NhContrib.Connection
{
    public abstract class ContextDriverConnectionProvider : DriverConnectionProvider
    {

        public static IContextDataProvider ContextDataProvider { set; get; }
        
        public override IDbConnection GetConnection()
        {
            Check.Require(ContextDataProvider != null,"Debe especificar el proveedor de los datos del contexto");
            var conn = base.GetConnection();
            SetContext(conn);
            return conn;
        }

        public override void CloseConnection(IDbConnection conn)
        {
            EraseContext(conn);
            base.CloseConnection(conn);
        }

        protected abstract void SetContext(IDbConnection conn);
        protected abstract void EraseContext(IDbConnection conn);

    }
}
