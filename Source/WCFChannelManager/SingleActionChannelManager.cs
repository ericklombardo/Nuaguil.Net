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

namespace WCFChannelManager
{
    /// <summary>
    /// Creates a channel for one time use.
    /// </summary>
    /// <typeparam name="TChannel">The type of the channel.</typeparam>
    public class SingleActionChannelManager<TChannel>
        : ChannelManagerBase<TChannel>
    {

       /// <summary>
       /// Constructor which takes the name of a valid endpoint.
       /// </summary>
       /// <param name="endpointConfigurationName">The name of the endpoint.</param>
       /// <param name="endPointAddressUri">Uri address</param>
       public SingleActionChannelManager(string endpointConfigurationName, string endPointAddressUri)
          : base(endpointConfigurationName, endPointAddressUri)
       {

       }

       /// <summary>
       /// Constructor which takes the name of a valid endpoint.
       /// </summary>
       /// <param name="endpointConfigurationName">The name of the endpoint.</param>
       /// <param name="endPointAddressUri">Uri address</param>
       /// <param name="binding">The name of the type binding</param>
       /// <param name="bindingConfiguration">The name of the binding configuration</param>
       public SingleActionChannelManager(string endpointConfigurationName, string endPointAddressUri,string binding,string bindingConfiguration)
            : base(endpointConfigurationName,endPointAddressUri,binding,bindingConfiguration)
        {

        }


       /// <summary>
        /// Constructor which takes a channelfactory which can be used by this instance.
        /// </summary>
        /// <param name="factory"></param>
        public SingleActionChannelManager(ICanCreateChannels<TChannel> factory)
            : base(factory)
        { 
     
        }

        /// <summary>
        /// Creates a channel that can be used.
        /// </summary>
        /// <returns>A usable channel.</returns>
        public override TChannel FetchChannelToWorkWith()
        {
           var channel =  CreateAndOpenChannel();
           var clientChannel = channel as IClientChannel;
           if (clientChannel != null) 
              clientChannel.Faulted += (sender, args) => clientChannel.Abort();
           
           return channel;
        }

       /// <summary>
       /// Closes the channel being passed.
       /// </summary>
       /// <param name="channel">The channel to close.</param>
       /// <param name="fault"></param>
       public override void FinishedWorkWithChannel(TChannel channel, bool fault)
       {

         var clientChannel = channel as IClientChannel;

         if (clientChannel == null) return;
          
         try
         {
            clientChannel.Close();
            clientChannel.Dispose();
         }
         catch (TimeoutException exc)
         {
            clientChannel.Abort();
            if (Logger.IsErrorEnabled)
            {
               Logger.Error("Error closing channel",exc);
            }     
         }
         catch (CommunicationException exc)
         {
            clientChannel.Abort();
            if (Logger.IsErrorEnabled)
            {
               Logger.Error("Error closing channel", exc);
            }
         }
         catch (Exception)
         {
            clientChannel.Abort();
            throw;
         }

       }


    }

    
}
