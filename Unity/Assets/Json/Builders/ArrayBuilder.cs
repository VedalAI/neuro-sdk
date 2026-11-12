using System;

namespace NeuroSdk.Json.Builders
{
    public sealed class ArrayBuilder<TItems> : SchemaBuilder<ArrayBuilder<TItems>>
        where TItems : SchemaBuilder<TItems>
    {
        public ArrayBuilder(Func<JsonSchemaBuilders, SchemaBuilder<TItems>> build)
        {
            Schema.Type = JsonSchemaType.Array;
            Schema.Items = build(new JsonSchemaBuilders()).Build();
        }

        public ArrayBuilder<TItems> MinItems(int value)
        {
            Schema.MinItems = value;
            return this;
        }

        public ArrayBuilder<TItems> MaxItems(int value)
        {
            Schema.MaxItems = value;
            return this;
        }

        public ArrayBuilder<TItems> UniqueItems(bool value = true)
        {
            Schema.UniqueItems = value;
            return this;
        }
    }
}