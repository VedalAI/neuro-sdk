namespace NeuroSdk.Json.Builders
{
    public sealed class NullBuilder : SchemaBuilder<object>
    {
        public NullBuilder()
        {
            Schema.Type = JsonSchemaType.Null;
            Schema.Const = null;
        }
    }
}