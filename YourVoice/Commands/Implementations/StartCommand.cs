using YourVoice.Services;

namespace YourVoice.Commands;

/// <summary>
/// Sends information about the bot
/// </summary>
internal sealed class StartCommand : Command {

    /// <summary>
    /// Creates a new StartCommand
    /// </summary>
    public StartCommand( ) : base( "Start", "Starts the text-to-speech engine for your user!" ) {
    }

    /// <inheritdoc />
    public override async Task Execute( Context context ) {
        var id = context.User.Id;
        //User has a selected voice
        if (UserVoiceService.GetUserVoice( id ) is not null) {
            UserVoiceService.AddParticipator( id );

            //Create a new EmbedBuilder
            EmbedBuilder embedBuilder = new EmbedBuilder( );

            //Set the title and color of the builder
            embedBuilder.WithTitle( "Start" );
            embedBuilder.WithColor( Settings.AccentColor );

            //Assign the description to the embed
            embedBuilder.WithDescription( "Success!" );

            //Send our embed
            await context.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
        }
        //User has not selected a voice
        else {
            //Create a new EmbedBuilder
            EmbedBuilder embedBuilder = new EmbedBuilder( );

            //Set the title and color of the builder
            embedBuilder.WithTitle( "Start" );
            embedBuilder.WithColor( Color.Red );

            //Assign the description to the embed
            embedBuilder.WithDescription( $"Failed! You need to select a voice first!\n" +
                $"List voices: {Settings.Prefix}listvoices\n" +
                $"Set voice:{Settings.Prefix}setvoice <voiceName>" );

            //Send our embed
            await context.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
        }
    }
}
