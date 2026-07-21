using System;

namespace NeuroSdk.Json.Builders
{
    public interface IRootSchemaBuilders
    {
        ObjectBuilder Object();
        ArrayBuilder<T> Array<T>(Func<JsonSchemaBuilders, SchemaBuilder<T>> build) where T : SchemaBuilder<T>;
    }

    public sealed class JsonSchemaBuilders : IRootSchemaBuilders
    {
        internal JsonSchemaBuilders()
        {
        }

        // singleton schizophrenia xd
        public static IRootSchemaBuilders Instance { get; } = new JsonSchemaBuilders();

        public ObjectBuilder Object()
        {
            return new ObjectBuilder();
        }

        public ArrayBuilder<T> Array<T>(Func<JsonSchemaBuilders, SchemaBuilder<T>> build)
            where T : SchemaBuilder<T>
        {
            return new ArrayBuilder<T>(build);
        }

        public StringBuilder String()
        {
            return new StringBuilder();
        }

        public IntegerBuilder Integer()
        {
            return new IntegerBuilder();
        }

        public FloatBuilder Float()
        {
            return new FloatBuilder();
        }

        public BooleanBuilder Boolean()
        {
            return new BooleanBuilder();
        }

        public NullBuilder Null()
        {
            return new NullBuilder();
        }
    }
}