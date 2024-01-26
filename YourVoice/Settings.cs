namespace YourVoice;

internal static class Settings {

    /// <summary>
    /// Holds the name of the creator
    /// </summary>
    public const string CreatorString = "Daniel Wein";

    /// <summary>
    /// Holds the data directory for the bot
    /// </summary>
    public const string Dir = "C:/ProgramData/Discord Bots/" + Name + "/";

    /// <summary>
    /// Holds an invite link for this bot
    /// </summary>
    public const string InviteLink = "https://discord.com/api/oauth2/authorize?client_id=1195155842207588412&permissions=3145728&scope=bot";

    /// <summary>
    /// Holds the name of the bot
    /// </summary>
    public const string Name = "Your Voice";

    /// <summary>
    /// Holds the name of the owner of this bot
    /// </summary>
    public const string OwnerName = "Daniel Wein";

    /// <summary>
    /// Holds the prefix for this bot.
    /// </summary>
    public const string Prefix = "v!";

    /// <summary>
    /// Holds the default accent color for embeds, etc.
    /// </summary>
    public static readonly Color AccentColor = Color.Gold;
}
