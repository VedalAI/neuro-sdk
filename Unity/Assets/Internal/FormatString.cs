#nullable enable

namespace NeuroSdk.Internal
{
    internal sealed class FormatString
    {
        private readonly string _str;

        private FormatString(string str)
        {
            _str = str;
        }

        public string Format(params object[] args)
        {
            return string.Format(_str, args);
        }

        public static implicit operator FormatString(string str)
        {
            return new FormatString(str);
        }
    }
}
