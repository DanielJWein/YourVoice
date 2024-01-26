using YourVoice.Services;

namespace YourVoice.Commands;

/// <summary>
/// Sends information about the bot
/// </summary>
internal sealed class StopCommand : Command {

    /// <summary>
    /// Creates a new StopCommand
    /// </summary>
    public StopCommand( ) : base( "Stop", "Stops the text-to-speech engine for your user!" ) {
    }

    /// <inheritdoc />
    public override async Task Execute( Context context ) {
        var id = context.User.Id;
        UserVoiceService.RemoveParticipator( id );

        //Create a new EmbedBuilder
        EmbedBuilder embedBuilder = new EmbedBuilder( );

        //Set the title and color of the builder
        embedBuilder.WithTitle( "Stop" );
        embedBuilder.WithColor( Settings.AccentColor );

        //Assign the description to the embed
        embedBuilder.WithDescription( "Success!" );

        //Send our embed
        await context.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
    }
}
