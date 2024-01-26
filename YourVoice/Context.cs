namespace YourVoice {

    /// <summary>
    /// Represents information around a command invocation
    /// </summary>
    internal class Context {

        /// <summary>
        /// The channel the message was sent in
        /// </summary>
        public IMessageChannel Channel;

        /// <summary>
        /// The context of the message
        /// </summary>
        public string Content;

        /// <summary>
        /// The guild the message was sent in (if applicable)
        /// </summary>
        public IGuild? Guild;

        /// <summary>
        /// The message this context was created from
        /// </summary>
        public IMessage Message;

        /// <summary>
        /// The user who sent the message
        /// </summary>
        public IUser User;

        /// <summary>
        /// Creates a new context
        /// </summary>
        /// <param name="msg"> The message to create the context from </param>
        public Context( SocketMessage msg ) {
            Content = msg.Content;
            Message = msg;
            User = msg.Author;
            Guild = (msg.Channel as SocketGuildChannel)?.Guild;
            Channel = msg.Channel;
        }
    }
}
