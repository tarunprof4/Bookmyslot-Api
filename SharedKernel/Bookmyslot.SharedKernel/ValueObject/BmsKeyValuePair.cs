namespace Bookmyslot.SharedKernel.ValueObject
{
    public class BmsKeyValuePair<TKey, TValue> : BaseValueObject
    {
        public BmsKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public TKey Key { get; }

        public TValue Value { get; }

    }

}
