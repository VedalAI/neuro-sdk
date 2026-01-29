namespace NeuroSdk.Json.Builders
{
    public sealed class NullBuilder : SchemaBuilder
    {
        public NullBuilder()
        {
            Schema.Type = JsonSchemaType.Null;
            Schema.Const = null;
        }
    }
}