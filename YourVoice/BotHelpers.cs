internal static class BotHelpers {

    /// <summary>
    /// Sends a failure embed
    /// </summary>
    /// <returns> </returns>
    internal static async Task SendFailEmbed( SocketMessage arg, string message ) {
        //Create a new EmbedBuilder
        EmbedBuilder embedBuilder = new EmbedBuilder( );

        //Set the title and color of the builder
        embedBuilder.WithTitle( "Speak Message" );
        embedBuilder.WithColor( Color.Red );

        //Assign the description to the embed
        embedBuilder.WithDescription( message );

        //Send our embed
        await arg.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
    }
}
