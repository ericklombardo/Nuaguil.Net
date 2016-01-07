using System;
using System.Collections.Generic;

namespace Nuaguil.Utils.Model.Dto
{
    [Serializable]
    public class PagedResultDto<T>
    {
        private long _total;
        private IEnumerable<T> _rows;


        public long Total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = value;
            }
        }


        public IEnumerable<T> Rows
        {
            get
            {
                return _rows;
            }
            set
            {
                _rows = value;
            }
        }

        public PagedResultDto()
        {
        }

       public PagedResultDto(long total, IEnumerable<T> rows)
       {
          _total = total;
          _rows = rows;
       }
    }
}