namespace NeuroSdk.Json.Builders
{
    public class SchemaBuilder
    {
        protected readonly JsonSchema Schema = new();

        public JsonSchema Build() => Schema;
    }
}