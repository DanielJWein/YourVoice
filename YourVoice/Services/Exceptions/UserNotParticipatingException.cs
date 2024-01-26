namespace YourVoice.Services.Exceptions {
    [Serializable]
    public class UserNotParticipatingException : Exception {

        public UserNotParticipatingException( ) {
        }

        public UserNotParticipatingException( string message ) : base( message ) {
        }

        public UserNotParticipatingException( string message, Exception inner ) : base( message, inner ) {
        }

        protected UserNotParticipatingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
