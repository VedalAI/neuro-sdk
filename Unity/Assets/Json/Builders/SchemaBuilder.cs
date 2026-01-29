using System.Collections.Generic;
using System.Linq;

namespace NeuroSdk.Json.Builders
{
    public class SchemaBuilder
    {
        protected readonly JsonSchema Schema = new();

        public JsonSchema Build() => Schema;

        public SchemaBuilder Const(object value)
        {
            Schema.Const = value;
            return this;
        }

        public SchemaBuilder Enum(IEnumerable<object> values)
        {
            Schema.Enum = values.ToList();
            return this;
        }
    }
}