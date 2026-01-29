using System;
using System.Collections.Generic;

namespace NeuroSdk.Json.Builders
{
    public sealed class ObjectBuilder : SchemaBuilder
    {
        public ObjectBuilder()
        {
            Schema.Type = JsonSchemaType.Object;
            Schema.Properties = new Dictionary<string, JsonSchema>();
        }

        public ObjectBuilder Property(
            string name,
            Func<JsonSchemaBuilders, SchemaBuilder> build,
            bool required = true)
        {
            var subBuilder = build(JsonSchemaBuilders.Instance);
            Schema.Properties[name] = subBuilder.Build();

            if (required)
                Schema.Required.Add(name);

            return this;
        }

    }
}