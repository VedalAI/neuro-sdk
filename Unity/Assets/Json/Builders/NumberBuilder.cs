namespace NeuroSdk.Json.Builders
{
    public abstract class NumberBuilder<TSelf> : SchemaBuilder<TSelf>
        where TSelf : NumberBuilder<TSelf>
    {
        protected TSelf Self => (TSelf)this;

        public TSelf Min(float value)
        {
            Schema.Minimum = value;
            return Self;
        }

        public TSelf Max(float value)
        {
            Schema.Maximum = value;
            return Self;
        }

        public TSelf ExclusiveMin(float value)
        {
            Schema.ExclusiveMinimum = value;
            return Self;
        }

        public TSelf ExclusiveMax(float value)
        {
            Schema.ExclusiveMaximum = value;
            return Self;
        }
    }
}
