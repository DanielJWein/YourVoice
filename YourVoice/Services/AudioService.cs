using Discord.Audio;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace YourVoice.Services {

    public sealed class AudioService {

        /// <summary>
        /// Holds the channels that users can write messages to.
        /// </summary>
        private readonly BufferedWaveProvider[ ] channels = new BufferedWaveProvider[ 3 ];

        /// <summary>
        /// Mixes the channels into one output
        /// </summary>
        private readonly MixingWaveProvider32 mixingWaveProvider32 = new( );

        /// <summary>
        /// A thread for sending data to Discord
        /// </summary>
        private readonly Thread sendDataThread;

        /// <summary>
        /// Holds the current output stream.
        /// </summary>
        private AudioOutStream? _currentStream;

        /// <summary>
        /// Initializes this AudioService
        /// </summary>
        public AudioService( ) {
            _currentStream = null;
            for (int i = 0; i < channels.Length; i++) {
                channels[ i ] = new( WaveFormat.CreateIeeeFloatWaveFormat( 48000, 2 ) ) {
                    BufferDuration = TimeSpan.FromSeconds( 10 ),
                    ReadFully = true
                };

                mixingWaveProvider32.AddInputStream( channels[ i ] );
            }

            sendDataThread = new( SendAudioThread );
            sendDataThread.Start( );
        }

        /// <summary>
        /// Gets or sets the current audio stream. The bot currently doesn't support multiple servers.
        /// </summary>
        public AudioOutStream? CurrentStream {
            get => _currentStream;
            set {
                if (_currentStream != value) {
                    _currentStream = value;
                }
            }
        }

        /// <summary>
        /// Plays an audio clip
        /// </summary>
        /// <param name="rawStream"> The audio to play. </param>
        public void PlayClip( RawSourceWaveStream rawStream ) {
            if (_currentStream is null) {
                return;
            }

            //Convert the sample rate from 24KHz -> 48KHz to match Discord's format
            WdlResamplingSampleProvider resampler = new(rawStream.ToSampleProvider(), 48000);

            //Convert the stream to stereo to match Discord's format
            var toStereo = resampler.ToStereo();

            //Conver the stream from unknown PCM to IEEE 32-bit float
            var toIEEEFloat = toStereo.ToWaveProvider();

            //Create a buffer to store the audio data
            byte[] audioDataBuffer = new byte[toIEEEFloat.WaveFormat.AverageBytesPerSecond * 8];

            //Read the data and store the length we read
            int bytesRead = toIEEEFloat .Read(audioDataBuffer, 0, audioDataBuffer.Length);

//Label to retry finding a channel
retry:
            int writeToChannel = -1;

            for (int i = 0; i < channels.Length; i++) {
                if (channels[ i ].BufferedDuration == TimeSpan.Zero) {
                    writeToChannel = i;
                }
            }

            //Loop backwards if we didn't find a clear buffer.
            if (writeToChannel is -1) {
                Thread.Sleep( 10 );
                goto retry;
            }

            //Write the data to the channel
            channels[ writeToChannel ].AddSamples( audioDataBuffer, 0, bytesRead );
        }

        //Submit our data to Discord
        private async void SendAudioThread( object? nil ) {
            //Unused variable
            _ = nil;

            while (true) {
                //We use label/goto here because it is easier to understand than
                // rewriting { Thread.Sleep(50); continue; }
                if (_currentStream is null)
                    goto wait;

                //Find the maximum duration of time we need to send
                TimeSpan dataToSend = channels.Max(x=>x.BufferedDuration );

                //If it's zero, wait
                if (dataToSend == TimeSpan.Zero)
                    goto wait;

                //AverageBytesPerSecond should always be 48000 * 16 * 2 / 8 = 192kbps,
                // but we calculate it anyway since it may change in the future.
                // Plus, it's nearly free & realtime operation isn't important
                // when Discord will have its delays anyway.
                int dataLength = (int)(Math.Ceiling(dataToSend.TotalSeconds * mixingWaveProvider32.WaveFormat.AverageBytesPerSecond));

                //Read the data in
                byte[] data = new byte[dataLength];

                //Convert from IEEE Float format to S16LE PCM
                var stereoWaveProvider = mixingWaveProvider32.ToSampleProvider();
                SampleToWaveProvider16 to16wp = new SampleToWaveProvider16(stereoWaveProvider);

                //Create a buffer and store the audio data
                byte[] audioDataBuffer = new byte[ dataLength ];
                int numBytesRead = to16wp.Read( data, 0, dataLength );

                //! If your bot is failing here, you need libsodium and libopus.
                //Write it to Discord
                await _currentStream.WriteAsync( data, 0, numBytesRead );
                await _currentStream.FlushAsync( );

                //This sleep is important - if we don't sleep and two people register a message at once,
                // this will cause the data to overlap and become completely unintelligible.
                Thread.Sleep( (int) (dataToSend.TotalSeconds * 1000) );

                continue;

wait:
                Thread.Sleep( 50 );
            }
        }
    }
}
