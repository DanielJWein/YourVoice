using YourVoice.Services;

namespace YourVoice.Commands;

/// <summary>
/// Sends information about the bot
/// </summary>
internal sealed class SetVoiceCommand : Command {

    /// <summary>
    /// Creates a new SetVoiceCommand
    /// </summary>
    public SetVoiceCommand( ) : base( "SetVoice", "Sets your preferred voice" ) {
        Usage = Settings.Prefix + "setvoice (name)\n*See " + Settings.Prefix + "listvoices for voices.*";
    }

    /// <inheritdoc />
    public override async Task Execute( Context context ) {
        string arg = context.Content.Substring(Settings.Prefix.Length + Name.Length).Trim();
        if (UserVoiceService.AvailableVoices.Contains( arg )) {
            UserVoiceService.SetUserVoice( context.User.Id, arg );
            UserVoiceService.SaveVoices( );

            EmbedBuilder embedBuilder = new EmbedBuilder( );

            //Set the title and color of the builder
            embedBuilder.WithTitle( "Set Voice" );
            embedBuilder.WithColor( Color.Green );

            //Assign the description to the embed
            embedBuilder.WithDescription( "Success!" );

            //Send our embed
            await context.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
        }
        else {
            //Create a new EmbedBuilder
            EmbedBuilder embedBuilder = new EmbedBuilder( );

            //Set the title and color of the builder
            embedBuilder.WithTitle( "Set Voice" );
            embedBuilder.WithColor( Color.Red );

            //Assign the description to the embed
            embedBuilder.WithDescription( "Failed! See " + Settings.Prefix + "listvoices to see what voices are available." );

            //Send our embed
            await context.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
        }
    }
}
