using System.Linq;

namespace NeuroSdk.Json.Builders
{
    public class PrimitiveBuilder<TSelf, THolds> : SchemaBuilder<TSelf>
        where TSelf : PrimitiveBuilder<TSelf, THolds>
    {
        public TSelf Const(THolds value)
        {
            Schema.Const = value;
            return Self;
        }

        public TSelf Enum(params THolds[] values)
        {
            Schema.Enum = values.Cast<object>().ToList();
            return Self;
        }
    }
}