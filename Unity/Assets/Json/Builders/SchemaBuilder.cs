namespace NeuroSdk.Json.Builders
{
    public class SchemaBuilder<TSelf> where TSelf : SchemaBuilder<TSelf>
    {
        protected readonly JsonSchema Schema = new();
        protected TSelf Self => (TSelf)this;

        internal bool IsOptional { get; private set; }

        public TSelf Optional()
        {
            IsOptional = true;
            return Self;
        }

        public JsonSchema Build()
        {
            return Schema;
        }
    }
}