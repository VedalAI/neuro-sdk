using System;
using System.Collections.Generic;

namespace NeuroSdk.Json.Builders
{
    public sealed class ArrayBuilder : SchemaBuilder
    {
        public ArrayBuilder(Func<JsonSchemaBuilders, SchemaBuilder> build)
        {
            Schema.Type = JsonSchemaType.Array;
            Schema.Items = build(new JsonSchemaBuilders()).Build();
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