using System;
using System.Collections.Generic;
using Nuaguil.CommonService.Interfaces;
using Nuaguil.NhContrib;
using Nuaguil.Utils.Model.Dto;
using Spring.Transaction.Interceptor;

namespace Nuaguil.CommonService
{
    public abstract class ServiceBase<T,TIdT> : 
        IServiceBase<TIdT,T> 
    {

        private IDao<T,TIdT> _dao;

        protected IDao<T, TIdT> Dao
        {
            get
            {
                return _dao;
            }
            set
            {
                _dao = value;
            }
        }

        public ServiceBase(IDao<T, TIdT> dao)
        {
            _dao = dao;
        }

        public virtual T Create()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        [Transaction]
        public virtual T Save(T entity)
        {

            _dao.SaveOrUpdate(entity);
            return entity;
        }

        [Transaction]
        public virtual void Delete(TIdT id)
        {
            T entity = _dao.GetById(id);
            _dao.Delete(entity);
        }
        [Transaction(ReadOnly = true)]
        public virtual IList<T> GetAll()
        {
           
            IList<T> data = _dao.GetAll();
            return data;
        }
        [Transaction(ReadOnly = true)]
        public virtual PagedResultDto<T> GetPagedResult(int start, int max, params Order[] order)
        {
            IList<T> data = _dao.GetPagedResult(start, max,order);
            PagedResultDto<T> pagedResult = new PagedResultDto<T>();
            pagedResult.Rows = (List<T>) data;
            pagedResult.Total = _dao.GetRowCount();
            return pagedResult;
        }
        [Transaction(ReadOnly = true)]
        public virtual T GetById(TIdT id)
        {
            T entity = _dao.GetById(id, false);
            return entity;
        }

        
    }


}