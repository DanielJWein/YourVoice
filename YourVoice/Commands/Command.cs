namespace YourVoice.Commands;

internal abstract class Command {

    /// <summary>
    /// A description of the command
    /// </summary>
    public string Description;

    /// <summary>
    /// If true, the command can only be used in a guild.
    /// </summary>
    public bool IsGuildCommand = false;

    /// <summary>
    /// The name of the command
    /// </summary>
    public string Name;

    /// <summary>
    /// How to use the command
    /// </summary>
    public string Usage;

    /// <summary>
    /// Creates a new command
    /// </summary>
    /// <param name="Name">        The name of the command </param>
    /// <param name="Description"> The description for the command </param>
    /// <param name="Usage">      
    /// Defines the usage string for the command. If unset, defaults to Prefix + Name
    /// </param>
    protected Command( string Name, string Description, string? Usage = null ) {
        this.Name = Name;
        this.Description = Description;
        if (Usage is null)
            this.Usage = Settings.Prefix + Name;
        else
            this.Usage = Usage;
    }

    /// <summary>
    /// Executes this command
    /// </summary>
    /// <param name="context"> The context for the command </param>
    /// <exception cref="NotImplementedException"> Thrown if the command is not yet implemented. </exception>
    public virtual async Task Execute( Context context )
        => throw new NotImplementedException( );
}
