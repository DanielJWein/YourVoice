namespace YourVoice.Commands;

/// <summary>
/// Sends information about a guild
/// </summary>
internal sealed class GuildCommand : Command {

    /// <summary>
    /// Creates a new GuildCommand
    /// </summary>
    public GuildCommand( ) : base( "Guild", "Shows information about the guild" ) {
        IsGuildCommand = true;
    }

    /// <inheritdoc />
    public override async Task Execute( Context context ) {
        //Create a new embed builder
        EmbedBuilder embedBuilder = new EmbedBuilder( );
        //Set its color
        embedBuilder.WithColor( Settings.AccentColor );

        //Try to get the guild from the context
        SocketGuild guild = (SocketGuild) context.Guild;

        //If the guild was null, we've reached an invalid state
        //Technically, this should be impossible since IsGuildCommand is true
        //But we'll handle it anyway.
        if (guild == null) {
            //Set the title to Error and the color to Red. Set the description to a message.
            embedBuilder.WithTitle( "Error" );
            embedBuilder.WithColor( Color.Red );
            embedBuilder.WithDescription( "Something went wrong. Did you use this command in a direct message?" );
        }
        //Else,
        else {
            //Set the title to the guild name, and the image URL to the guild's icon.
            embedBuilder.WithTitle( guild.Name );
            embedBuilder.WithThumbnailUrl( guild.IconUrl );

            //Create a description for the embed and assign it.
            string Description = $"Created at {guild.CreatedAt:dddd MMMM d, yyyy HH:mm:ss} by {guild.Owner}.\n" +
                $"Has {guild.MemberCount} members (not including bots).\n" +
                $"{(DateTime.UtcNow - guild.CreatedAt.UtcDateTime).TotalDays:n2} days old.";
            embedBuilder.WithDescription( Description );
        }
        //Send our embed
        await context.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
    }
}
