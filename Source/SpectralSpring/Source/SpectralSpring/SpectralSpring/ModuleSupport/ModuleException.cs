using System;

namespace SpectralSpring.ModuleSupport
{
    public class ModuleInitializationException:Exception
    {
        public ModuleInitializationException(string moduleName, string message) 
            : base ("Cannot initialize module " + moduleName + " , error is: "+ message){}
        
    }
}
