using Jenny.Core;
using Jenny.front.Services;

namespace Jenny.front.CommandChoices
{
    public class CC_SpanishHelp : CommandPath
    {
        private readonly VoiceSynthesizer voiceSynthWrapper;
        private readonly DictationBuilder dictationChoicesBuilder;
        private readonly SpeechRecognizer speechRecognitionWrapper;
        private readonly SpanishDictationService spanishDictationService;
        private readonly LogService logService;

        public CC_SpanishHelp(
            VoiceSynthesizer voiceSynthWrapper,
            DictationBuilder dictationChoicesBuilder,
            SpeechRecognizer speechRecognitionWrapper,
            SpanishDictationService spanishDictationService,
            LogService logService
            ) 
        {
            this.voiceSynthWrapper = voiceSynthWrapper;
            this.dictationChoicesBuilder = dictationChoicesBuilder;
            this.speechRecognitionWrapper = speechRecognitionWrapper;
            this.spanishDictationService = spanishDictationService;
            this.logService = logService;

            speechActions = new Dictionary<string, DictationBuilder.SpeechAction>()
            {
                { "spanish", startSpanishPractice },
            };
        }

        private void startSpanishPractice(string s)
        {
            speechRecognitionWrapper.UpdateLanguage("es-ES");
            voiceSynthWrapper.UpdateLanguage("es-ES");
            dictationChoicesBuilder.stopString = "detener";

            // Put it back to english if i give the stop command
            dictationChoicesBuilder.onStop += () =>
            {
                speechRecognitionWrapper.UpdateLanguage("en-US");
                voiceSynthWrapper.UpdateLanguage("en-US");
                dictationChoicesBuilder.stopString = "stop";
            };

            voiceSynthWrapper.SpeakAndWrite("Qué es lo que quieres hacer");
            logService.LogWithColor("Dictar", ConsoleColor.Blue);
            logService.LogWithColor("Traducir", ConsoleColor.Blue);

            dictationChoicesBuilder.ClearDictations();
            dictationChoicesBuilder.AddScentence("dictar", (string s) => { spanishDictationService.startDictation(); });
            dictationChoicesBuilder.AddScentence("traducir", (string s) => { spanishDictationService.startTranslations(); });
            speechRecognitionWrapper.UpdateGrammar();
        }
    }
}
