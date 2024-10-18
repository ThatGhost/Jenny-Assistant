using Jenny.Core;

namespace Jenny.front.CommandChoices
{
    public class CC_Entry : CommandPath
    {
        private readonly VoiceSynthesizer voiceSynth;
        private readonly DictationBuilder dictationChoicesBuilder;
        private readonly SpeechRecognizer speechWrapper;

        CC_Config configChoice;
        CC_SpanishHelp spanishHelp;

        public CC_Entry(
            VoiceSynthesizer voiceSynth,
            DictationBuilder dictationChoicesBuilder,
            SpeechRecognizer speechWrapper,
            CC_Config config,
            CC_SpanishHelp spanishHelp)
        {
            this.voiceSynth = voiceSynth;
            this.dictationChoicesBuilder = dictationChoicesBuilder;
            this.speechWrapper = speechWrapper;

            this.configChoice = config;
            this.spanishHelp = spanishHelp;

            speechActions = new Dictionary<string, DictationBuilder.SpeechAction>()
            {
                { "Hey Jenny", (string s) => { onEntry("Hey Ibn, Do you need anything?"); } },
                { "Jenny are you there", (string s) => { onEntry("Yes i am here, Anything you want?"); } },
                { "Jenny you there", (string s) => { onEntry("Yes right here. Is there anything you need"); } }
            };
        }

        private void onEntry(string awnser)
        {
            voiceSynth.SpeakAndWrite(awnser);

            dictationChoicesBuilder.ClearDictations();
            dictationChoicesBuilder.AddCommandPath(configChoice);
            dictationChoicesBuilder.AddCommandPath(spanishHelp);
            dictationChoicesBuilder.AddScentence("No", (string s) => { dictationChoicesBuilder.ClearDictations(); dictationChoicesBuilder.AddCommandPath(this); speechWrapper.UpdateGrammar(); });
            dictationChoicesBuilder.AddScentence("Clear the screen", (string s) => Console.Clear());

            speechWrapper.UpdateGrammar();
        }
    }
}
