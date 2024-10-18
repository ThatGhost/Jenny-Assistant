using Jenny.Core;

namespace Jenny.front.CommandChoices
{
    public class CC_SpanishHelp : CommandChoice
    {
        private readonly VoiceSynthWrapper voiceSynthWrapper;
        private readonly DictationChoicesBuilder dictationChoicesBuilder;
        private readonly SpeechRecognitionWrapper speechRecognitionWrapper;

        public CC_SpanishHelp(
            VoiceSynthWrapper voiceSynthWrapper,
            DictationChoicesBuilder dictationChoicesBuilder,
            SpeechRecognitionWrapper speechRecognitionWrapper
            ) 
        {
            this.voiceSynthWrapper = voiceSynthWrapper;
            this.dictationChoicesBuilder = dictationChoicesBuilder;
            this.speechRecognitionWrapper = speechRecognitionWrapper;

            triggers = new Dictionary<string, DictationChoicesBuilder.SpeechAction>()
            {
                { "Let's practice some spanish", startSpanishPractice },
                { "Let's do some spanish", startSpanishPractice },
                { "Let's try some spanish", startSpanishPractice },
                { "Let's practice spanish", startSpanishPractice },
                { "Let's do spanish", startSpanishPractice },
                { "Let's try spanish", startSpanishPractice },
                { "Let's learn spanish", startSpanishPractice },
            };
        }

        private void startSpanishPractice(string s)
        {
            speechRecognitionWrapper.UpdateLanguage("es-ES");
            voiceSynthWrapper.UpdateLanguage("es-ES");

            // Put it back to english if i give the stop command
            dictationChoicesBuilder.onStop += () =>
            {
                speechRecognitionWrapper.UpdateLanguage("en-US");
                voiceSynthWrapper.UpdateLanguage("en-US");
            };

            dictationChoicesBuilder.Clear();
            dictationChoicesBuilder.AddScentence("Hola", (string s) => { voiceSynthWrapper.SpeakAndWrite("Hola Ivan, como estas"); });
            speechRecognitionWrapper.UpdateGrammar();
        }
    }
}
