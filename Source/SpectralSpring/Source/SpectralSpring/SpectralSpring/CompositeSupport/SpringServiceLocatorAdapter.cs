﻿using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Practices.ServiceLocation;
using Spring.Objects.Factory;


namespace SpectralSpring.CompositeSupport
{
    /// <summary>
    /// Converts IServiceLocator calls to Spring.NET IListableObjectFactory calls
    /// </summary>
    public class SpringServiceLocatorAdapter : ServiceLocatorImplBase
    {
        private readonly IListableObjectFactory _springFactory;

        /// <summary>
        /// Creates a new adapter instance, using the specified <paramref name="factory"/>
        /// for resolving service instance requests.
        /// </summary>
        /// <param name="factory">the actual factory used for resolving service instance requests.</param>
        public SpringServiceLocatorAdapter(IListableObjectFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory", "A factory is mandatory");
            }
            _springFactory = factory;
        }

        /// <summary>
        /// Resolves a requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>
        /// The requested service instance or null, if <paramref name="key"/> is not found.
        /// </returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (key == null)
            {
                IEnumerator it = DoGetAllInstances(serviceType).GetEnumerator();
                if (it.MoveNext())
                {
                    return it.Current;
                }
                throw new ObjectCreationException(string.Format("no services of type '{0}' defined", serviceType.FullName));
            }
            return _springFactory.GetObject(key, serviceType);
        }

        /// <summary>
        /// Resolves service instances by type.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>
        /// Sequence of service instance objects matching the <paramref name="serviceType"/>.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            foreach (object o in _springFactory.GetObjectsOfType(serviceType, true, true).Values)
            {
                yield return o;
            }
        }
    }
}
