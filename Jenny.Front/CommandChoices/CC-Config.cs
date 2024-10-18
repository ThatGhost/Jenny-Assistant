using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jenny.Core;

namespace Jenny.front.CommandChoices
{
    public class CC_Config : CommandChoice
    {
        private readonly VoiceSynthWrapper voiceSynth;
        private readonly DictationChoicesBuilder dictationChoicesBuilder;
        private readonly SpeechRecognitionWrapper speechWrapper;

        public CC_Config(
            VoiceSynthWrapper voiceSynth,
            DictationChoicesBuilder dictationChoicesBuilder,
            SpeechRecognitionWrapper speechRecognitionWrapper)
        {
            this.voiceSynth = voiceSynth;
            this.dictationChoicesBuilder = dictationChoicesBuilder;
            this.speechWrapper = speechRecognitionWrapper;

            triggers = new Dictionary<string, DictationChoicesBuilder.SpeechAction>
            {
                { "I want to update some of your config", OnConfig },
                { "update your config", OnConfig },
                { "update your configuration", OnConfig },
                { "Change some settings", OnConfig },
                { "Update some of your settings", OnConfig },
                { "I want to update some of your settings", OnConfig },
            };
        }

        private void OnConfig()
        {
            voiceSynth.SpeakAndWrite("Ok, what do you want to update?");

            dictationChoicesBuilder.Clear();
            dictationChoicesBuilder.AddScentence("volume up", () => { voiceSynth.VolumeUp(); Console.WriteLine("Volume: " + voiceSynth.Volume); });
            dictationChoicesBuilder.AddScentence("volume down", () => { voiceSynth.VolumeDown(); Console.WriteLine("Volume: " + voiceSynth.Volume); });
            speechWrapper.UpdateGrammar();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Volume: " + voiceSynth.Volume);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" - volume up");
            Console.WriteLine(" - volume down");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
