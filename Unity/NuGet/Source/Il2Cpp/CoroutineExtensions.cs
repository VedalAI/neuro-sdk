#nullable enable

using System;
using System.Linq;
using System.Reflection;

// ReSharper disable CheckNamespace
namespace UnityEngine
{
    // ReSharper disable once UnusedType.Global
    public static class CoroutineExtensions
    {
        private static readonly Lazy<Type?> _collectionExtensionsType = new(() => Type.GetType("BepInEx.Unity.IL2CPP.Utils.Collections.CollectionExtensions, BepInEx.Unity.IL2CPP"));

        private static readonly Lazy<MethodInfo?> _startIl2CppCoroutine = new (() => typeof(MonoBehaviour).GetMethods().FirstOrDefault(m => m.Name == nameof(MonoBehaviour.StartCoroutine) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.FullName == "Il2CppSystem.Collections.IEnumerator"));

        // Class methods get prioritized over extension methods, so this extension method will only be called when the corresponding class method is missing.
        // ReSharper disable once UnusedMember.Global
        public static Coroutine StartCoroutine(this MonoBehaviour mainThreadUtil, System.Collections.IEnumerator enumerator)
        {
            MethodInfo? startCoroutineMethod = _startIl2CppCoroutine.Value;
            if (startCoroutineMethod == null)
            {
                throw new MissingMethodException("Could not find UnityEngine.MonoBehaviour.StartCoroutine(Il2CppSystem.Collections.IEnumerator) method. How did you even get here? This method should only be called in an Il2Cpp environment.");
            }

            return (Coroutine) startCoroutineMethod.Invoke(mainThreadUtil, new[] { enumerator.WrapToIl2Cpp() });
        }

        private static object WrapToIl2Cpp(this System.Collections.IEnumerator enumerator)
        {
            Type? collectionExtensions = _collectionExtensionsType.Value;
            if (collectionExtensions == null)
            {
                throw new InvalidOperationException("Could not find type 'BepInEx.Unity.IL2CPP.Utils.Collections.CollectionExtensions'");
            }

            MethodInfo? wrapToIl2CppMethod = collectionExtensions.GetMethods().FirstOrDefault(m => m.Name == "WrapToIl2Cpp" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(System.Collections.IEnumerator));
            if (wrapToIl2CppMethod == null)
            {
                throw new InvalidOperationException("Could not find 'Il2CppSystem.Collections.IEnumerator WrapToIl2Cpp(System.Collections.IEnumerator)' method in 'BepInEx.Unity.IL2CPP.CollectionExtensions' type");
            }

            return wrapToIl2CppMethod.Invoke(null, new object[] { enumerator });
        }
    }
}
