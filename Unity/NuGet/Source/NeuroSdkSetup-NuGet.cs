using System.Runtime.CompilerServices;
using NeuroSdk.Resources;

namespace NeuroSdk
{
    partial class NeuroSdkSetup
    {
        [ModuleInitializer]
        internal static void ModuleInitializer()
        {
            ResourceManager.InjectAssemblies();
        }
    }
}
