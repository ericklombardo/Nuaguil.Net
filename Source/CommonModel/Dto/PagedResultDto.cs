using System;
using System.Collections.Generic;

namespace Nuaguil.CommonModel.Dto
{
    [Serializable]
    public class PagedResultDto<T>
    {
        private long _total;
        private List<T> _rows;


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


        public List<T> Rows
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

    }
}