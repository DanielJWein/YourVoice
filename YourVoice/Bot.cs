using System.Text;

using Discord.Audio;

using YourVoice.Commands;

using YourVoice.Services;

using NAudio.Wave;

namespace YourVoice;

public class Program {

    public static void Main( ) {
        new Bot( ).MainAsync( ).GetAwaiter( ).GetResult( );
    }
}

public class Bot {
    internal static Bot Instance;

    internal AudioService audioService;

    internal CommandRegistry CommandRegistry = new( );

    internal DiscordSocketClient DiscordClient;

    internal ElevenLabsService elevenLabs;

    public Bot( ) {
        Instance = this;
        Initialize( );

        audioService = new( );
        elevenLabs = new( );
        //Bind all registry commands
        CommandRegistry.AddCommand( new AboutCommand( ) );
        CommandRegistry.AddCommand( new HelpCommand( ) );
        CommandRegistry.AddCommand( new GuildCommand( ) );
        CommandRegistry.AddCommand( new ListVoicesCommand( ) );
        CommandRegistry.AddCommand( new SetVoiceCommand( ) );
        CommandRegistry.AddCommand( new StartCommand( ) );
        CommandRegistry.AddCommand( new StopCommand( ) );
        CommandRegistry.AddCommand( new QuotaCommand( ) );

        //Create our config
        var Config = new DiscordSocketConfig();
        Config.GatewayIntents |= GatewayIntents.MessageContent;
        Config.LogGatewayIntentWarnings = true;

        //Create the client
        DiscordClient = new DiscordSocketClient( Config );

        //Bind to events
        DiscordClient.Ready += ReadyAsync;
        DiscordClient.Connected += Connected;
        DiscordClient.Disconnected += Disconnected;
        DiscordClient.MessageReceived += MessageReceived;
        DiscordClient.Log += LogAsync;

        //Set the console encodings to Unicode.
        Console.OutputEncoding = Console.InputEncoding = Encoding.Unicode;
    }

    /// <summary>
    /// Login and delay forever
    /// </summary>
    /// <returns> </returns>
    public async Task MainAsync( ) {
        string Token = File.ReadAllText( Settings.Dir + "token.txt" );
        await DiscordClient.LoginAsync( TokenType.Bot, Token );
        //Start the client
        await DiscordClient.StartAsync( );

        //Delay forever
        await Task.Delay( -1 );
    }

    /// <summary>
    /// Log that we have connected to the gateway
    /// </summary>
    /// <returns> </returns>
    internal Task Connected( ) {
        Console.WriteLine( Settings.Name + " is connected." );
        return Task.CompletedTask;
    }

    /// <summary>
    /// Log that we have disconnected from the gateway
    /// </summary>
    /// <returns> </returns>
    internal Task Disconnected( Exception e ) {
        Console.WriteLine( Settings.Name + " is disconnected." );
        Console.WriteLine( "Exception info: " + e.Message );
        return Task.CompletedTask;
    }

    /// <summary>
    /// Log a message
    /// </summary>
    /// <returns> </returns>
    internal Task LogAsync( LogMessage msg ) {
        Console.WriteLine( msg.Source + " - " + msg.Message );
        return Task.CompletedTask;
    }

    /// <summary>
    /// Log that we are ready
    /// </summary>
    /// <returns> </returns>
    internal async Task ReadyAsync( ) {
        Console.WriteLine( Settings.Name + " is ready!" );
        await DiscordClient.SetGameAsync( "v!help for help!" );
        await DiscordClient.SetStatusAsync( UserStatus.Online );
        audioService.CurrentStream = null;
    }

    private void AddClipThread( object? a ) {
        if (a is not Tuple<SocketVoiceChannel, ulong, string> x)
            return;
        StartAddClip( x.Item1, x.Item2, x.Item3 );
    }

