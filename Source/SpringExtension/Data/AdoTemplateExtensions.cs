using System;
using System.Collections.Generic;
using System.Data;
using Spring.Data;
using Spring.Data.Common;
using Spring.Data.Generic;

namespace Nuaguil.SpringExt.Data
{

   public delegate T SafeRowMapperDelegate<out T>(SafeDataReader dataReader, int rowNum);
   public delegate void SafeRowCallbackDelegate(SafeDataReader dataReader);

   public static class AdoTemplateExtensions
   {
      public static IList<T> QueryWithSafeRowMapperDelegate<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, SafeRowMapperDelegate<T> rowMapperDelegate)
      {
         return adoTemplate.QueryWithResultSetExtractor(cmdType, cmdText, new SafeDataReaderRowMapperResultSetExtractor<T>(rowMapperDelegate));
      }

      public static IList<T> QueryWithSafeRowMapperDelegate<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, SafeRowMapperDelegate<T> rowMapperDelegate, ICommandSetter commandSetter)
      {
         return adoTemplate.QueryWithResultSetExtractor(cmdType, cmdText, new SafeDataReaderRowMapperResultSetExtractor<T>(rowMapperDelegate), commandSetter);
      }

      public static IList<T> QueryWithSafeRowMapperDelegate<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, SafeRowMapperDelegate<T> rowMapperDelegate, string parameterName, Enum dbType, int size, object parameterValue)
      {
         return adoTemplate.QueryWithResultSetExtractor(cmdType, cmdText, new SafeDataReaderRowMapperResultSetExtractor<T>(rowMapperDelegate), parameterName, dbType, size, parameterValue);
      }

      public static IList<T> QueryWithSafeRowMapperDelegate<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, SafeRowMapperDelegate<T> rowMapperDelegate, IDbParameters parameters)
      {
         return adoTemplate.QueryWithResultSetExtractor(cmdType, cmdText, new SafeDataReaderRowMapperResultSetExtractor<T>(rowMapperDelegate), parameters);
      }

      public static void QueryWithSafeRowCallbackDelegate(this AdoTemplate adoTemplate, CommandType cmdType, string sql, SafeRowCallbackDelegate rowCallbackDelegate)
      {
         adoTemplate.ClassicAdoTemplate.QueryWithResultSetExtractor(cmdType, sql, new SafeDataReaderRowCallbackResultSetExtractor(rowCallbackDelegate));
      }

      public static void QueryWithRowCallbackDelegate(this AdoTemplate adoTemplate, CommandType cmdType, string sql, SafeRowCallbackDelegate rowCallbackDelegate, ICommandSetter commandSetter)
      {
         adoTemplate.ClassicAdoTemplate.QueryWithResultSetExtractor(cmdType, sql, new SafeDataReaderRowCallbackResultSetExtractor(rowCallbackDelegate), commandSetter);
      }

      public static  void QueryWithRowCallbackDelegate(this AdoTemplate adoTemplate, CommandType cmdType, string sql, SafeRowCallbackDelegate rowCallbackDelegate, string name, Enum dbType, int size, object parameterValue)
      {
         adoTemplate.ClassicAdoTemplate.QueryWithResultSetExtractor(cmdType, sql, new SafeDataReaderRowCallbackResultSetExtractor(rowCallbackDelegate), name, dbType, size, parameterValue);
      }

      public static void QueryWithRowCallbackDelegate(this AdoTemplate adoTemplate, CommandType cmdType, string sql, SafeRowCallbackDelegate rowCallbackDelegate, IDbParameters parameters)
      {
         adoTemplate.ClassicAdoTemplate.QueryWithResultSetExtractor(cmdType, sql, new SafeDataReaderRowCallbackResultSetExtractor(rowCallbackDelegate), parameters);
      }



   }
}
