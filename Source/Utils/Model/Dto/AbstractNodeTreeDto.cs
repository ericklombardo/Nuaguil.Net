using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Nuaguil.Utils.DesignByContract;

namespace Nuaguil.Utils.Model.Dto
{
   [DataContract]
   public abstract class AbstractNodeTreeDto
   {
      // Fields
      private bool _leaf = true;

      public AbstractNodeTreeDto()
      {
         start = 0;
      }

      // Properties
      [DataMember]
      public List<AbstractNodeTreeDto> children { get; set; }

      [DataMember]
      public string iconCls { get; set; }

      [DataMember]
      public int id { get; set; }

      [DataMember]
      public bool leaf
      {
         get
         {
            return this._leaf;
         }
         set
         {
            this._leaf = value;
         }
      }

      [DataMember]
      public int? PadreId { get; set; }

      public AbstractNodeTreeDto Padre { get; set; }

      [DataMember]
      public int start { get; set; }

      public void AddChild(AbstractNodeTreeDto child)
      {
         if (this.children == null)
         {
            this.children = new List<AbstractNodeTreeDto>();
         }
         _leaf = false;
         child.Padre = this;
         children.Add(child);
      }

      public List<T> BuildTree<T>(List<T> plainTree) where T : AbstractNodeTreeDto
      {
         return this.BuildTree<T>(plainTree, arg => string.Empty);
      }

      public virtual List<T> BuildTree<T>(List<T> plainTree, Func<T, string> getIconCls) where T : AbstractNodeTreeDto
      {
         AbstractNodeTreeDto anterior = null;
         AbstractNodeTreeDto padre;
         foreach (AbstractNodeTreeDto node in plainTree)
         {
            node.iconCls = getIconCls((T)node);
            if (!node.PadreId.HasValue)
            {
               AddChild(node);
            }

            if (anterior != null && node.PadreId.HasValue)
            {
               padre = anterior;
               while (node.PadreId != padre.id)
               {
                  padre = padre.Padre;
                  Check.Ensure(padre != null, String.Format("Error en la jerarquia. El padre del nodoId = {0} no existe", node.id));
               }

               padre.AddChild(node);
            }

            anterior = node;
         }

         return new List<T> { (T)this };
      }

   }
}

