namespace YourVoice.Services.Exceptions {
    [Serializable]
    public class NoApiInfoException : Exception {

        public NoApiInfoException( ) {
        }

        public NoApiInfoException( string message ) : base( message ) {
        }

        public NoApiInfoException( string message, Exception inner ) : base( message, inner ) {
        }

        protected NoApiInfoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
