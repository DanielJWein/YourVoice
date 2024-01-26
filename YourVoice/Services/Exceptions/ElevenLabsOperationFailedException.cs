namespace YourVoice.Services.Exceptions {

    [Serializable]
    public class ElevenLabsOperationFailedException : Exception {

        public ElevenLabsOperationFailedException( ) {
        }

        public ElevenLabsOperationFailedException( string message ) : base( message ) {
        }

        public ElevenLabsOperationFailedException( string message, Exception inner ) : base( message, inner ) {
        }

        protected ElevenLabsOperationFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
