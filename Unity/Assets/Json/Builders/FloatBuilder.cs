namespace NeuroSdk.Json.Builders
{
    public sealed class FloatBuilder : NumberBuilder<FloatBuilder, float>
    {
        public FloatBuilder()
        {
            Schema.Type = JsonSchemaType.Float;
        }
    }
}