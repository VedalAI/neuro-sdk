#nullable enable

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using NeuroSdk.Il2Cpp;
using NeuroSdk.Resources;

namespace NeuroSdk
{
    partial class NeuroSdkSetup
    {
        [ModuleInitializer]
        [Obsolete("This method is only for compiler use and should not be called directly.", true)]
        internal static void ModuleInitializer()
        {
            ResourceManager.InjectAssemblies();
            RegisterInIl2CppAttribute.Register(Assembly.GetExecutingAssembly());
        }
    }
}
