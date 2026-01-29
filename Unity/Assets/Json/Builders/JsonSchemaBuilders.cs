namespace NeuroSdk.Json.Builders
{
    public sealed class JsonSchemaBuilders
    {
        public static JsonSchemaBuilders Instance { get; } = new();

        private JsonSchemaBuilders() {}

        public ObjectBuilder Object() => new();
        public ArrayBuilder Array() => new();
        public StringBuilder String() => new();
        public IntegerBuilder Integer() => new();
        public FloatBuilder Float() => new();
        public BooleanBuilder Boolean() => new();
        public NullBuilder Null() => new();
    }

}