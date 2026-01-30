namespace NeuroSdk.Json.Builders
{
    public sealed class BooleanBuilder : PrimitiveBuilder<bool>
    {
        public BooleanBuilder()
        {
            Schema.Type = JsonSchemaType.Boolean;
        }
    }
}