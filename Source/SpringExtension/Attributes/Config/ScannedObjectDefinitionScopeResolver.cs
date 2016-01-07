using System;
using System.Collections.Generic;
using Common.Logging;
using Spring.Context.Attributes;
using Spring.Core;
using Spring.Objects.Factory.Config;
using Spring.Stereotype;

namespace Nuaguil.SpringExt.Attributes.Config
{
    /// <summary>
    /// Establecer todas las clases con el atributo Service o Controller con el alcance prototype
    /// </summary>
    public class ScannedObjectDefinitionScopeResolver : IObjectFactoryPostProcessor, IOrdered
    {
        protected static readonly ILog Logger = LogManager.GetLogger<ScannedObjectDefinitionScopeResolver>();
        private int _order = int.MaxValue;

        public bool WcfServices { get; set; }
        
        public void PostProcessObjectFactory(IConfigurableListableObjectFactory factory)
        {
            IList<string> objectDefinitionNames = factory.GetObjectDefinitionNames();

            if (Logger.IsInfoEnabled)
                Logger.Info("Iniciando proceso para establecer el alcance de los services y controllers");

            foreach (string objectDefinitionName in objectDefinitionNames)
            {
                IObjectDefinition objectDefinition = factory.GetObjectDefinition(objectDefinitionName);
                if (objectDefinition is ScannedGenericObjectDefinition)
                {
                    var componentAttribute =
                        Attribute.GetCustomAttribute(objectDefinition.ObjectType, typeof (ComponentAttribute), true) as
                            ComponentAttribute;

                    if ( (componentAttribute is ControllerAttribute) || (WcfServices && componentAttribute is ServiceAttribute) )
                    {
                        if (Logger.IsInfoEnabled)
                            Logger.InfoFormat("Estableciendo alcance para la definición {0}", objectDefinitionName);
                        objectDefinition.Scope = "prototype";
                    }
                }
            }
        }

        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        protected static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}