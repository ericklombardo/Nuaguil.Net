using Nuaguil.Utils.Model.Dto;
using Order = NHibernate.Criterion.Order;

namespace Nuaguil.NhContrib
{
    public static class CommonModelExtensions
    {

       public static Order GetNhOrder(this Nuaguil.Utils.Model.Dto.Order order)
        {
           return order.Direction == OrderDirection.ASC
                       ? Order.Asc(order.Property)
                       : Order.Desc(order.Property);
        }
    }
}
