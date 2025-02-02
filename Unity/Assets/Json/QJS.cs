#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuroSdk.Json
{
    /// <summary>
    /// Utility class for generating quick JSON schemas
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class QJS
    {
        private static JsonSchema Const<T>(T value)
        {
            return new JsonSchema
            {
                Const = value
            };
        }

        public static JsonSchema Const(string value) => Const<string>(value);
        public static JsonSchema Const(int value) => Const<int>(value);
        public static JsonSchema Const(IEnumerable<string> values) => Const<IEnumerable<string>>(values);
        public static JsonSchema Const(IEnumerable<int> values) => Const<IEnumerable<int>>(values);
        public static JsonSchema ConstEmptyArray => Const(Array.Empty<object>());
        public static JsonSchema ConstNull => Enum(new object?[] { null });

        private static JsonSchema Enum<T>(IEnumerable<T> values)
        {
            return new JsonSchema
            {
                Enum = values.Cast<object>().ToList()
            };
        }

        public static JsonSchema Enum(IEnumerable<string> values) => Enum<string>(values);
        public static JsonSchema Enum(IEnumerable<int> values) => Enum<int>(values);

        public static JsonSchema Type(JsonSchemaType type)
        {
            return new JsonSchema
            {
                Type = type
            };
        }
    }
}
