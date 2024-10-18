using Jenny.Core;

namespace Jenny.front.CommandChoices
{
    public class CC_Entry : CommandChoice
    {
        private readonly VoiceSynthWrapper voiceSynth;
        private readonly DictationChoicesBuilder dictationChoicesBuilder;
        private readonly SpeechRecognitionWrapper speechWrapper;

        CC_Config configChoice;

        public CC_Entry(
            VoiceSynthWrapper voiceSynth,
            DictationChoicesBuilder dictationChoicesBuilder,
            SpeechRecognitionWrapper speechWrapper,
            CC_Config config)
        {
            this.voiceSynth = voiceSynth;
            this.dictationChoicesBuilder = dictationChoicesBuilder;
            this.speechWrapper = speechWrapper;

            this.configChoice = config;

            triggers = new Dictionary<string, DictationChoicesBuilder.SpeechAction>()
            {
                { "Hey Jenny", (string s) => { onEntry("Hey Ibn, Do you need anything?"); } },
                { "Jenny are you there", (string s) => { onEntry("Yes i am here, Anything you want?"); } },
                { "Jenny you there", (string s) => { onEntry("Yes right here. Is there anything you need"); } }
            };
        }

        private void onEntry(string awnser)
        {
            voiceSynth.SpeakAndWrite(awnser);

            dictationChoicesBuilder.Clear();
            dictationChoicesBuilder.AddCommandChoice(configChoice);
            dictationChoicesBuilder.AddScentence("No", (string s) => { dictationChoicesBuilder.Clear(); dictationChoicesBuilder.AddCommandChoice(this); speechWrapper.UpdateGrammar(); });

            speechWrapper.UpdateGrammar();
        }
    }
}
