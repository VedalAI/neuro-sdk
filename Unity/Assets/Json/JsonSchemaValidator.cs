#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NeuroSdk.Actions;
using Newtonsoft.Json.Linq;

namespace NeuroSdk.Json
{
    public class JsonSchemaValidator
    {
        /// <summary>
        /// Validate an object (POCO, Dictionary, etc.) against a JsonSchema
        /// Returns false and the error message if invalid
        /// </summary>
        public static bool ValidateSafe(JsonSchema schema, ActionJData? obj, out string? message, string? path = "")
        {
            return ValidateSafe(schema, (object?)obj?.Data, out message, path);
        }
        
        /// <summary>
        /// Validate an object (POCO, Dictionary, etc.) against a JsonSchema
        /// Returns false and the error message if invalid
        /// </summary>
        public static bool ValidateSafe(JsonSchema schema, JToken? obj, out string? message, string? path = "")
        {
            return ValidateSafe(schema, (object?)obj, out message, path);
        }

        /// <summary>
        /// Validate an object (POCO, Dictionary, etc.) against a JsonSchema
        /// Returns false and the error message if invalid
        /// </summary>
        public static bool ValidateSafe(JsonSchema schema, object? obj, out string? message, string? path = "")
        {
            try
            {
                Validate(schema, obj);
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
            
            message = null;
            return true;
        }
        
        /// <summary>
        /// Validate an object (POCO, Dictionary, etc.) against a JsonSchema
        /// Throws an exception if invalid
        /// </summary>
        public static void Validate(JsonSchema schema, ActionJData? actionData, string path = "")
        {
            if (actionData == null)
            {
                throw new Exception($"{path}: expected action data");
            }
            
            var token = actionData.Data;
            
            if (token == null || token.Type == JTokenType.Null)
            {
                if (schema.Type == JsonSchemaType.Null || schema.Const == null) return;
                throw new Exception($"{path}: value is null but schema does not allow null");
            }

            object? obj = token.Type switch
            {
                JTokenType.Object => token.ToObject<Dictionary<string, object>>()!,
                JTokenType.Array => token.ToObject<List<object>>()!,
                JTokenType.Integer => token.Value<long>(),
                JTokenType.Float => token.Value<double>(),
                JTokenType.Boolean => token.Value<bool>(),
                JTokenType.String => token.Value<string>(),
                _ => throw new Exception($"{path}: unsupported token type {token.Type}")
            };

            Validate(schema, obj, path);
        }

        /// <summary>
        /// Validate an object (POCO, Dictionary, etc.) against a JsonSchema
        /// Throws an exception if invalid
        /// </summary>
        public static void Validate(JsonSchema schema, JToken? token, string path = "")
        {
            if (token == null || token.Type == JTokenType.Null)
            {
                if (schema.Type == JsonSchemaType.Null /* || schema.Const == null */) return;
                throw new Exception($"{path}: value is null but schema does not allow null");
            }

            object? obj = token.Type switch
            {
                JTokenType.Object => token.ToObject<Dictionary<string, object>>()!,
                JTokenType.Array => token.ToObject<List<object>>()!,
                JTokenType.Integer => token.Value<long>(),
                JTokenType.Float => token.Value<double>(),
                JTokenType.Boolean => token.Value<bool>(),
                JTokenType.String => token.Value<string>(),
                _ => throw new Exception($"{path}: unsupported token type {token.Type}")
            };

            Validate(schema, obj, path);
        }

        /// <summary>
        /// Validate an object (POCO, Dictionary, etc.) against a JsonSchema
        /// Throws an exception if invalid
        /// </summary>
        public static void Validate(JsonSchema schema, object? obj, string path = "")
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema));
            if (obj == null)
            {
                // How do I check if the schema.Const is explicitly null?
                // Welp, I just won't check for that I guess.
                if (schema.Type == JsonSchemaType.Null /* || schema.Const == null */) return;
                throw new Exception($"{path}: value is null but schema does not allow null");
            }

            switch (schema.Type)
            {
                case JsonSchemaType.String:
                    ValidateString(schema, obj, path);
                    break;
                case JsonSchemaType.Float:
                    ValidateFloat(schema, obj, path);
                    break;
                case JsonSchemaType.Integer:
                    ValidateInteger(schema, obj, path);
                    break;
                case JsonSchemaType.Object:
                    ValidateObject(schema, obj, path);
                    break;
                case JsonSchemaType.Array:
                    ValidateArray(schema, obj, path);
                    break;
                case JsonSchemaType.Boolean:
                    ValidateBoolean(obj, path);
                    break;
                case JsonSchemaType.Null:
                    ValidateNull(obj, path);
                    break;
                case JsonSchemaType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            if (schema.Const != null && !schema.Const.Equals(obj))
                throw new Exception($"{path}: value must be constant {schema.Const}");
            
            if (schema.Enum != null && schema.Enum.Count > 0 && !schema.Enum.Contains(obj))
                throw new Exception($"{path}: value must be one of [{string.Join(", ", schema.Enum)}]");
        }

