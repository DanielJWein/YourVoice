namespace YourVoice.Commands;

/// <summary>
/// Holds a registry for the commands
/// </summary>
internal class CommandRegistry {

    /// <summary>
    /// Holds the commands currently loaded
    /// </summary>
    private readonly List<Command> Commands = [ ];

    /// <summary>
    /// Adds a command to the registry
    /// </summary>
    /// <param name="c"> The command to add </param>
    public void AddCommand( Command c ) => Commands.Add( c );

    /// <summary>
    /// Gets a command from the registry
    /// </summary>
    /// <param name="name"> The name of the command to retrieve. Case insensitive </param>
    /// <returns> The command if found, or null. </returns>
    public Command? GetCommand( string name )
        => Commands.FirstOrDefault( c => c.Name.ToUpperInvariant( ) == name.ToUpperInvariant( ) );

    /// <summary>
    /// Gets all of the loaded commands
    /// </summary>
    /// <returns> All loaded commands. </returns>
    public IEnumerable<Command> GetCommands( ) => Commands;

    /// <summary>
    /// Removes a command from the registry
    /// </summary>
    /// <param name="c"> The command to remove </param>
    public void RemoveCommand( Command c ) => Commands.Remove( c );
}
