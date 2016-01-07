using System.Runtime.Serialization;

namespace WCFChannelManager.Tests.FakeObjects
{
   
   [DataContract]
   public class DummyDto
   {

      [DataMember]
      public int A { get; set; }
      [DataMember]
      public string B { get; set; }
       
   }
}