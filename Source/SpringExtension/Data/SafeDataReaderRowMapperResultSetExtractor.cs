using System.Collections.Generic;
using System.Data;
using Spring.Data.Generic;
using Spring.Util;

namespace Nuaguil.SpringExt.Data
{
    public class SafeDataReaderRowMapperResultSetExtractor<T> : IResultSetExtractor<IList<T>>
    {
        private readonly IRowMapper<T> _rowMapper;
        private readonly SafeRowMapperDelegate<T> _rowMapperDelegate;

        #region Constructors

        public SafeDataReaderRowMapperResultSetExtractor(IRowMapper<T> rowMapper)
        {
            AssertUtils.ArgumentNotNull(rowMapper, "rowMapper");
            _rowMapper = rowMapper;
        }

        public SafeDataReaderRowMapperResultSetExtractor(SafeRowMapperDelegate<T> rowMapperDelegate)
        {
            AssertUtils.ArgumentNotNull(rowMapperDelegate, "rowMapperDelegate");
            _rowMapperDelegate = rowMapperDelegate;
        }

        #endregion

        public IList<T> ExtractData(IDataReader reader)
        {
            IList<T> list = new List<T>();
            int num = 0;
            if (_rowMapper != null)
            {
                using (SafeDataReader sdr = new SafeDataReader(reader))
                {
                    while (sdr.Read())
                        list.Add(_rowMapper.MapRow(sdr, num++));
                }
            }
            else
            {
                using (SafeDataReader sdr = new SafeDataReader(reader))
                {
                    while (sdr.Read())
                        list.Add(_rowMapperDelegate(sdr, num++));
                }
            }
            return list;
        }
    }
}