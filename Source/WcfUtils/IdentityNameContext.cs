using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Nuaguil.Wcf.Utils
{
   public class IdentityNameContext
   {
      private const string TypeName = "Identity";
      private const string TypeNamespace = "App";

      /// <summary>
      /// Obtener/Establecer el nombre de la cuenta del usuario logeado en el sistema
      /// </summary>
      public static String Current
      {
         get
         {
            OperationContext context = OperationContext.Current;
            if (context == null)
            {
               return null;
            }
            return context.IncomingMessageHeaders.GetHeader<String>(TypeName, TypeNamespace);            
         }
         set
         {
            OperationContext context = OperationContext.Current;
            MessageHeader<String> header = new MessageHeader<String>(value);
            context.OutgoingMessageHeaders.Add(header.GetUntypedHeader(TypeName, TypeNamespace));
         }
      }
   }

}
