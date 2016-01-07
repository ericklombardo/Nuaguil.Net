using System;

namespace Nuaguil.CommonModel.Dto
{
    [Serializable]
    public class Order
    {
        public string Property { get; set;}
        public OrderDirection Direction{ get; set;}

        public string ToString(string alias)
        {
            return String.Format("{0}.{1} {2}",alias,Property, Direction);
        }

        public override string ToString()
        {
            return String.Format("{0} {1}",Property, Direction);
        }

        public Order()
        {
        }

        public static Order Create(string property,OrderDirection direction)
        {
            Order order = new Order();
            order.Property = property;
            order.Direction = direction;
            return order;
        }
    
    }

    public enum OrderDirection
    {
        ASC,DESC
    }
}