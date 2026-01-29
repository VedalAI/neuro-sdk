namespace NeuroSdk.Json.Builders
{
    public interface IRootSchemaBuilders
    {
        ObjectBuilder Object();
        ArrayBuilder Array();
    }
    
    public sealed class JsonSchemaBuilders : IRootSchemaBuilders
    {
        // singleton schizophrenia xd
        public static IRootSchemaBuilders Instance { get; } = new JsonSchemaBuilders();

        internal JsonSchemaBuilders() {}

        public ObjectBuilder Object() => new();
        public ArrayBuilder Array() => new();
        public StringBuilder String() => new();
        public IntegerBuilder Integer() => new();
        public FloatBuilder Float() => new();
        public BooleanBuilder Boolean() => new();
        public NullBuilder Null() => new();
    }

}