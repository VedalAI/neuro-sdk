namespace NeuroSdk.Json.Builders
{
    public sealed class IntegerBuilder : NumberBuilder<IntegerBuilder, int>
    {
        public IntegerBuilder()
        {
            Schema.Type = JsonSchemaType.Integer;
        }
    }
}