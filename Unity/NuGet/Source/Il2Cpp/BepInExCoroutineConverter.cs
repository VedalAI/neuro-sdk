#nullable enable

using System;
using System.Linq;
using System.Reflection;

namespace NeuroSdk.Il2Cpp
{
    // ReSharper disable once UnusedType.Global
    public sealed class BepInExCoroutineConverter : ICoroutineConverter
    {
        private static Assembly? BepInExAssembly => AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "BepInEx.Unity.IL2CPP");

        public bool CanBeUsed => BepInExAssembly != null;

        public object ConvertToIl2Cpp(System.Collections.IEnumerator enumerator)
        {
            Type? collectionExtensions = BepInExAssembly!.GetTypes().FirstOrDefault(t => t.Name == "CollectionExtensions");
            if (collectionExtensions == null)
            {
                throw new InvalidOperationException("Could not find CollectionExtensions type in BepInEx.Unity.IL2CPP assembly");
            }

            MethodInfo? wrapToIl2CppMethod = collectionExtensions.GetMethods().FirstOrDefault(m => m.Name == "WrapToIl2Cpp" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(System.Collections.IEnumerator));
            if (wrapToIl2CppMethod == null)
            {
                throw new InvalidOperationException("Could not find WrapToIl2Cpp method in CollectionExtensions type");
            }

            return wrapToIl2CppMethod.Invoke(null, new object[] { enumerator });
        }
    }
}
