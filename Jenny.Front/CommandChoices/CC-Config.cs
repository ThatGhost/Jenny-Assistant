using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jenny.Core;
using Jenny.front.Services;

namespace Jenny.front.CommandChoices
{
    public class CC_Config : CommandChoice
    {
        private readonly VoiceSynthWrapper voiceSynth;
        private readonly DictationChoicesBuilder dictationChoicesBuilder;
        private readonly SpeechRecognitionWrapper speechWrapper;
        private readonly VolumeService volumeService;
        private readonly LogService logService;

        public CC_Config(
            VoiceSynthWrapper voiceSynth,
            DictationChoicesBuilder dictationChoicesBuilder,
            SpeechRecognitionWrapper speechRecognitionWrapper,
            VolumeService volumeService,
            LogService logService
            )
        {
            this.logService = logService;
            this.voiceSynth = voiceSynth;
            this.dictationChoicesBuilder = dictationChoicesBuilder;
            this.speechWrapper = speechRecognitionWrapper;
            this.volumeService = volumeService;

            triggers = new Dictionary<string, DictationChoicesBuilder.SpeechAction>
            {
                { "I want to update some of your config", OnConfig },
                { "update config", OnConfig },
                { "update configuration", OnConfig },
                { "Change some settings", OnConfig },
                { "Update some settings", OnConfig },
                { "I want to update some of your settings", OnConfig },
            };
        }

        private void OnConfig(string data)
        {
            voiceSynth.SpeakAndWrite("Ok, what do you want to update?");

            dictationChoicesBuilder.Clear();
            dictationChoicesBuilder.AddScentence("volume up", (string o) => { volumeService.VolumeUp(); logService.LogAssistant("Volume: " + volumeService.Volume); });
            dictationChoicesBuilder.AddScentence("volume down", (string o) => { volumeService.VolumeDown(); logService.LogAssistant("Volume: " + volumeService.Volume); });
            dictationChoicesBuilder.AddScentence("set volume", (string o) => { SetVolume(); });
            dictationChoicesBuilder.AddScentence("Say something", (string o) => { voiceSynth.Speak("Hoyaaaaaaaa"); });
            speechWrapper.UpdateGrammar();

            logService.LogWithColor($"Volume: {volumeService.Volume}", ConsoleColor.Green);
            logService.LogWithColor($"- Volume up ", ConsoleColor.Cyan);
            logService.LogWithColor($"- Volume down ", ConsoleColor.Cyan);
            logService.LogWithColor($"- Set Volume ", ConsoleColor.Cyan);
        }

        private void SetVolume()
        {
            voiceSynth.SpeakAndWrite("What Volume do you want it?");
            dictationChoicesBuilder.Clear();
            dictationChoicesBuilder.NumbersBetween(1,100, (string awnser) =>
            {
                int volume = int.Parse(awnser);
                volumeService.SetVolume(volume);
                logService.LogAssistant("Volume: " + volumeService.Volume);
            });
            speechWrapper.UpdateGrammar();
        }
    }
}
