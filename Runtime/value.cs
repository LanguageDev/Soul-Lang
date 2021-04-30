
namespace SoulLang
{
    public interface IValue
    {
        public string ToString();
    }

    public class IntegerValue : IValue
    {
        public int Value { get; set; }

        public IntegerValue(int value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();
    }

    public class StringValue : IValue
    {
        public string Value { get; set; }

        public StringValue(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;
    }

    public class BoolValue : IValue
    {
        public bool Value { get; set; }

        public BoolValue(bool value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();
    }
}
