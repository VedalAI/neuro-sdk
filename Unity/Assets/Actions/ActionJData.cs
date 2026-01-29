#nullable enable

using System;
using System.Collections.Generic;
using NeuroSdk.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NeuroSdk.Actions
{
    /// <summary>
    /// A wrapper class for the data of an <see cref="NeuroSdk.Messages.Incoming.Action"/> message.
    /// </summary>
    public sealed class ActionJData : IJTokenWrapper
    {
        public JToken? Data { get; private set; }

        private ActionJData()
        {
        }

        private void DeserializeFromJson(string? stringifiedData)
        {
            if (stringifiedData is null or "")
            {
                Data = null;
                return;
            }

            Data = JToken.Parse(stringifiedData);
        }

        internal static bool TryParse(string? stringifiedData, out ActionJData? actionJData)
        {
            try
            {
                actionJData = new ActionJData();
                actionJData.DeserializeFromJson(stringifiedData);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to deserialize ActionJData from string.");
                Debug.LogError(e.ToString());
                actionJData = null;
                return false;
            }
        }
        
        #region Utility

        public T? Get<T>(string path, T? defaultValue = default)
        {
            return TryGet(path, out T? value) ? value : defaultValue;
        }

        public bool TryGet<T>(string path, out T? value)
        {
            value = default;

            var token = Data?.SelectToken(path);
            if (token is null || token.Type == JTokenType.Null)
                return false;

            try
            {
                value = token.ToObject<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string? GetString(string path, string? defaultValue = null) => Get(path, defaultValue);
        public bool TryGetString(string path, out string? value) => TryGet(path, out value);

        public int GetInt(string path, int defaultValue = 0) => Get(path, defaultValue);
        public bool TryGetInt(string path, out int value)
        {
            value = 0;
            if (!TryGet(path, out int? val)) return false;
            
            value = val.GetValueOrDefault();
            return true;
        }

        public float GetFloat(string path, float defaultValue = 0f) => Get(path, defaultValue);
        public bool TryGetFloat(string path, out float value)
        {
            value = 0f;
            if (!TryGet(path, out float? val)) return false;
            
            value = val.GetValueOrDefault();
            return true;
        }

        public bool GetBool(string path, bool defaultValue = false) => Get(path, defaultValue);
        public bool TryGetBool(string path, out bool value)
        {
            value = false;
            if (!TryGet(path, out bool? val)) return false;
            
            value = val.GetValueOrDefault();
            return true;
        }

        public JObject? GetObject(string path) => Get<JObject>(path);
        public bool TryGetObject(string path, out JObject? obj)
        {
            obj = null;
            if (Data?.SelectToken(path) is not JObject token) return false;
            
            obj = token;
            return true;
        }
        
        public JArray? GetArray(string path) => Get<JArray>(path);
        public bool TryGetArray(string path, out JArray? array)
        {
            array = null;
            if (Data?.SelectToken(path) is not JArray token) return false;
            
            array = token;
            return true;
        }

        public bool Contains(string path) => Data?.SelectToken(path) != null;
        public bool HasValue(string path) => Data?.SelectToken(path) is { Type: not JTokenType.Null };
        public bool IsNull(string path) => Data?.SelectToken(path)?.Type == JTokenType.Null;

        public Dictionary<string, object>? ToDictionary() => Data?.ToObject<Dictionary<string, object>>();
        
        public JToken? DeepCopy() => Data?.DeepClone();
        public T? ToObject<T>()
        {
            if (Data == null) return default;

            try
            {
                return Data.ToObject<T>();
            }
            catch
            {
                return default;
            } 
        }

        #endregion
    }
}
