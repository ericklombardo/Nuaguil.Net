using System.Text.RegularExpressions;
using Spring.Context.Attributes;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace Nuaguil.SpringExt.Attributes
{
    /// <summary>
    /// Establecer el nombre de las definiciones de los objetos con el nombre de la clase, sin el namespace
    /// </summary>
    public class TypeObjectNameGenerator : IObjectNameGenerator
    {
        protected static Regex ClassFullNameRegEx = new Regex(@"\.[A-Z0-9]+$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public string GenerateObjectName(IObjectDefinition definition, IObjectDefinitionRegistry registry)
        {
            var objectDefinition = definition as ScannedGenericObjectDefinition;
            if (objectDefinition != null)
            {
                string componentName = objectDefinition.ComponentName;
                if (!string.IsNullOrEmpty(componentName))
                    return componentName;
            }
            return BuildDefaultObjectName(definition);
        }

        private string BuildDefaultObjectName(IObjectDefinition definition)
        {
            var match = ClassFullNameRegEx.Match(definition.ObjectType.FullName);
            return match.Value.Substring(1);
        }
    }
}