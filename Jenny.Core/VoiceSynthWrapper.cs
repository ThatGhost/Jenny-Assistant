using System;
using AudioSwitcher.AudioApi.CoreAudio;
using Google.Cloud.TextToSpeech.V1;
using NAudio.Wave;

namespace Jenny.Core
{
    public class VoiceSynthWrapper
    {
        //private readonly SpeechSynthesizer synthesizer;
        private TextToSpeechClient textToSpeechClient;
        private VoiceSelectionParams voiceSelectionParams;
        private readonly AudioConfig audioConfig;

        public VoiceSynthWrapper()
        {
            /*
            synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            */

            audioConfig = new AudioConfig { AudioEncoding = AudioEncoding.Linear16 };
        }

        public void SetGoogleApiKey(string GoogleVoiceApiKey)
        {
            TextToSpeechClientBuilder builder = new TextToSpeechClientBuilder();
            builder.ApiKey = GoogleVoiceApiKey;
            textToSpeechClient = builder.Build();
            voiceSelectionParams = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                SsmlGender = SsmlVoiceGender.Female,
            };
        }

        public void Speak(string command)
        {
            var input = new SynthesisInput { Text = command };

            // Get the response without using a 'using' statement
            var response = textToSpeechClient.SynthesizeSpeech(input, voiceSelectionParams, audioConfig);

            // Use MemoryStream to play audio
            using (var ms = new MemoryStream(response.AudioContent.ToByteArray()))
            {
                using (var waveStream = new WaveFileReader(ms))
                {
                    using (var waveOut = new WaveOutEvent())
                    {
                        waveOut.Init(waveStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            Thread.Sleep(100); // Wait for the audio to finish playing
                        }
                    }
                }
            }
        }

        public void SpeakAndWrite(string command)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - "+command);
            Console.ForegroundColor = ConsoleColor.Gray;
            Speak(command);
        }


        //public void Stop() => synthesizer.Dispose();
    }
}
