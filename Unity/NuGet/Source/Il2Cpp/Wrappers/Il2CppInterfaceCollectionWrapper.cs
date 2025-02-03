#nullable enable

using System;
using System.Collections.Generic;

namespace NeuroSdk.Source.Il2Cpp.Wrappers
{
    public class Il2CppInterfaceCollectionWrapper
    {
        private static readonly Lazy<Type?> _class = new(() => Type.GetType("Il2CppInterop.Runtime.Injection.Il2CppInterfaceCollection, Il2CppInterop.Runtime"));

        public object Value { get; }

        public Il2CppInterfaceCollectionWrapper(IEnumerable<Type> interfaces)
        {
            Value = Activator.CreateInstance(_class.Value, interfaces);
        }
    }
}
