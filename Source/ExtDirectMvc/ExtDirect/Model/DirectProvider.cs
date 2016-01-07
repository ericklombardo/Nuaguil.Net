using System.Collections.Generic;

namespace ExtDirect.Model
{
   public class DirectProvider
   {
      public string type { get; set; }
      public string url { get; set; }
      public IDictionary<string, IEnumerable<DirectMethod>> actions { get; set; }
      public string @namespace { get; set; }
   }
}