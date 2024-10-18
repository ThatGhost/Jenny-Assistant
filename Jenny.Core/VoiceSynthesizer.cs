using System;
using AudioSwitcher.AudioApi.CoreAudio;
using Google.Cloud.TextToSpeech.V1;
using NAudio.Wave;

namespace Jenny.Core
{
    public class VoiceSynthesizer
    {
        private TextToSpeechClient textToSpeechClient;
        private VoiceSelectionParams voiceSelectionParams;
        private readonly AudioConfig audioConfig;
        private readonly LogService logService;

        public VoiceSynthesizer(LogService logService)
        {
            this.logService = logService;

            audioConfig = new AudioConfig { AudioEncoding = AudioEncoding.Linear16 };
            voiceSelectionParams = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                SsmlGender = SsmlVoiceGender.Female,
            };
        }

        public void SetTTSApiKey(string GoogleVoiceApiKey)
        {
            TextToSpeechClientBuilder builder = new TextToSpeechClientBuilder();
            builder.ApiKey = GoogleVoiceApiKey;
            textToSpeechClient = builder.Build();
        }

        public void UpdateLanguage(string language)
        {
            voiceSelectionParams = new VoiceSelectionParams
            {
                LanguageCode = language,
                SsmlGender = SsmlVoiceGender.Female,
            };
        }

        public void Speak(string text)
        {
            var input = new SynthesisInput { Text = text };
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
            logService.LogAssistant(" - " + command);
            Speak(command);
        }
    }
}
