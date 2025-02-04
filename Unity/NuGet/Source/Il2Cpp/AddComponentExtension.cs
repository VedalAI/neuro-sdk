#nullable enable

using System;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace UnityEngine
{
    // ReSharper disable once UnusedType.Global
    public static class AddComponentExtension
    {
        private static readonly Lazy<MethodInfo?> _addComponentMethod = new(() => typeof(GameObject).GetMethods().FirstOrDefault(m => m.Name == nameof(GameObject.AddComponent) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.FullName == "Il2CppSystem.Type"));

        private static readonly Lazy<MethodInfo?> _il2CppTypeFromMethod = new(() => Type.GetType("Il2CppInterop.Runtime.Il2CppType, Il2CppInterop.Runtime")?.GetMethods().FirstOrDefault(m => m.Name == "From" && m.GetParameters().Length == 1));

        // Class methods get prioritized over extension methods, so this extension method will only be called when the corresponding class method is missing.
        // ReSharper disable once UnusedMember.Global
        public static Component AddComponent(this GameObject obj, Type type)
        {
            MethodInfo? addComponentMethod = _addComponentMethod.Value;
            if (addComponentMethod == null)
            {
                throw new MissingMethodException("Could not find method 'UnityEngine.GameObject:AddComponent(Il2CppSystem.Type)'. How did you even get here? This method should only be called in an Il2Cpp environment.");
            }

            return (Component) addComponentMethod.Invoke(obj, new[] { Il2CppTypeFrom(type) });
        }

        private static object Il2CppTypeFrom(Type type)
        {
            MethodInfo? il2CppTypeFromMethod = _il2CppTypeFromMethod.Value;
            if (il2CppTypeFromMethod == null)
            {
                throw new InvalidOperationException("Could not find method 'Il2CppInterop.Runtime.Il2CppType:From(System.Type)'");
            }

            return il2CppTypeFromMethod.Invoke(null, new object[] { type });
        }
    }
}
