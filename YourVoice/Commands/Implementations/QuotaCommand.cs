using YourVoice.Services;

namespace YourVoice.Commands;

/// <summary>
/// Returns usage information for each command
/// </summary>
internal sealed class QuotaCommand : Command {

    /// <summary>
    /// Creates a new QuotaCommand
    /// </summary>
    public QuotaCommand( ) : base( "Quota", "Report the amount of remaining quota." ) {
    }

    /// <inheritdoc />
    public override async Task Execute( Context context ) {
        ElevenLabsService elevenLabs = Bot.Instance.elevenLabs;

        //Get information from Eleven Labs
        int used = elevenLabs.GetUsedQuota();
        int max = elevenLabs.GetMaxQuota();
        //Possible "infinite quota" case?
        if (max == 0) {
            max = int.MaxValue;
        }
        DateTime nextReset = elevenLabs.GetQuotaResetDate();

        //Create a new embed builder
        EmbedBuilder embedBuilder = new( );

        //Set the title and color
        embedBuilder.WithTitle( "Quota:" );
        embedBuilder.WithColor( Settings.AccentColor );

        //Set the description of the emebd
        embedBuilder.WithDescription( $"Usage: {used} / {max} ({((double) used / max):0.00%})\n" +
            $"Resets {nextReset.ToUniversalTime( ):MM/dd/yyyy HH:mm:ss} UTC" );

        //Send the embed.
        await context.Channel.SendMessageAsync( null, false, embedBuilder.Build( ) );
    }
}
