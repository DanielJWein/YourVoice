//For HttpClient
using System.Net.Http;

//For Settings

//For JsonConvert
using Newtonsoft.Json;

//For Root
using QuickType;

namespace YourVoice.Services {

    /// <summary>
    /// Registers which voice a user has picked, as well as keeping a list of users who are
    /// currently using TTS.
    /// </summary>
    public static class UserVoiceService {

        /// <summary>
        /// Holds all of the information that Eleven Labs provided regarding the voices they have.
        /// </summary>
        public static Root? apiInfo = null;

        /// <summary>
        /// Holds the URL of the ElevenLabs v1 API Voices endpoint
        /// </summary>
        private const string ElevenLabsApiURL = "https://api.elevenlabs.io/v1/voices";

        /// <summary>
        /// Holds the path of the uservoices file
        /// </summary>
        private const string UserVoicesPath = Settings.Dir + "uservoices.json";

        /// <summary>
        /// Holds a list of voices that are available from Eleven labs.
        /// </summary>
        private static List<string> availableVoices = new( );

        /// <summary>
        /// Holds a list of user IDs that are participating in Text-To-Speech
        /// </summary>
        private static List<ulong> participators = new( );

        /// <summary>
        /// Holds a list of which user has selected which voice.
        /// </summary>
        private static Dictionary<ulong, string> selectedVoices;

        /// <summary>
        /// Initializes this service
        /// </summary>
        static UserVoiceService( ) {
            selectedVoices = new( );
            LoadVoices( );
        }

        /// <summary>
        /// Holds a list of voices that are available from Eleven labs.
        /// </summary>
        public static string[ ] AvailableVoices => availableVoices.ToArray( );

        /// <summary>
        /// Adds a user to the Text-to-Speech Participators
        /// </summary>
        /// <param name="id"> The user to add </param>
        public static void AddParticipator( ulong id ) {
            participators.Add( id );
        }

        /// <summary>
        /// Gets a user's selected voice
        /// </summary>
        /// <param name="id"> The user to query </param>
        /// <returns>
        /// The name of the voice they selected, or null if they have not selected a voice yet.
        /// </returns>
        public static string? GetUserVoice( ulong id ) {
            if (selectedVoices.ContainsKey( id )) {
                return selectedVoices[ id ];
            }

            return null;
        }

        /// <summary>
        /// Returns whether a user is in the Text-to-Speech Participators or not.
        /// </summary>
        /// <param name="id"> The user to query </param>
        /// <returns> true, if the user is in the participators list. </returns>
        public static bool IsParticipator( ulong id ) {
            return participators.Contains( id );
        }

        /// <summary>
        /// Gets a list of voices from Eleven Labs
        /// </summary>
        public static void LoadVoices( ) {
            //Create or reset the SelectedVoices list
            if (selectedVoices is null) {
                selectedVoices = [ ];
            }
            else {
                selectedVoices.Clear( );
            }

            //Download the voice information from Eleven Labs
            HttpClient httpClient = new();
            HttpRequestMessage request = new(HttpMethod.Get, ElevenLabsApiURL);
            HttpResponseMessage response = httpClient.Send( request );

            string jsonVoices = response.Content.ReadAsStringAsync( ).GetAwaiter().GetResult();

            response.Dispose( );
            request.Dispose( );
            httpClient.Dispose( );

            //Conver the downloaded info into a readable format.
            Root? elevenLabsApiInfo = JsonConvert.DeserializeObject<QuickType.Root>( jsonVoices ) ;
            if (elevenLabsApiInfo is null) {
                Console.WriteLine( "Couldn't download voice data." );
                //This is fatal, so we exit.
                Environment.Exit( 1 );
            }

            //Assign the API info to the visible fields
            apiInfo = elevenLabsApiInfo;

            //Calculate which voices are available
            availableVoices = [ .. apiInfo.Voices.Select( x => x.Name ) ];

            //Try to load the UserVoices settings file. If it doesn't exist, leave this function.
            if (!File.Exists( UserVoicesPath )) {
                Console.WriteLine( "No User Voices file found." );
                return;
            }
            Dictionary<ulong,string>? deserializedVoiceChoice
                = JsonConvert.DeserializeObject<Dictionary<ulong, string>>
                    ( File.ReadAllText( UserVoicesPath ) );

            if (deserializedVoiceChoice is not null) {
                selectedVoices = deserializedVoiceChoice;
            }
            else {
                Console.WriteLine( "Could not deserialize uservoices.json!" );
            }
        }

        /// <summary>
        /// Removes a user from the Text-to-Speech Participators
        /// </summary>
        /// <param name="id"> The user to remove </param>
        public static void RemoveParticipator( ulong id ) {
            participators.RemoveAll( x => x == id );
        }

        /// <summary>
        /// Saves the selected voices list.
        /// </summary>
        public static void SaveVoices( ) {
            string fileContent = JsonConvert.SerializeObject(selectedVoices, Formatting.Indented);

            File.WriteAllText( UserVoicesPath, fileContent );
        }

        /// <summary>
        /// Sets a user's selected voice
        /// </summary>
        /// <param name="userId"> The user to modify </param>
        /// <param name="Voice">  The voice to select. </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="Voice" /> is not in the available voices list.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="Voice" /> is null.
        /// </exception>
        public static void SetUserVoice( ulong userId, string Voice ) {
            if (Voice is null) {
                throw new ArgumentNullException( nameof( Voice ) );
            }
            if (!availableVoices.Contains( Voice )) {
                throw new ArgumentException( "That voice was not in the available voices!", nameof( Voice ) );
            }
            selectedVoices[ userId ] = Voice;
        }
    }
}
