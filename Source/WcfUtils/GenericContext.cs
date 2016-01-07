using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using Nuaguil.Utils.DesignByContract;

namespace Nuaguil.Wcf.Utils
{

   [DataContract]
   public class GenericContext<T>
   {
      [DataMember]
      public readonly T Value;

      internal static string TypeName;
      internal static string TypeNamespace;
      
      static GenericContext()
      {
         //Verify [DataContract] or [Serializable] on T
         Check.Require(IsDataContract(typeof(T)) || typeof(T).IsSerializable);
         TypeNamespace = "net.clr:" + typeof(T).FullName;
         TypeName = "GenericContext";
      }
      static bool IsDataContract(Type type)
      {
         object[] attributes =
         type.GetCustomAttributes(typeof(DataContractAttribute),false);
         return attributes.Length == 1;
      }
      public GenericContext(T value)
      {
         Value = value;
      }

      public GenericContext() : this(default(T))
      {}
      public static GenericContext<T> Current
      {
         get
         {
            OperationContext context = OperationContext.Current;
            if(context == null)
            {
               return null;
            }
            try
            {
               return context.IncomingMessageHeaders.
                        GetHeader<GenericContext<T>>(TypeName,TypeNamespace);
            }
            catch
            {
               return null;
            }
         }
         set
         {
            OperationContext context = OperationContext.Current;
            Check.Require(context != null);
            //Having multiple GenericContext<T> headers is an error
            bool headerExists = false;
            try
            {
               context.OutgoingMessageHeaders.
               GetHeader<GenericContext<T>>(TypeName, TypeNamespace);
               headerExists = true;
            }
            catch (MessageHeaderException exception)
            {
               Check.Require(exception.Message == "There is not a header with name " +
               TypeName + " and namespace " +
               TypeNamespace + " in the message.");
            }
            if (headerExists)
            {
               throw new InvalidOperationException("A header with name " + TypeName +
               " and namespace " + TypeNamespace +
               " already exists in the message.");
            }
            MessageHeader<GenericContext<T>> genericHeader =
            new MessageHeader<GenericContext<T>>(value);
            context.OutgoingMessageHeaders.Add(
               genericHeader.GetUntypedHeader(TypeName, TypeNamespace));
         }
      }
   }

}
