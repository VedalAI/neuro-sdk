using System;

namespace NeuroSdk.Source.Il2Cpp.Wrappers
{
    public class RegisterTypeOptionsWrapper
    {
        private static readonly Lazy<Type> _class = new(() => Type.GetType("Il2CppInterop.Runtime.Injection.RegisterTypeOptions, Il2CppInterop.Runtime"));

        public object Value { get; } = Activator.CreateInstance(_class.Value);

        public Type[] Interfaces
        {
            set => _class.Value.GetProperty("Interfaces")?.SetValue(Value, new Il2CppInterfaceCollectionWrapper(value).Value);
        }
    }
}
