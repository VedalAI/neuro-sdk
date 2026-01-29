namespace NeuroSdk.Json.Builders
{
    public sealed class BooleanBuilder : SchemaBuilder<bool>
    {
        public BooleanBuilder()
        {
            Schema.Type = JsonSchemaType.Boolean;
        }
    }
}