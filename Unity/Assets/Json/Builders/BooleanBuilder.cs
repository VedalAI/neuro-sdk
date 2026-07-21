namespace NeuroSdk.Json.Builders
{
    public sealed class BooleanBuilder : PrimitiveBuilder<BooleanBuilder, bool>
    {
        public BooleanBuilder()
        {
            Schema.Type = JsonSchemaType.Boolean;
        }
    }
}