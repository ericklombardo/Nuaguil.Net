using System.Configuration;
using ExtDirect.Model;

namespace ExtDirect.Config
{
   public class ExtDirectSection : ConfigurationSection
   {

      public const string SectionName = "extDirect";
      private const string NamespaceProperty = "namespace";
      private const string StrategyProperty = "strategy";


      [ConfigurationProperty(NamespaceProperty, IsRequired = false)]
      public string Namespace
      {
         get { return (string)this[NamespaceProperty]; }
         set { this[NamespaceProperty] = value; }
      }

      [ConfigurationProperty(StrategyProperty, IsRequired = false)]
      public StrategyType Strategy
      {
         get { return (StrategyType) this[StrategyProperty]; }
         set { this[StrategyProperty] = value; }
      }

      public static ExtDirectSection GetExtDirectSection()
      {
         ExtDirectSection section = ConfigurationManager.GetSection(SectionName) as ExtDirectSection;

         return section;
      }


   }
}