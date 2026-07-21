using System;
using System.Collections.Generic;

namespace NeuroSdk.Json.Builders
{
    public sealed class ObjectBuilder : SchemaBuilder<ObjectBuilder>
    {
        public ObjectBuilder()
        {
            Schema.Type = JsonSchemaType.Object;
            Schema.Properties = new Dictionary<string, JsonSchema>();
        }

        public ObjectBuilder Property<T>(
            string name,
            Func<JsonSchemaBuilders, SchemaBuilder<T>> build) where T : SchemaBuilder<T>
        {
            var subBuilder = build(new JsonSchemaBuilders());
            Schema.Properties[name] = subBuilder.Build();

            if (!subBuilder.IsOptional) Schema.Required.Add(name);

            return this;
        }
    }
}