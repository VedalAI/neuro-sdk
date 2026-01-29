using System;
using System.Collections.Generic;

namespace NeuroSdk.Json.Builders
{
    public sealed class ArrayBuilder : SchemaBuilder<IEnumerator<object>>
    {
        public ArrayBuilder()
        {
            Schema.Type = JsonSchemaType.Array;
        }

        public ArrayBuilder Items<TBuilder>(Func<JsonSchemaBuilders, SchemaBuilder<TBuilder>> build)
        {
            Schema.Items = build(JsonSchemaBuilders.Instance).Build();
            return this;
        }

        public ArrayBuilder MinItems(int value)
        {
            Schema.MinItems = value;
            return this;
        }

        public ArrayBuilder MaxItems(int value)
        {
            Schema.MaxItems = value;
            return this;
        }

        public ArrayBuilder UniqueItems(bool value = true)
        {
            Schema.UniqueItems = value;
            return this;
        }
    }
}