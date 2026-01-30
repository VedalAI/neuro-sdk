using System.Linq;

namespace NeuroSdk.Json.Builders
{
    public class PrimitiveBuilder<THolds> : SchemaBuilder
    {
        public PrimitiveBuilder<THolds> Const(THolds value)
        {
            Schema.Const = value;
            return this;
        }

        public PrimitiveBuilder<THolds> Enum(params THolds[] values)
        {
            Schema.Enum = values.Cast<object>().ToList();
            return this;
        }
    }
}