        private static void ValidateString(JsonSchema schema, object obj, string path)
        {
            if (obj is not string s)
                throw new Exception($"{path}: expected string");

            if (schema.MinLength.HasValue && s.Length < schema.MinLength.Value)
                throw new Exception($"{path}: string too short (min {schema.MinLength.Value})");

            if (schema.MaxLength.HasValue && s.Length > schema.MaxLength.Value)
                throw new Exception($"{path}: string too long (max {schema.MaxLength.Value})");

            if (string.IsNullOrEmpty(schema.Pattern)) return;
            
            if (schema.Pattern != null && !System.Text.RegularExpressions.Regex.IsMatch(s, schema.Pattern))
                throw new Exception($"{path}: string does not match pattern {schema.Pattern}");
        }

        private static void ValidateFloat(JsonSchema schema, object obj, string path)
        {
            switch (obj)
            {
                case float f:
                    ValidateNumber(schema, f, path);
                    break;
                case double d:
                    ValidateNumber(schema, d, path);
                    break;
                case int i:
                    ValidateNumber(schema, i, path);
                    break;
                default:
                    throw new Exception($"{path}: expected float");
            }
        }

        private static void ValidateInteger(JsonSchema schema, object obj, string path)
        {
            switch (obj)
            {
                case int i:
                    ValidateNumber(schema, i, path);
                    break;
                case long l:
                    ValidateNumber(schema, l, path);
                    break;
                default:
                    throw new Exception($"{path}: expected integer");
            }
        }

        private static void ValidateNumber(JsonSchema schema, double value, string path)
        {
            if (schema.Minimum.HasValue && value < schema.Minimum.Value)
                throw new Exception($"{path}: value {value} < minimum {schema.Minimum.Value}");
            if (schema.Maximum.HasValue && value > schema.Maximum.Value)
                throw new Exception($"{path}: value {value} > maximum {schema.Maximum.Value}");
            if (schema.ExclusiveMinimum.HasValue && value <= schema.ExclusiveMinimum.Value)
                throw new Exception($"{path}: value {value} <= exclusive minimum {schema.ExclusiveMinimum.Value}");
            if (schema.ExclusiveMaximum.HasValue && value >= schema.ExclusiveMaximum.Value)
                throw new Exception($"{path}: value {value} >= exclusive maximum {schema.ExclusiveMaximum.Value}");
        }
        
        private static void ValidateObject(JsonSchema schema, object obj, string path)
        {
            
            if (obj is not IDictionary<string, object> dict)
            {
                if (obj is JObject jObj)
                    dict = jObj.ToObject<Dictionary<string, object>>()!;
                else
                    throw new Exception($"{path}: expected object");
            }

            foreach (var req in schema.Required.Where(req => !dict.ContainsKey(req)))
                throw new Exception($"{MakePath(path, req)}: missing required property");

            foreach (var kvp in dict)
            {
                if (!schema.Properties.TryGetValue(kvp.Key, out var subSchema))
                {
                    if (!schema.AdditionalProperties)
                        throw new Exception($"{MakePath(path, kvp.Key)}: unknown property not allowed");
                }
                else
                {
                    Validate(subSchema, kvp.Value, $"{MakePath(path, kvp.Key)}");
                }
            }

            return;

            string MakePath(string parentPath, string key)
            {
                if (string.IsNullOrEmpty(parentPath)) 
                    return key;
                return parentPath + "." + key;
            }
        }

        private static void ValidateArray(JsonSchema schema, object obj, string path)
        {
            if (obj is not IEnumerable enumerable)
                throw new Exception($"{path}: expected array");

            var list = enumerable.Cast<object>().ToList();

            if (schema.MinItems.HasValue && list.Count < schema.MinItems.Value)
                throw new Exception(
                    $"{path}: array item count must be at least {schema.MinItems.Value}"
                );

            if (schema.MaxItems.HasValue && list.Count > schema.MaxItems.Value)
                throw new Exception(
                    $"{path}: array item count must be at most {schema.MaxItems.Value}"
                );

            if (schema.UniqueItems == true && list.Distinct().Count() != list.Count)
                throw new Exception(
                    $"{path}: array items must be unique"
                );


            if (schema.Items == null) return;
            
            for (var i = 0; i < list.Count; i++)
                Validate(schema.Items, list[i], $"{path}[{i}]");
        }

        private static void ValidateBoolean(object obj, string path)
        {
            if (obj is not bool) 
                throw new Exception($"{path}: expected boolean, got {obj.GetType().Name}");
        }
        
        private static void ValidateNull(object obj, string path)
        {
            if (obj is not null) throw new Exception($"{path}: expected null");
        }
    }
}