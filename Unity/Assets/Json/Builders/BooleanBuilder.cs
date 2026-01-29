namespace NeuroSdk.Json.Builders
{
    public sealed class BooleanBuilder : SchemaBuilder
    {
        public BooleanBuilder()
        {
            Schema.Type = JsonSchemaType.Boolean;
        }
    }
}