using NAudio.Wave;

using YourVoice.Services.Exceptions;

namespace YourVoice.Services {

    public sealed class ElevenLabsService {
        private readonly ElevenLabs.ElevenLabsAuthentication auth;

        private readonly ElevenLabs.ElevenLabsClient elevenClient;

        public ElevenLabsService( ) {
            string Token = File.ReadAllText( Settings.Dir + "elevenlabstoken.txt" );
            auth = new( Token );
            elevenClient = new( auth );
        }

        /// <summary>
        /// Gets an audio clip
        /// </summary>
        /// <param name="user">    The user who wrote the message </param>
        /// <param name="content"> The message to speak </param>
        /// <returns> A wave stream representing the audio data we received from Eleven Labs </returns>
        public async Task<RawSourceWaveStream?> GetAudio( ulong user, string content ) {
            //! Preconditions

            if (string.IsNullOrWhiteSpace( content )) {
                throw new ArgumentNullException( nameof( content ) );
            }

            //Get the user's voice selection
            string? voice = UserVoiceService.GetUserVoice(user);
            if (voice is null)
                throw new NoUserVoiceSelectionException( "The user has not selected a voice!" );

            //Ensure they're in the TTS Participators
            bool isParticipant = UserVoiceService.IsParticipator(user);
            if (!isParticipant)
                throw new UserNotParticipatingException( "The user is not a participator in TTS!" );

            //Get the API info from UVS
            var apiInfo = UserVoiceService.apiInfo;
            var voices = apiInfo?.Voices;
            if (voices is null)
                throw new NoApiInfoException( "Could not get the Eleven Labs API data!" );

            //Get the real ID of the voice
            string? voiceId = voices.FirstOrDefault(x=>x.Name == voice)?.VoiceId ?? null;
            if (voiceId == null)
                throw new NoSuchVoiceException( "Could not get the voice ID!" );

            //Get the real voice object
            var voicex = await elevenClient.VoicesEndpoint.GetVoiceAsync(voiceId);
            if (voicex is null)
                throw new NoSuchVoiceException( "Could not get the voice from Eleven Labs!" );

            //Get an audio clip from Eleven Labs
            var res = await elevenClient.TextToSpeechEndpoint.TextToSpeechAsync( content, voicex, outputFormat:ElevenLabs.OutputFormat.PCM_24000 );
            if (res is null) {
                throw new ElevenLabsOperationFailedException( "Eleven Labs failed to send us a result!" );
            }

            //Read the data
            var data = res.ClipData;
            if (data.IsEmpty || data.Length is 0) {
                throw new ElevenLabsOperationFailedException( "Eleven Labs didn't send us any data!" );
            }
            RawSourceWaveStream rsws = new(data.ToArray(), 0, data.Length, new(24000,1));

#if DEBUG
            //Write it to the last wave file
            WaveFileWriter wfw = new(new FileStream(Settings.Dir + "lastWave.wav", FileMode.Create), rsws.WaveFormat);
            wfw.Write( data.ToArray( ), 0, data.Length );
            wfw.Close( );
#endif

            return rsws;
        }

        /// <summary>
        /// Gets the maximum number of quota characters.
        /// </summary>
        /// <returns> The max number of quota characters. </returns>
        public int GetMaxQuota( ) {
            return elevenClient.UserEndpoint.GetSubscriptionInfoAsync( ).Result.CharacterLimit;
        }

        /// <summary>
        /// Gets the quota reset date.
        /// </summary>
        /// <returns> The date your quota resets. </returns>
        public DateTime GetQuotaResetDate( ) {
            return elevenClient.UserEndpoint.GetSubscriptionInfoAsync( ).Result.NextCharacterCountReset;
        }

        /// <summary>
        /// Gets the used number of quota characters.
        /// </summary>
        /// <returns> The number of quota characters used. </returns>
        public int GetUsedQuota( ) {
            return elevenClient.UserEndpoint.GetSubscriptionInfoAsync( ).Result.CharacterCount;
        }
    }
}
