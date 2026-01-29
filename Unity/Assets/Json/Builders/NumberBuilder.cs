namespace NeuroSdk.Json.Builders
{
    public abstract class NumberBuilder<TSelf> : SchemaBuilder
        where TSelf : NumberBuilder<TSelf>
    {
        protected TSelf Self => (TSelf)this;

        public TSelf Min(double value)
        {
            Schema.Minimum = (float)value;
            return Self;
        }

        public TSelf Max(double value)
        {
            Schema.Maximum = (float)value;
            return Self;
        }

        public TSelf ExclusiveMin(double value)
        {
            Schema.ExclusiveMinimum = (float)value;
            return Self;
        }

        public TSelf ExclusiveMax(double value)
        {
            Schema.ExclusiveMaximum = (float)value;
            return Self;
        }
    }
}