namespace NeuroSdk.Json.Builders
{
    public sealed class NullBuilder : PrimitiveBuilder<NullBuilder, object>
    {
        public NullBuilder()
        {
            Schema.Type = JsonSchemaType.Null;
            Schema.Const = null;
        }
    }
}