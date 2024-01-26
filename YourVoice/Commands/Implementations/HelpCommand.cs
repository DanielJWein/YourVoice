using System.Text;

namespace YourVoice.Commands;

/// <summary>
/// Returns usage information for each command
/// </summary>
internal sealed class HelpCommand : Command {

    /// <summary>
    /// Creates a new HelpCommand
    /// </summary>
    public HelpCommand( ) : base( "Help", "Returns help for commands" ) {
    }

    /// <inheritdoc />
    public override async Task Execute( Context context ) {
        //Create a new embed builder
        EmbedBuilder embedBuilder = new( );

        //Set the title and color
        embedBuilder.WithTitle( "Help for " + Settings.Name + ":" );
        embedBuilder.WithColor( Settings.AccentColor );

        //Set up our amalgamator for the description
        StringBuilder descriptionBuilder = new( );

        //Get the full description
        foreach (var cmd in Bot.Instance.CommandRegistry.GetCommands( )) {
            //Add this command to the description of the embed
            descriptionBuilder.AppendLine( cmd.Name + ": " + cmd.Description );
        }

        descriptionBuilder.AppendLine( "*Command names are case-insensitive*" );
        //Set the description of the emebd
        embedBuilder.WithDescription( descriptionBuilder.ToString( ) );

        //Send the embed.
        await context.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
    }
}
