using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NUnit.Framework;
using WCFChannelManager.Tests.FakeObjects;

namespace WCFChannelManager.Tests
{
   
   [TestFixture]
   public class DummyServiceTests
   {

      protected ServiceHost Host;

      [TestFixtureSetUp]
      public void Setup()
      {
         Host = new ServiceHost(typeof(DummyService));
         Host.Open();
      }

      [Test]
      public void DummyService_DoWork()
      {
         Binding binding = new BasicHttpBinding();
         EndpointAddress address = new EndpointAddress("http://localhost:8733/DummyService/");

         var dummyService = ChannelFactory<IDummyService>.CreateChannel(binding, address);

         Console.Write( dummyService.DoWork());
      }

      [Test]
      public void DummyService_WithProxyChannel_ReturnNull()
      {
         Binding binding = new BasicHttpBinding();
         EndpointAddress address = new EndpointAddress("http://localhost:8733/DummyService/");

         var channelFactory = new ChannelFactory<IDummyService>(binding, address);
         ICanCreateChannels<IDummyService> channelCreator = new ChannelCreator<IDummyService>(channelFactory);
         IChannelManager<IDummyService> channelManager = new SingleActionChannelManager<IDummyService>(channelCreator);

         var proxy = new DummyProxyChannel(channelManager);

         proxy.Execute("ReturnNull", null, Type.EmptyTypes);

      }

      [Test]
      public void DummyService_WithFactoryObject()
      {
         var factory = new ChannelManagerFactoryObject
         {
            ChannelType = typeof(IDummyService),
            EndPointAddressUri = "http://localhost:8733/DummyService/",
            Binding = "BasicHttp",
            ChannelManagementMode = "SingleAction"
         };

         var proxy = factory.GetObject() as IDummyService;
         proxy.ReturnNull();
         Console.WriteLine(proxy.ReturnInt());
         Console.WriteLine(proxy.DoWork());
         Console.WriteLine(proxy.ReturnDtos()[0].B);
      }


      [TestFixtureTearDown]
      public void CloseHost()
      {
         Host.Close();
      }


       
   }
}