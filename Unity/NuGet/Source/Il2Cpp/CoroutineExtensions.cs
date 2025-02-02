#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NeuroSdk.Il2Cpp;
using UnityEngine;

namespace NeuroSdk.Source.Il2Cpp
{
    // ReSharper disable once UnusedType.Global
    internal static class CoroutineExtensions
    {
        private static MethodInfo? StartIl2CppCoroutine => typeof(MonoBehaviour).GetMethods().FirstOrDefault(m => m.Name == nameof(MonoBehaviour.StartCoroutine) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.FullName == "Il2CppSystem.Collections.IEnumerator");

        private static IEnumerable<ICoroutineConverter> Converters => AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ICoroutineConverter).IsAssignableFrom(t))
            .Select(Activator.CreateInstance)
            .Cast<ICoroutineConverter>();

        // ReSharper disable once UnusedMember.Global
        public static Coroutine StartCoroutine(this MainThreadUtil mainThreadUtil, System.Collections.IEnumerator enumerator)
        {
            MethodInfo? startCoroutineMethod = StartIl2CppCoroutine;
            if (startCoroutineMethod == null)
            {
                throw new MissingMethodException("Could not find UnityEngine.MonoBehaviour.StartCoroutine(Il2CppSystem.Collections.IEnumerator) method. How did you even get here? This method should only be called in an Il2Cpp environment.");
            }

            Debug.Log("Searching for system->il2cpp coroutine converters...");
            foreach (ICoroutineConverter converter in Converters)
            {
                try
                {
                    if (!converter.CanBeUsed)
                    {
                        Debug.Log($"Coroutine converter {converter.GetType().Name} cannot be used, skipping.");
                        continue;
                    }

                    object? convertedCoroutine = converter.ConvertToIl2Cpp(enumerator);
                    if (convertedCoroutine == null)
                    {
                        Debug.LogError($"Coroutine converter {converter.GetType().Name} returned null.");
                        continue;
                    }

                    Debug.Log($"Using coroutine converter {converter.GetType().Name}");
                    return (Coroutine) startCoroutineMethod.Invoke(mainThreadUtil, new[] { convertedCoroutine });
                }
                catch (Exception e)
                {
                    Debug.LogError($"Coroutine converter {converter.GetType().Name} threw an exception: {e}");
                }
            }

            throw new Exception("No usable coroutine converter was found. If you are not using BepInEx, define your own ICoroutineConverter.");
        }
    }
}
