namespace YourVoice.Commands;

/// <summary>
/// Sends information about the bot
/// </summary>
internal sealed class AboutCommand : Command {

    /// <summary>
    /// Creates a new AboutCommand
    /// </summary>
    public AboutCommand( ) : base( "About", "Returns information about the bot" ) {
    }

    /// <inheritdoc />
    public override async Task Execute( Context context ) {
        //Create a new EmbedBuilder
        EmbedBuilder embedBuilder = new EmbedBuilder( );

        //Set the title and color of the builder
        embedBuilder.WithTitle( Settings.Name );
        embedBuilder.WithColor( Settings.AccentColor );

        //Get the client for the discord information
        DiscordSocketClient client = Bot.Instance.DiscordClient;

        //Get the number of guilds the client is in
        int numGuilds = client.Guilds.Count;
        //Get the number of users the client is serving
        int numUsers = client.Guilds.Select ( x => x.MemberCount).Sum();

        //Set up the description for the embed
        string Description = $"In {numGuilds} servers, serving {numUsers} users.\nCreated by {Settings.CreatorString}.";

        //Assign the description to the embed
        embedBuilder.WithDescription( Description );

        //Send our embed
        await context.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
    }
}
