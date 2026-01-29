using System;

namespace NeuroSdk.Json.Builders
{
    public sealed class ArrayBuilder : SchemaBuilder
    {
        public ArrayBuilder()
        {
            Schema.Type = JsonSchemaType.Array;
        }

        public ArrayBuilder Items(Func<SchemaBuilder> build)
        {
            Schema.Items = build().Build();
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