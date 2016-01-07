using System.Data;
using Spring.Data;
using Spring.Util;

namespace Nuaguil.SpringExt.Data
{
   public class SafeDataReaderRowCallbackResultSetExtractor : IResultSetExtractor
   {

    private readonly IRowCallback _rowCallback;
    private readonly SafeRowCallbackDelegate _rowCallbackDelegate;

    public SafeDataReaderRowCallbackResultSetExtractor(IRowCallback rowCallback)
    {
      AssertUtils.ArgumentNotNull(rowCallback, "rowCallback");
      _rowCallback = rowCallback;
    }

    public SafeDataReaderRowCallbackResultSetExtractor(SafeRowCallbackDelegate rowCallbackDelegate)
    {
      AssertUtils.ArgumentNotNull(rowCallbackDelegate, "safeRowCallbackDelegate");
      _rowCallbackDelegate = rowCallbackDelegate;
    }

    public object ExtractData(IDataReader reader)
    {
      if (_rowCallback != null)
      {
         using (SafeDataReader sdr = new SafeDataReader(reader))
         {
            while (sdr.Read())
               _rowCallback.ProcessRow(sdr);
         }
      }
      else
      {
         using (SafeDataReader sdr = new SafeDataReader(reader))
         {
            while (sdr.Read())
               _rowCallbackDelegate(sdr);
         }
      }
      return null;
    }

   }
}
