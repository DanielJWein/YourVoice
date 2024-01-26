namespace YourVoice.Services.Exceptions {
    [Serializable]
    public class NoSuchVoiceException : Exception {

        public NoSuchVoiceException( ) {
        }

        public NoSuchVoiceException( string message ) : base( message ) {
        }

        public NoSuchVoiceException( string message, Exception inner ) : base( message, inner ) {
        }

        protected NoSuchVoiceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
