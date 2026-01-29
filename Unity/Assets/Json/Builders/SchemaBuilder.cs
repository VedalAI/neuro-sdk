using System.Collections.Generic;
using System.Linq;

namespace NeuroSdk.Json.Builders
{
    public class SchemaBuilder<THolds>
    {
        protected readonly JsonSchema Schema = new();

        public JsonSchema Build() => Schema;

        public SchemaBuilder<THolds> Const(object value)
        {
            Schema.Const = value;
            return this;
        }

        public SchemaBuilder<THolds> Enum(params THolds[] values)
        {
            Schema.Enum = values.Cast<object>().ToList();
            return this;
        }
    }
}