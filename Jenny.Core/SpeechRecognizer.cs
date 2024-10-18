using System.Speech.Recognition;

namespace Jenny.Core
{
    public class SpeechRecognizer
    {
        private SpeechRecognitionEngine speechRecognitionEngine;
        private readonly DictationBuilder dictationBuilder;
        private readonly LogService logService;

        public SpeechRecognizer(
            DictationBuilder dictationBuilder,
            LogService logService
        ) {
            this.dictationBuilder = dictationBuilder;
            this.logService = logService;

            UpdateLanguage("en-US");
        }

        public void UpdateGrammar()
        {
            speechRecognitionEngine.UnloadAllGrammars();
            speechRecognitionEngine.LoadGrammar(dictationBuilder.BuildGrammar());
        }

        public void UpdateLanguage(string language)
        {
            speechRecognitionEngine = new SpeechRecognitionEngine(new System.Globalization.CultureInfo(language));
            dictationBuilder.cultureInfo = speechRecognitionEngine.RecognizerInfo.Culture;

            dictationBuilder.AddScentence("Hey", (string s) => { logService.LogWithColor("NOTHING ADDED TO SPEECH", ConsoleColor.Red); });
            speechRecognitionEngine.LoadGrammar(dictationBuilder.BuildGrammar());

            speechRecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            logService.LogUser($"You: {e.Result.Text}");
            dictationBuilder.InvokeAction(e.Result.Text);
        }
    }
}
