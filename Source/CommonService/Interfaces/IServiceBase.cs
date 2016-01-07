using System.Collections.Generic;
using Nuaguil.Utils.Model.Dto;

namespace Nuaguil.CommonService.Interfaces
{
    public interface IServiceBase<in TIdT, T>
    {
        void Delete(TIdT id);
        IList<T> GetAll();
        PagedResultDto<T> GetPagedResult(int start, int max, params Order[] order);
        T GetById(TIdT id);
        T Create();
        T Save(T entity);
    }
}