    private async Task elevenLabsRoutine( SocketMessage arg ) {
        string? voice = UserVoiceService.GetUserVoice(arg.Author.Id);

        string? content = arg.Content;

        if (string.IsNullOrWhiteSpace( content )) {
            //The user probably sent a picture or something.
            return;
        }

        if (content.ToLower( ).Contains( "http" )) {
            //The user probably sent a link to a picture or something
            return;
        }

        if (voice is null) {
            await BotHelpers.SendFailEmbed( arg, "Failed! You haven't selected a voice yet!" );
            return;
        }

        if (content.Length > 100) {
            await BotHelpers.SendFailEmbed( arg, "Failed! Your message was too long!" );
            return;
        }

        SocketGuildUser? sgu = (arg.Author as SocketGuildUser);
        if (sgu is null) {
            await BotHelpers.SendFailEmbed( arg, "Failed! You need to be in a guild!" );
            return;
        }

        SocketVoiceChannel svc = sgu.VoiceChannel;
        if (svc is null) {
            await BotHelpers.SendFailEmbed( arg, "Failed! You need to be in a voice channel!" );
            return;
        }

        //We absolutely must connect to a voice channel from a new thread - connecting from this thread
        // blocks the gateway and guarantees that it will fail.
        Thread x = new Thread(new ParameterizedThreadStart(AddClipThread));
        x.Start( Tuple.Create( svc, arg.Author.Id, arg.Content ) );

        return;
    }

    /// <summary>
    /// Creates the settings directory and creates the token file
    /// </summary>
    private void Initialize( ) {
        Console.WriteLine( "Initializing the bot..." );
        //If our settings dir is new,
        if (!Directory.Exists( Settings.Dir )) {
            //Create the directory
            Directory.CreateDirectory( Settings.Dir );

            //Touch both token files
            File.Create( Settings.Dir + "token.txt" );
            File.Create( Settings.Dir + "elevenlabstoken.txt" );

            //Write a message and exit.
            Console.WriteLine( "Placeholder token files created. Fill them in to get started. Bot is exiting." );
            Environment.Exit( 1 );
        }
    }

    private async Task MessageReceived( SocketMessage arg ) {
        //Sometimes possible. Wacky.
        if (arg is null)
            return;

        //Make sure it was a USER who sent the content
        if (arg.Source != MessageSource.User)
            return;

        string msg = arg.Content;
        //Make sure there was actually content
        if (string.IsNullOrWhiteSpace( msg ))
            return;

        //The message is likely a command invocation
        if (msg.StartsWith( Settings.Prefix )) {
            //Get the content after the prefix
            string content = msg.Substring( Settings.Prefix.Length );
            //Split out the command name
            string commandName = content.Contains( ' ' ) ? content.Substring( 0, content.IndexOf( ' ' ) ) : content;

            //Try to get the command
            Command? cmd = CommandRegistry.GetCommand( commandName);

            //If there wasn't one, exit.
            if (cmd is null)
                return;

            //Otherwise, make sure we're in a guild
            if (cmd.IsGuildCommand && !(arg.Channel is SocketGuildChannel)) {
                await BotHelpers.SendFailEmbed( arg, "Sorry, but you can only use this command in a guild." );
                return;
            }

            //Execute the command
            await cmd.Execute( new Context( arg ) );
        }
        //If the user is in the TTS participators
        else if (UserVoiceService.IsParticipator( arg.Author.Id )) {
            try {
                //Submit the message to Eleven Labs.
                await elevenLabsRoutine( arg );
            }
            catch (Exception ex) {
                Console.WriteLine( ex );
            }
        }
    }

    private async void StartAddClip( SocketVoiceChannel where, ulong user, string content ) {
        if (audioService.CurrentStream is null) {
            try {
                IAudioClient audioClient = await where.ConnectAsync(false, false);
                audioService.CurrentStream = audioClient.CreatePCMStream( AudioApplication.Mixed );
            }
            catch {
                //We weren't able to join the channel
                return;
            }
        }

        try {
            RawSourceWaveStream? data = await elevenLabs.GetAudio( user, content );
            if (data is null) {
                Console.WriteLine( "Received no data from 11L!" );
                return;
            }
            audioService.PlayClip( data );
        }
        catch {
            return;
        }
    }
}
