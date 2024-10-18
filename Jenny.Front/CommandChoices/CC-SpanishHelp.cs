using Jenny.Core;

namespace Jenny.front.CommandChoices
{
    public class CC_SpanishHelp : CommandPath
    {
        private readonly VoiceSynthesizer voiceSynthWrapper;
        private readonly DictationBuilder dictationChoicesBuilder;
        private readonly SpeechRecognizer speechRecognitionWrapper;

        public CC_SpanishHelp(
            VoiceSynthesizer voiceSynthWrapper,
            DictationBuilder dictationChoicesBuilder,
            SpeechRecognizer speechRecognitionWrapper
            ) 
        {
            this.voiceSynthWrapper = voiceSynthWrapper;
            this.dictationChoicesBuilder = dictationChoicesBuilder;
            this.speechRecognitionWrapper = speechRecognitionWrapper;

            speechActions = new Dictionary<string, DictationBuilder.SpeechAction>()
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

            dictationChoicesBuilder.ClearDictations();
            dictationChoicesBuilder.AddScentence("Hola", (string s) => { voiceSynthWrapper.SpeakAndWrite("Hola Ivan, como estas"); });
            speechRecognitionWrapper.UpdateGrammar();
        }
    }
}
