using System.Data;

namespace Nuaguil.NhContrib.Connection
{
    public class MsSqlContextDriverConnectionProvider : ContextDriverConnectionProvider
    {


        public override IDbConnection GetConnection()
        {            
            var conn = base.GetConnection();
            SetContext(conn);
            return conn;
        }

        public override void CloseConnection(IDbConnection conn)
        {
            EraseContext(conn);
            base.CloseConnection(conn);
        }


        protected override void EraseContext(IDbConnection conn)
        {
            SetContext(conn,ContextDataProvider.GetEmptyData());
        }

        protected override void SetContext(IDbConnection conn)
        {
            string user = ContextDataProvider.GetData();
            SetContext(conn, user);
        }
        

        protected void SetContext(IDbConnection conn,string data)
        {
            IDbCommand cmd = conn.CreateCommand();
            IDbDataParameter param = cmd.CreateParameter();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "exec sp_set_context_info @data = @data";

            param.ParameterName = "@data";
            param.DbType = DbType.AnsiString;
            param.Size = 127;
            param.Value = data;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();            
        }

        
    }
}
