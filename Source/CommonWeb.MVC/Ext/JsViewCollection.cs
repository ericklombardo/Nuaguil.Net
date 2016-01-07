using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Common.Web.MVC.StringTemplateViewEngine;
using Newtonsoft.Json;

namespace Common.Web.MVC.Ext
{
    public class JsViewCollection : ICollection<JsView>
    {
        private readonly List<JsView> _list;
        private readonly string _mainViewXType;

        public string MainViewXType { get { return _mainViewXType; } }

        public JsViewCollection(string mainViewXType)
        {
            _list = new List<JsView>();
            _mainViewXType = mainViewXType;
        }

        /// <summary>
        /// Método para agregar una vista
        /// </summary>
        /// <param name="jsClass">Nombre de la clase js</param>
        /// <returns>Colección de vistas</returns>
        public JsViewCollection Add(string jsClass)
        {
            _list.Add(ViewHelper.CreateView(jsClass));
            return this;
        }

        /// <summary>
        /// Método para agregar una vista ux
        /// </summary>
        /// <param name="jsClass">Nombre de la clase js</param>
        /// <returns>Colección de vistas</returns>
        public JsViewCollection AddUx(string jsClass)
        {
            _list.Add(ViewHelper.CreateUxView(jsClass));
            return this;
        }

        /// <summary>
        /// Método para agregar una vista
        /// </summary>
        /// <param name="item">Objeto vista</param>
        /// <returns>Colección de vistas</returns>
        public JsViewCollection Add(JsView item)
        {
            _list.Add(item);
            return this;
        }


        #region Implementation of IEnumerable

        public IEnumerator<JsView> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<JsView>

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(JsView item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(JsView[] array, int arrayIndex)
        {
            _list.CopyTo(array,arrayIndex);
        }

        public bool Remove(JsView item)
        {
            return _list.Remove(item);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region ICollection<JsView> Members

        void ICollection<JsView>.Add(JsView item)
        {
            Add(item);
        }

        void ICollection<JsView>.Clear()
        {
            Clear();
        }

        bool ICollection<JsView>.Contains(JsView item)
        {
            return Contains(item);
        }

        void ICollection<JsView>.CopyTo(JsView[] array, int arrayIndex)
        {
            CopyTo(array,arrayIndex);
        }

        int ICollection<JsView>.Count
        {
            get { return Count; }
        }

        bool ICollection<JsView>.IsReadOnly
        {
            get { return IsReadOnly; }
        }

        bool ICollection<JsView>.Remove(JsView item)
        {
            return Remove(item);
        }

        #endregion

    }
}
