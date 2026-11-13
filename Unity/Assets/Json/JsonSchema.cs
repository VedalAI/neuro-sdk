#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NeuroSdk.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NeuroSdk.Json
{
    public class JsonSchema
    {
        [JsonIgnore]
        public Dictionary<string, JsonSchema> Properties
        {
            get => _properties ??= new Dictionary<string, JsonSchema>();
            set => _properties = value;
        }

        [JsonIgnore]
        public JsonSchemaType Type
        {
            get
            {
                return _type switch
                {
                    "string" => JsonSchemaType.String,
                    "number" => JsonSchemaType.Float,
                    "integer" => JsonSchemaType.Integer,
                    "boolean" => JsonSchemaType.Boolean,
                    "object" => JsonSchemaType.Object,
                    "array" => JsonSchemaType.Array,
                    "null" => JsonSchemaType.Null,
                    _ => JsonSchemaType.None
                };
            }
            set
            {
                _type = value switch
                {
                    JsonSchemaType.String => "string",
                    JsonSchemaType.Float => "number",
                    JsonSchemaType.Integer => "integer",
                    JsonSchemaType.Boolean => "boolean",
                    JsonSchemaType.Object => "object",
                    JsonSchemaType.Array => "array",
                    JsonSchemaType.Null => "null",
                    _ => null
                };
            }
        }

        [JsonIgnore]
        public List<object> Enum
        {
            get => _enum ??= new List<object>();
            set => _enum = value;
        }

        [JsonIgnore]
        public List<string> Required
        {
            get => _required ??= new List<string>();
            set => _required = value;
        }

        internal sealed class ConstNull : JsonSchema
        {
            [JsonProperty("const", NullValueHandling = NullValueHandling.Include)]
            public override object? Const { get; set; }
        }

        #region Validation

        /// <summary>
        ///     Validate an ActionJData object against a JsonSchema
        ///     Returns false and the error message if invalid
        /// </summary>
        public bool ValidateSafe(ActionJData? obj, out string? message)
        {
            try
            {
                Validate(obj);
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
        ///     Validate a JToken against a JsonSchema
        ///     Returns false and the error message if invalid
        /// </summary>
        public bool ValidateSafe(JToken? obj, out string? message)
        {
            try
            {
                Validate(obj);
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
        ///     Validate an object (POCO, Dictionary, etc.) against a JsonSchema
        ///     Returns false and the error message if invalid
        /// </summary>
        public bool ValidateSafe(object? obj, out string? message)
        {
            try
            {
                Validate(obj);
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
        ///     Validate an ActionJData object against a JsonSchema
        ///     Throws an exception if invalid
        /// </summary>
        public void Validate(ActionJData? actionJData)
        {
            Validate(actionJData, "");
        }

        private void Validate(ActionJData? actionData, string path)
        {
            if (actionData == null) throw new Exception($"{path}: expected action data");

            var token = actionData.Data;

            Validate(token, path);
        }

        /// <summary>
        ///     Validate a JToken against a JsonSchema
        ///     Throws an exception if invalid
        /// </summary>
        public void Validate(JToken? token)
        {
            Validate(token, "");
        }

        private void Validate(JToken? token, string path)
        {
            if (token == null || token.Type == JTokenType.Null)
            {
                if (Type == JsonSchemaType.Null /* || schema.Const == null */) return;
                throw new Exception($"{path}: value is null but schema does not allow null");
            }

            var obj = UnwrapToken(token);

            Validate(obj, path);
        }

        /// <summary>
        ///     Validate an object (POCO, Dictionary, etc.) against a JsonSchema
        ///     Throws an exception if invalid
        /// </summary>
        public void Validate(object? obj)
        {
            Validate(obj, "");
        }

        private void Validate(object? obj, string path)
        {
            if (obj == null)
            {
                // How do I check if the schema.Const is explicitly null?
                // Welp, I just won't check for that I guess.
                if (Type == JsonSchemaType.Null /* || schema.Const == null */) return;
                throw new Exception($"{path}: value is null but schema does not allow null");
            }

            if (Const != null && !Const.Equals(obj))
                throw new Exception($"{path}: value must be constant {Const}");

            if (Enum is { Count: > 0 } && !Enum.Contains(obj))
                throw new Exception($"{path}: value must be one of [{string.Join(", ", Enum)}]");


            switch (Type)
            {
                case JsonSchemaType.String:
                    ValidateString(obj, path);
                    break;
                case JsonSchemaType.Float:
                    ValidateFloat(obj, path);
                    break;
                case JsonSchemaType.Integer:
                    ValidateInteger(obj, path);
                    break;
                case JsonSchemaType.Object:
                    ValidateObject(obj, path);
                    break;
                case JsonSchemaType.Array:
                    ValidateArray(obj, path);
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
        }

        private void ValidateString(object obj, string path)
        {
            if (obj is not string s)
                throw new Exception($"{path}: expected string");

            if (MinLength.HasValue && s.Length < MinLength.Value)
                throw new Exception($"{path}: string too short (min {MinLength.Value})");

            if (MaxLength.HasValue && s.Length > MaxLength.Value)
                throw new Exception($"{path}: string too long (max {MaxLength.Value})");

            if (string.IsNullOrEmpty(Pattern)) return;

            if (Pattern != null && !Regex.IsMatch(s, Pattern))
                throw new Exception($"{path}: string does not match pattern {Pattern}");
        }

        private void ValidateFloat(object obj, string path)
        {
            switch (obj)
            {
                case float f:
                    ValidateNumber(f, path);
                    break;
                case double d:
                    ValidateNumber(d, path);
                    break;
                case int i:
                    ValidateNumber(i, path);
                    break;
                default:
                    throw new Exception($"{path}: expected float");
            }
        }

        private void ValidateInteger(object obj, string path)
        {
            switch (obj)
            {
                case int i:
                    ValidateNumber(i, path);
                    break;
                case long l:
                    ValidateNumber(l, path);
                    break;
                default:
                    throw new Exception($"{path}: expected integer");
            }
        }

        private void ValidateNumber(double value, string path)
        {
            if (value < Minimum)
                throw new Exception($"{path}: value {value} < minimum {Minimum.Value}");
            if (value > Maximum)
                throw new Exception($"{path}: value {value} > maximum {Maximum.Value}");
            if (value <= ExclusiveMinimum)
                throw new Exception($"{path}: value {value} <= exclusive minimum {ExclusiveMinimum.Value}");
            if (value >= ExclusiveMaximum)
                throw new Exception($"{path}: value {value} >= exclusive maximum {ExclusiveMaximum.Value}");
        }

        private void ValidateObject(object obj, string path)
        {
            if (obj is not IDictionary<string, object?> dict)
                throw new Exception($"{path}: expected object");


            foreach (var req in Required.Where(req => !dict.ContainsKey(req)))
                throw new Exception($"{MakePath(path, req)}: missing required property");

            foreach (var kvp in dict)
                if (!Properties.TryGetValue(kvp.Key, out var subSchema))
                {
                    if (!AdditionalProperties)
                        throw new Exception($"{MakePath(path, kvp.Key)}: unknown property not allowed");
                }
                else
                {
                    subSchema.Validate(kvp.Value, $"{MakePath(path, kvp.Key)}");
                }

            return;

            string MakePath(string parentPath, string key)
            {
                if (string.IsNullOrEmpty(parentPath))
                    return key;
                return parentPath + "." + key;
            }
        }

        private void ValidateArray(object obj, string path)
        {
            if (obj is not IEnumerable enumerable)
                throw new Exception($"{path}: expected array");

            var list = enumerable.Cast<object>().ToList();

            if (MinItems.HasValue && list.Count < MinItems.Value)
                throw new Exception(
                    $"{path}: array item count must be at least {MinItems.Value}"
                );

            if (MaxItems.HasValue && list.Count > MaxItems.Value)
                throw new Exception(
                    $"{path}: array item count must be at most {MaxItems.Value}"
                );

            if (UniqueItems == true && list.Distinct().Count() != list.Count)
                throw new Exception(
                    $"{path}: array items must be unique"
                );


            if (Items == null) return;

            for (var i = 0; i < list.Count; i++)
                Items.Validate(list[i], $"{path}[{i}]");
        }

        private void ValidateBoolean(object obj, string path)
        {
            if (obj is not bool)
                throw new Exception($"{path}: expected boolean, got {obj.GetType().Name}");
        }

        private void ValidateNull(object obj, string path)
        {
            if (obj is not null) throw new Exception($"{path}: expected null");
        }

        private static object? UnwrapToken(JToken? token)
        {
            if (token == null || token.Type == JTokenType.Null)
                return null;

            return token.Type switch
            {
                JTokenType.Object => UnwrapJObject((JObject)token),
                JTokenType.Array => UnwrapJArray((JArray)token),
                JTokenType.Integer => token.Value<long>(),
                JTokenType.Float => token.Value<double>(),
                JTokenType.Boolean => token.Value<bool>(),
                JTokenType.String => token.Value<string>(),
                _ => throw new Exception($"Unsupported token type {token.Type}")
            };
        }

        private static Dictionary<string, object?> UnwrapJObject(JObject jObj)
        {
            return jObj.Properties()
                .ToDictionary(
                    p => p.Name,
                    p => UnwrapToken(p.Value)
                );
        }

        private static List<object?> UnwrapJArray(JArray jArr)
        {
            return jArr.Select(UnwrapToken).ToList();
        }

        #endregion

        #region Keywords

        [JsonProperty("properties")] private Dictionary<string, JsonSchema>? _properties;

        [JsonProperty("items")] public JsonSchema? Items { get; set; }

        [JsonProperty("type")] private string? _type;

        [JsonProperty("enum")] private List<object>? _enum;

        [JsonProperty("const")] public virtual object? Const { get; set; }

        [JsonProperty("minLength")] public int? MinLength { get; set; }

        [JsonProperty("pattern")] public string? Pattern { get; set; }

        [JsonProperty("maxLength")] public int? MaxLength { get; set; }

        [JsonProperty("maximum")] public float? Maximum { get; set; }

        [JsonProperty("exclusiveMinimum")] public float? ExclusiveMinimum { get; set; }

        [JsonProperty("exclusiveMaximum")] public float? ExclusiveMaximum { get; set; }

        [JsonProperty("minimum")] public float? Minimum { get; set; }

        [JsonProperty("required")] private List<string>? _required;

        [JsonProperty("minItems")] public int? MinItems { get; set; }

        [JsonProperty("maxItems")] public int? MaxItems { get; set; }

        [JsonProperty("uniqueItems")] public bool? UniqueItems { get; set; }

        [JsonProperty("format")] public string? Format { get; set; }

        [JsonProperty("additionalProperties")] public bool AdditionalProperties { get; set; }

        #endregion
    }
}