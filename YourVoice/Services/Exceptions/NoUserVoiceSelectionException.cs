namespace YourVoice.Services.Exceptions {
    [Serializable]
    public class NoUserVoiceSelectionException : Exception {

        public NoUserVoiceSelectionException( ) {
        }

        public NoUserVoiceSelectionException( string message ) : base( message ) {
        }

        public NoUserVoiceSelectionException( string message, Exception inner ) : base( message, inner ) {
        }

        protected NoUserVoiceSelectionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
