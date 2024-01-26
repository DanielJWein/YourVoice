# YourVoice
A Discord bot that allows users to use Eleven Labs TTS voices.

Discord: https://www.discord.com/

Eleven Labs: https://www.elevenlabs.io/

## Requirements / Setup
You'll need a Discord bot and an Eleven Labs API key. Getting an Eleven Labs API key is free (at least, as of 1/26/2024).
To set up the bot, all you need to do is compile it and run it. It will create two files - token.txt and elevenlabstoken.txt in C:/ProgramData/Nuyube/Discord Bots/YourVoice.
Fill these two files with your Discord token and Eleven Labs API Key respectively and restart the bot.
It should begin working immediately.

## How to Use
YourVoice is not set up to be deployed in multiple servers simultaneously, so you'll only be able to use it in one voice chat.

To begin, use v!listvoices to determine which voices are available. Then, copy one of those voice names, and use v!setvoice <voice>. Then, use v!start when you're in a voice channel and it will speak all of your messages (the first message when joining a channel may not play until a second message is sent).

```
v!listvoices
> Mark ... (example output)
v!setvoice Mark
v!start

hey guys!
```
## Troubleshooting
*YourVoice is not sending any audio!*
You may not have libsodium and libopus. Open a terminal in the solution directory and run `dotnet restore`.

## Acknowledgements
I would like to thank the Discord.NET team for making an easy-to-use client library for Discord in C#, 
Eleven Labs for providing a free 10,000 character tier for their service, 
Stephen Hodgson for making an extremely simple to use C# Eleven Labs API client library,
Mark Heath and all NAudio contributors for making a great audio library, 
and *you* for viewing my repository.

Libraries:

NAudio repo: https://github.com/naudio/NAudio

Discord.NET repo: https://github.com/discord-net/Discord.Net

Eleven Labs Library repo: https://github.com/RageAgainstThePixel/ElevenLabs-DotNet

## License
MIT license. You can take this bot and do whatever you please with it. I would like if you could credit me, but it is not necessary. 
