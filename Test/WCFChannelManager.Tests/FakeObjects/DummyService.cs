using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFChannelManager.Tests.FakeObjects
{
   // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DummyService" in both code and config file together.
   public class DummyService : IDummyService
   {
      public string DoWork()
      {
         return "testing";
      }

      public string ReturnNull()
      {
         return null;
      }

      public int ReturnInt()
      {
         return 1;
      }

      public IList<DummyDto> ReturnDtos()
      {
         return new List<DummyDto>
         {
            new DummyDto {A = 1, B = "prueba"},
            new DummyDto {A = 2, B = "prueba2"}
         };
      }
   }
}
