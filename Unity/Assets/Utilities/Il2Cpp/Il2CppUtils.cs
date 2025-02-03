#nullable enable

using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NeuroSdk.Utilities.Il2Cpp
{
    internal static class Il2CppUtils
    {
        private static readonly Lazy<MethodInfo> _addComponentMethod = new(() => typeof(GameObject).GetMethods()
            .First(m => m.Name == nameof(GameObject.AddComponent) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.Name == "Type"));

        private static readonly Lazy<MethodInfo?> _iltocppTypeFromMethod = new(() =>
        {
            return Type.GetType("Il2CppInterop.Runtime.Il2CppType, Il2CppInterop.Runtime")?.GetMethods().FirstOrDefault(m => m.Name == "From" && m.GetParameters().Length == 1);
        });

        public static T AddTypedComponent<T>(GameObject obj, Type type)
        {
            switch (_addComponentMethod.Value.GetParameters()[0].ParameterType.FullName)
            {
                case "System.Type":
                    return (T) _addComponentMethod.Value.Invoke(obj, new object[] { type });
                case "Il2CppSystem.Type":
                    MethodInfo? iltocppTypeFromMethod = _iltocppTypeFromMethod.Value;
                    if (iltocppTypeFromMethod == null)
                    {
                        throw new InvalidOperationException("Could not find method 'Il2CppInterop.Runtime.Il2CppType.From(System.Type)'");
                    }

                    return (T) _addComponentMethod.Value.Invoke(obj, new object[] { iltocppTypeFromMethod.Invoke(null, new object[] { type }) });
                default:
                    throw new InvalidOperationException("Could not find method 'GameObject.AddComponent(Type)'");
            }
        }
    }
}
