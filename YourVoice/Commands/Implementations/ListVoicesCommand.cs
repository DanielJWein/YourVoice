using YourVoice.Services;

namespace YourVoice.Commands;

/// <summary>
/// Sends information about the bot
/// </summary>
internal sealed class ListVoicesCommand : Command {

    /// <summary>
    /// Creates a new ListVoicesCommand
    /// </summary>
    public ListVoicesCommand( ) : base( "ListVoices", "Shows the list of available voices and their descriptions" ) {
    }

    /// <inheritdoc />
    public override async Task Execute( Context context ) {
        //Create a new EmbedBuilder
        EmbedBuilder embedBuilder = new EmbedBuilder( );

        //Set the title and color of the builder
        embedBuilder.WithTitle( "Voices" );
        embedBuilder.WithColor( Settings.AccentColor );

        //Assign the description to the embed
        embedBuilder.WithDescription( String.Join( ", ", UserVoiceService.AvailableVoices ) );

        //Send our embed
        await context.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
    }
}
