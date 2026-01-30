namespace NeuroSdk.Json.Builders
{
    public sealed class NullBuilder : PrimitiveBuilder<object>
    {
        public NullBuilder()
        {
            Schema.Type = JsonSchemaType.Null;
            Schema.Const = null;
        }
    }
}