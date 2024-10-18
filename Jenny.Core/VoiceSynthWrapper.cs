using System;
using AudioSwitcher.AudioApi.CoreAudio;
using Google.Cloud.TextToSpeech.V1;
using NAudio.Wave;

namespace Jenny.Core
{
    public class VoiceSynthWrapper
    {
        //private readonly SpeechSynthesizer synthesizer;
        private readonly CoreAudioDevice defaultPlaybackDevice;
        private readonly TextToSpeechClient textToSpeechClient;
        private readonly VoiceSelectionParams voiceSelectionParams;
        private readonly AudioConfig audioConfig;

        public VoiceSynthWrapper()
        {
            /*
            synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            */
            defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;

            TextToSpeechClientBuilder builder = new TextToSpeechClientBuilder();
            builder.ApiKey = "AIzaSyDtMJ3Im6YoHiRPqMK11tkXXro3Aoipg0I";
            textToSpeechClient = builder.Build();
            voiceSelectionParams = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                SsmlGender = SsmlVoiceGender.Neutral
            };
            audioConfig = new AudioConfig { AudioEncoding = AudioEncoding.Linear16 };
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

        public void VolumeUp()
        {
            if(defaultPlaybackDevice.Volume <= 95)
            defaultPlaybackDevice.Volume += 5;
        }

        public void VolumeDown()
        {
            if (defaultPlaybackDevice.Volume >= 5)
                defaultPlaybackDevice.Volume -= 5;
        }

        public double Volume { get { return defaultPlaybackDevice.Volume; } }

        //public void Stop() => synthesizer.Dispose();
    }
}
