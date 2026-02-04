namespace NeuroSdk.Json.Builders
{
    public abstract class NumberBuilder<TSelf, THolds> : PrimitiveBuilder<TSelf, THolds>
        where TSelf : NumberBuilder<TSelf, THolds>
    {
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