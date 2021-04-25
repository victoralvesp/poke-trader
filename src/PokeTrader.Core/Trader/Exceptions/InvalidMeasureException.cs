namespace PokeTrader.Core.Trader.Exceptions
{

    [System.Serializable]
    public class InvalidMeasureException : System.Exception
    {
        public InvalidMeasureException() { }
        public InvalidMeasureException(string message) : base(message) { }
        public InvalidMeasureException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidMeasureException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}