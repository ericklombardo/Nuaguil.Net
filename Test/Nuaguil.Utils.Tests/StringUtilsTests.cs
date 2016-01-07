using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuaguil.Utils;
using NUnit.Framework;

namespace Nuaguil.Utils.Tests
{
 
   [TestFixture]
   public class StringUtilsTests
    {

      [Test]
      public void ToUpperOrNull_ConvertToUpper()
      {
         string value ="erick";
         string expected = "ERICK";

         Assert.True(value.ToUpperOrNull()==expected);
      }

      [Test]
      public void ToUpper_NullString_ThrowExc()
      {
         string value = null;
         Assert.Throws<NullReferenceException>(() => value.ToUpper());
      }

      [Test]
      public void ToUpperOrNull_NullString_ReturnNull()
      {
         string value = null;
         Assert.Null(value.ToUpperOrNull());
      }


    }
}
