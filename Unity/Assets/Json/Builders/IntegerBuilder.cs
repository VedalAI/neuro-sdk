namespace NeuroSdk.Json.Builders
{
    public sealed class IntegerBuilder : NumberBuilder<IntegerBuilder>
    {
        public IntegerBuilder()
        {
            Schema.Type = JsonSchemaType.Integer;
        }
    }
}