// Taken from https://github.com/NuclearPowered/Reactor/blob/master/Reactor/Utilities/Attributes/RegisterInIl2CppAttribute.cs

using System;
using System.Collections.Generic;
using System.Reflection;
using NeuroSdk.Source.Il2Cpp.Wrappers;
using UnityEngine;

namespace Reactor.Utilities.Attributes
{
    /// <summary>
    /// Automatically registers an il2cpp type using <see cref="ClassInjector.RegisterTypeInIl2Cpp{T}()"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class RegisterInIl2CppAttribute : Attribute
    {
        private static readonly HashSet<Assembly> _registeredAssemblies = new();

        /// <summary>
        /// Gets il2cpp interfaces to be injected with this type.
        /// </summary>
        private Type[] Interfaces { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterInIl2CppAttribute"/> class without any interfaces.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public RegisterInIl2CppAttribute()
        {
            Interfaces = Type.EmptyTypes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterInIl2CppAttribute"/> class with interfaces.
        /// </summary>
        /// <param name="interfaces">Il2Cpp interfaces to be injected with this type.</param>
        // ReSharper disable once UnusedMember.Global
        public RegisterInIl2CppAttribute(params Type[] interfaces)
        {
            Interfaces = interfaces;
        }

        private static void RegisterType(Type type, Type[] interfaces)
        {
            RegisterInIl2CppAttribute baseTypeAttribute = type.BaseType?.GetCustomAttribute<RegisterInIl2CppAttribute>();
            if (baseTypeAttribute != null)
            {
                RegisterType(type.BaseType!, baseTypeAttribute.Interfaces);
            }

            if (ClassInjectorWrapper.IsTypeRegisteredInIl2Cpp(type))
            {
                return;
            }

            try
            {
                ClassInjectorWrapper.RegisterTypeInIl2Cpp(type, interfaces);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to register {type.FullName}: {e}");
            }
        }

        /// <summary>
        /// Registers all Il2Cpp types annotated with <see cref="RegisterInIl2CppAttribute"/> in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <remarks>This is called automatically on plugin assemblies so you probably don't need to call this.</remarks>
        /// <param name="assembly">The assembly to search.</param>
        public static void Register(Assembly assembly)
        {
            if (!_registeredAssemblies.Add(assembly)) return;

            foreach (Type type in assembly.GetTypes())
            {
                RegisterInIl2CppAttribute attribute = type.GetCustomAttribute<RegisterInIl2CppAttribute>();
                if (attribute != null)
                {
                    RegisterType(type, attribute.Interfaces);
                }
            }
        }
    }
}
