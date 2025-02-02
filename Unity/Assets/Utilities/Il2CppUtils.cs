#nullable enable

using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace NeuroSdk.Utilities
{
    internal static class Il2CppUtils
    {
        private static readonly MethodInfo _addComponentMethod = typeof(GameObject).GetMethods()
            .First(m => m.Name == nameof(GameObject.AddComponent) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.Name == "Type");

        private static readonly Lazy<MethodInfo?> _il2cppTypeFromMethod = new(() =>
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.FullName == "Il2CppInterop.Runtime.Il2CppType")?.GetMethods().FirstOrDefault(m => m.Name == "From" && m.GetParameters().Length == 1);
        });

        public static T AddTypedComponent<T>(GameObject obj, Type type)
        {
            switch (_addComponentMethod.GetParameters()[0].ParameterType.FullName)
            {
                case "System.Type":
                    return (T) _addComponentMethod.Invoke(obj, new object[] { type });
                case "Il2CppSystem.Type":
                    MethodInfo? il2cppTypeFromMethod = _il2cppTypeFromMethod.Value;
                    if (il2cppTypeFromMethod == null)
                    {
                        throw new InvalidOperationException("Could not find method 'Il2CppInterop.Runtime.Il2CppType.From(System.Type)'");
                    }

                    return (T) _addComponentMethod.Invoke(obj, new object[] { il2cppTypeFromMethod.Invoke(null, new object[] { type }) });
                default:
                    throw new InvalidOperationException("Could not find method 'GameObject.AddComponent(Type)'");
            }
        }
    }
}
