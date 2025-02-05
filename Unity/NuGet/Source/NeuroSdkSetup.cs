using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using NeuroSdk.Il2Cpp;
using NeuroSdk.Internal;
using NeuroSdk.Resources;

namespace NeuroSdk
{
    partial class NeuroSdkSetup
    {
#pragma warning disable CS0436 // Type conflicts with imported type
        [ModuleInitializer]
#pragma warning restore CS0436 // Type conflicts with imported type
        [Obsolete("This method is only for compiler use and should not be called directly.", true)]
        internal static void ModuleInitializer()
        {
            ResourceManager.InjectAssemblies();
            ClassInjectorPatch.Patch();
            RegisterInIl2CppAttribute.Register(Assembly.GetExecutingAssembly());
        }
    }
}
