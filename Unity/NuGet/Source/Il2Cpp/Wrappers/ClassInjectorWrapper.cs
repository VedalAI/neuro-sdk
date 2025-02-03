#nullable enable

using System;
using System.Linq;

namespace NeuroSdk.Source.Il2Cpp.Wrappers
{
    internal static class ClassInjectorWrapper
    {
        private static readonly Lazy<Type?> _class = new(() => Type.GetType("Il2CppInterop.Runtime.Injection.ClassInjector, Il2CppInterop.Runtime"));

        public static bool IsTypeRegisteredInIl2Cpp(Type type)
        {
            return (bool) _class.Value.GetMethod("IsTypeRegisteredInIl2Cpp", new[] { typeof(Type) })!.Invoke(null, new object[] { type });
        }

        public static void RegisterTypeInIl2Cpp(Type type, Type[] interfaces)
        {
            _class.Value.GetMethods().First(m => m.Name == "RegisterTypeInIl2Cpp" && m.GetParameters().Length == 2)!.Invoke(null, new[] { type, new RegisterTypeOptionsWrapper { Interfaces = interfaces }.Value });
        }
    }
}
