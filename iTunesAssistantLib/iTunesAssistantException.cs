namespace iTunesAssistantLib
{
    [System.Serializable]
    public class iTunesAssistantException : System.Exception
    {
        public iTunesAssistantException() { }
        public iTunesAssistantException(string message) : base(message) { }
        public iTunesAssistantException(string message, System.Exception inner) : base(message, inner) { }
        protected iTunesAssistantException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
