using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nuaguil.CommonModel.Dto
{
    public abstract class AbstractNodeTreeDto
    {

        private bool _leaf = true;
        public int id { get; set; }

        public List<AbstractNodeTreeDto> children { get; set; }
        public bool leaf
        {
            get { return _leaf; }
            set { _leaf = value; }
        }
        public int? PadreId { get; set; }

        public string iconCls { get; set; }

        [XmlIgnore]
        public AbstractNodeTreeDto Padre { get; set; }

        public void AddChild(AbstractNodeTreeDto child)
        {
            if (children == null)
                children = new List<AbstractNodeTreeDto>();
            _leaf = false;
            child.Padre = this;
            children.Add(child);
        }

        public List<T> BuildTree<T>(List<T> plainTree)
           where T : AbstractNodeTreeDto
        {
            return BuildTree(plainTree, x => String.Empty);
        }

        public List<T> BuildTree<T>(List<T> plainTree, Func<T, string> getIconCls)
           where T : AbstractNodeTreeDto
        {
            AbstractNodeTreeDto anterior = null;
            AbstractNodeTreeDto padre = null;
            foreach (AbstractNodeTreeDto item in plainTree)
            {

                item.iconCls = getIconCls((T)item);

                if (!item.PadreId.HasValue)
                    AddChild(item);

                if (anterior != null && item.PadreId.HasValue)
                {
                    padre = anterior;
                    while (item.PadreId != padre.id)
                        padre = padre.Padre;

                    padre.AddChild(item);
                }

                anterior = item;
            }

            return new List<T> { (T)this };
        }


    }
}
