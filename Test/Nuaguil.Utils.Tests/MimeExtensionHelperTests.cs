using NUnit.Framework;

namespace Nuaguil.Utils.Tests
{
   
   [TestFixture]
   public class MimeExtensionHelperTests
   {

      [Test]
      public void FindExtension_ExtensionRegistered_ExtensionFound()
      {
         string extension = MimeExtensionHelper.FindExtension("text/csv");
         Assert.That(extension,Is.EqualTo(".csv"));
      }

      [Test]
      public void FindExtension_ExtensionNotRegistered_ShouldReturnNull()
      {
         string extension = MimeExtensionHelper.FindExtension("abc/abc");
         Assert.That(extension, Is.Null);
      }

      [Test]
      public void FindMimeType_NotVerifyContent_ReturnMimeType()
      {
         string mimeType = MimeExtensionHelper.FindMimeType("Files\\Empleados.xls");
         Assert.That(mimeType, Is.EqualTo("application/vnd.ms-excel"));
      }

      [Test]
      public void FindMimeType_VerifyContent_ReturnMimeType()
      {
         string mimeType = MimeExtensionHelper.FindMimeType("Files\\Empleados.xls",true);
         Assert.That(mimeType, Is.EqualTo("application/vnd.ms-excel"));
      }

      [Test]
      public void FindMimeByContent_ReturnMimeType()
      {
         string mimeType = MimeExtensionHelper.FindMimeByContent("Files\\Empleados.xls");
         Assert.That(mimeType, Is.EqualTo("application/vnd.ms-excel"));         
      }

 
   }
}