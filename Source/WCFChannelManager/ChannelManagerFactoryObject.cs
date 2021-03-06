﻿//Copyright 2009 Benny Michielsen

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuaguil.Wcf.Utils;
using Spring.Objects.Factory.Config;
using Spring.Util;
using Spring.Proxy;
using System.Reflection;
using Spring.Objects.Factory.Support;
using Spring.Objects.Factory;

namespace WCFChannelManager
{
    /// <summary>
    /// ChannelManagerFactoryObjects dynamically subclasses ChannelActionWrappers and implements the channel interface
    /// </summary>
    public class ChannelManagerFactoryObject
        : IConfigurableFactoryObject, IInitializingObject
    {
        protected ConstructorInfo ProxyConstructor;

        public const string SingleAction = "SingleAction";
        public const string FixedPool = "FixedPool";
        public const string VariablePool = "VariablePool";
        public const string ChannelManagerPropertyName = "ChannelManager";

       public const string BasicHttp = "BasicHttp";
       public const string NetNamedPipe = "NetNamedPipe";
       public const string NetTcp = "NetTcp";

        public static IIdentityNameContextProvider IdentityNameContextProvider;

        #region IConfigurableFactoryObject Members

        /// <summary>
        /// Get or set the template used to configure the proxy.
        /// </summary>
        public IObjectDefinition ProductTemplate
        {
            get;
            set;
        }

        #endregion

        #region IFactoryObject Members
        /// <summary>
        /// Creates a new instance of the channelmanager.
        /// </summary>
        /// <returns>New instance of the channelmanager</returns>
        public object GetObject()
        {
            if (ProxyConstructor == null)
            {
                GenerateProxy();
            }
            object channelProxy = ObjectUtils.InstantiateType(ProxyConstructor, ObjectUtils.EmptyObjects);
            SetDefaultValues(channelProxy);
            return channelProxy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelProxy"></param>
        private void SetDefaultValues(object channelProxy)
        {
            if (ProductTemplate == null || !ProductTemplate.PropertyValues.Contains(ChannelManagerPropertyName))
            {
               Type channelManagerType = null;
               Type channelType = WcfChannelProxyTypeBuilder.FindChannelType(channelProxy.GetType());
               List<Type> contructorArgumentTypes = new List<Type> { typeof(string), typeof(string), typeof(string), typeof(string) };
               List<object> constructorArguments = new List<object> {EndpointConfigurationName, EndPointAddressUri,Binding,BindingConfiguration};
               if (string.IsNullOrEmpty(ChannelManagementMode) || ChannelManagementMode.Equals(SingleAction))
                {
                    channelManagerType = typeof(SingleActionChannelManager<>).MakeGenericType(channelType);
               
                }
                else
                {
                    channelManagerType = typeof(ChannelPoolManager<>).MakeGenericType(channelType);
                    contructorArgumentTypes.Add(typeof(IPoolFactory));
                    if (ChannelManagementMode.Equals(FixedPool))
                    {
                        constructorArguments.Add(new SimplePoolFactory());
                    }
                    else
                    {
                        constructorArguments.Add(new AutoSizePoolFactory());
                    }
                }
                ConstructorInfo managerConstructor = channelManagerType.GetConstructor(contructorArgumentTypes.ToArray());
                object channelManager = ObjectUtils.InstantiateType(managerConstructor, constructorArguments.ToArray());
                PropertyInfo property = channelProxy.GetType().GetProperty(ChannelManagerPropertyName);
                property.SetValue(channelProxy, channelManager, null);
            }
        }

        /// <summary>
        /// Is the factory a singleton.
        /// </summary>
        public bool IsSingleton
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the type of the channelmanager.
        /// </summary>
        public Type ObjectType
        {
            get
            {
                return (ProxyConstructor != null ? ProxyConstructor.DeclaringType : ChannelActionWrapperType);
            }

        }

        #endregion
        /// <summary>
        /// The type used for subclassing, default is ProxyChannel.
        /// </summary>
        public virtual Type ChannelActionWrapperType
        {
            get;
            set;
        }
        /// <summary>
        /// The channel interface to implement.
        /// </summary>
        public virtual Type ChannelType
        {
            get;
            set;
        }
        /// <summary>
        /// The mode on how the channel is managed. Default is "SingleAction".
        /// </summary>
        public virtual string ChannelManagementMode
        {
            get;
            set;
        }
        /// <summary>
        /// The name of the endpoint in system.servicemodel section.
        /// </summary>
        public virtual string EndpointConfigurationName
        {
            get;
            set;
        }

       /// <summary>
       /// The name of the binding to use
       /// </summary> 
       public virtual string Binding { get; set; }

       /// <summary>
       /// The name of the binding configuration
       /// </summary>
       public virtual string BindingConfiguration { get; set; }

        /// <summary>
        /// The Uri address of the endpoint
        /// </summary>
        public virtual string EndPointAddressUri { get; set; }

       /// <summary>
        /// Indicate if set the IdentityNameContext in the wcf message header
       /// </summary>
       public virtual Boolean SetIdentityNameContext { get; set; }

        /// <summary>
        /// Returns the type of the channelactionwrapper to use.
        /// </summary>
        /// <returns>The channel type.</returns>
        protected Type BuildChannelActionWrapperType()
        {
            Type baseClass = null;
            if (ChannelActionWrapperType == null)
            {
                baseClass = typeof(ProxyChannel<>).MakeGenericType(ChannelType);
            }
            else if (ChannelActionWrapperType.IsGenericType)
            {
                baseClass = ChannelActionWrapperType.MakeGenericType(ChannelType);
            }
            else
            {
                baseClass = ChannelActionWrapperType;
            }
            return baseClass;
        }

        /// <summary>
        /// Returns the channel type.
        /// </summary>
        /// <param name="channelActionWrapperType">The channelActionWrapperType being used.</param>
        /// <returns>The channel type.</returns>
        protected virtual Type BuildChannelType(Type channelActionWrapperType)
        {
            Type channelType = null;
            if (ChannelType != null)
            {
                channelType = ChannelType;
            }
            else
            {
                channelType = WcfChannelProxyTypeBuilder.FindChannelType(channelActionWrapperType);
            }
            return channelType;
        }

        /// <summary>
        /// Generates a subclass of the specified ChannelActionWrapperType and the ChannelType.
        /// </summary>
        protected virtual void GenerateProxy()
        {
            Type baseClass = BuildChannelActionWrapperType();
            IProxyTypeBuilder builder = new WcfChannelProxyTypeBuilder(baseClass);
            Type wrapper = builder.BuildProxyType();
            ProxyConstructor = wrapper.GetConstructor(Type.EmptyTypes);
        }



        #region IInitializingObject Members
        /// <summary>
        /// Validates the configuration.
        /// </summary>
        public virtual void AfterPropertiesSet()
        {
            if ((ChannelType == null && ChannelActionWrapperType == null) ||
                (ChannelType == null && ChannelActionWrapperType != null && WcfChannelProxyTypeBuilder.FindChannelType(ChannelActionWrapperType).IsGenericParameter))
            {
                throw new ArgumentException("No channeltype supplied.");
            }
            if (string.IsNullOrEmpty(EndpointConfigurationName))
            {
               if(string.IsNullOrEmpty(Binding)) 
                  throw new ArgumentException("No endpoint or binding supplied.");
            }
        }

        #endregion
    }
}
