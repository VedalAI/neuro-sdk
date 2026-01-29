namespace NeuroSdk.Json.Builders
{
    public sealed class FloatBuilder : NumberBuilder<FloatBuilder>
    {
        public FloatBuilder()
        {
            Schema.Type = JsonSchemaType.Float;
        }
    }
}