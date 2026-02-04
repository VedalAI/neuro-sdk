namespace NeuroSdk.Json.Builders
{
    public sealed class StringBuilder : PrimitiveBuilder<StringBuilder, string>
    {
        public StringBuilder()
        {
            Schema.Type = JsonSchemaType.String;
        }

        public StringBuilder MinLength(int value)
        {
            Schema.MinLength = value;
            return this;
        }

        public StringBuilder MaxLength(int value)
        {
            Schema.MaxLength = value;
            return this;
        }

        public StringBuilder Pattern(string regex)
        {
            Schema.Pattern = regex;
            return this;
        }
    }
}