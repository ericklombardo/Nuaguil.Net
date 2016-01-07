//Copyright 2009 Benny Michielsen

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
using System.ServiceModel;
using System.ServiceModel.Channels;
using Common.Logging;

namespace WCFChannelManager
{
    /// <summary>
    /// Base class for channelmanagers.
    /// </summary>
    /// <typeparam name="TChannel">The type of the channel</typeparam>
    public abstract class ChannelManagerBase<TChannel>
         : IChannelManager<TChannel>
    {

       public static readonly ILog Logger = LogManager.GetLogger("WCFChannelManager");

       /// <summary>
       /// Constructor taking the name of an endpoint.
       /// </summary>
       /// <param name="endpointConfigurationName">Name of an endpoint.</param>
       /// <param name="endPointAddressUri">Uri Address of the service</param>
       protected ChannelManagerBase(string endpointConfigurationName,string endPointAddressUri)
            : this(new ChannelCreator<TChannel>(
               String.IsNullOrEmpty(endPointAddressUri) ?
               new ChannelFactory<TChannel>(endpointConfigurationName) :
               new ChannelFactory<TChannel>(endpointConfigurationName,new EndpointAddress(endPointAddressUri))))
        {
     
        }

       /// <summary>
       /// Constructor taking the name of an endpoint.
       /// </summary>
       /// <param name="endpointConfigurationName">Name of an endpoint.</param>
       /// <param name="endPointAddressUri">Uri Address of the service</param>
       /// <param name="binding">The name of the type binding</param>
       /// <param name="bindingConfiguration">The name of the binding configuration</param>
       protected ChannelManagerBase(string endpointConfigurationName, string endPointAddressUri, string binding, string bindingConfiguration)
       {
          ChannelFactory<TChannel> channelFactory=null;

          if (!String.IsNullOrEmpty(endpointConfigurationName))
          {
             channelFactory = String.IsNullOrEmpty(endPointAddressUri)
                                 ? new ChannelFactory<TChannel>(endpointConfigurationName)
                                 : new ChannelFactory<TChannel>(endpointConfigurationName,new EndpointAddress(endPointAddressUri));
          }
          else 
          {
             if(String.IsNullOrEmpty(endPointAddressUri))
                throw new Exception("Must specify the endpoint address");
             if(String.IsNullOrEmpty(binding))
                throw new Exception("Must specify the type of binding");

             channelFactory = new ChannelFactory<TChannel>(CreateBinding(binding, bindingConfiguration),
                                                            new EndpointAddress(endPointAddressUri)); 
          }
          
          ChannelCreater = new ChannelCreator<TChannel>(channelFactory);
       }

        /// <summary>
        /// Constructor taking a channelfactory.
        /// </summary>
        /// <param name="factory">The channelfactory to use in this instance.</param>
        protected ChannelManagerBase(ICanCreateChannels<TChannel> factory)
        {
            ChannelCreater = factory;
        }


        public Binding CreateBinding(string bindingType, string bindingConfiguration)
        {
           bindingConfiguration = bindingConfiguration ?? string.Empty;
           switch (bindingType)
           {
              case ChannelManagerFactoryObject.BasicHttp:
                 return new BasicHttpBinding(bindingConfiguration);
              case ChannelManagerFactoryObject.NetTcp:
                 return new NetTcpBinding(bindingConfiguration);
              case ChannelManagerFactoryObject.NetNamedPipe:
                 return new NetNamedPipeBinding(bindingConfiguration);
              default:
                 throw new Exception(string.Format("Binding type invalid: {0}. Must specify a valid binding type",bindingType));
           }           
        }

        /// <summary>
        /// The channelfactory.
        /// </summary>
        public virtual IChannelFactory<TChannel> ChannelFactory
        {
            get
            {
                return ChannelCreater.ChannelFactory;
            }
        }

        public virtual ICanCreateChannels<TChannel> ChannelCreater
        {
            get;
            set;
        }

        /// <summary>
        /// Indicate if set the IdentityNameContext in the wcf message header
        /// </summary>
        public Boolean SetIdentityNameContext { get; set; }

        /// <summary>
        /// Get a channel on which actions can be performed.
        /// </summary>
        /// <returns>A working channel.</returns>
        public abstract TChannel FetchChannelToWorkWith();

       /// <summary>
       /// Perform any cleanup work needed when an action has been performed on a channel.
       /// </summary>
       /// <param name="channel">The channel that was used.</param>
       /// <param name="fault"></param>
       public abstract void FinishedWorkWithChannel(TChannel channel, bool fault);

        /// <summary>
        /// Creates and opens a channel.
        /// </summary>
        /// <returns>An open channel.</returns>
        protected virtual TChannel CreateAndOpenChannel()
        {
            return OpenChannel(CreateChannel());
           
        }

        /// <summary>
        /// Creates a channel
        /// </summary>
        /// <returns></returns>
        protected virtual TChannel CreateChannel()
        {
            TChannel channel = ChannelCreater.CreateChannel();
           return channel;
        }

        /// <summary>
        /// Open the channel.
        /// </summary>
        /// <param name="channel">The channel to open.</param>
        /// <returns>An opened channel.</returns>
        protected virtual TChannel OpenChannel(TChannel channel)
        {
            ICommunicationObject communicationChannel = channel as ICommunicationObject;
            if (communicationChannel != null && communicationChannel.State == CommunicationState.Created)
            {
                communicationChannel.Open();
            }
            return channel;
        }

        protected virtual void CloseChannel(TChannel channel)
        {
            ICommunicationObject communicationChannel = channel as ICommunicationObject;
            if (communicationChannel != null)
            {
                communicationChannel.Close();
            }
        }

        protected virtual void AbortChannel(TChannel channel)
        {
           ICommunicationObject communicationChannel = channel as ICommunicationObject;
           if (communicationChannel != null)
           {
              communicationChannel.Abort();
           }           
        }
    }
}
