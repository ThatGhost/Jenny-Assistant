using System.Speech.Recognition;

namespace Jenny.Core
{
    public class SpeechRecognitionWrapper
    {
        private SpeechRecognitionEngine recognizer;
        private readonly DictationChoicesBuilder choicesBuilder;
        private readonly LogService logService;

        public SpeechRecognitionWrapper(
            DictationChoicesBuilder builder,
            LogService logService
        ) {
            this.choicesBuilder = builder;
            this.logService = logService;

            UpdateLanguage("en-US");
        }

        public void UpdateGrammar()
        {
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar(choicesBuilder.BuildGrammar());
        }

        public void StopSpeechRegonition()
        {
            recognizer.Dispose();
        }

        public void UpdateLanguage(string language)
        {
            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo(language));
            choicesBuilder.cultureInfo = recognizer.RecognizerInfo.Culture;

            choicesBuilder.AddScentence("Hey", (string s) => { logService.LogWithColor("NOTHING ADDED TO SPEECH", ConsoleColor.Red); });
            recognizer.LoadGrammar(choicesBuilder.BuildGrammar());

            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            logService.LogUser($"You: {e.Result.Text}");
            choicesBuilder.InvokeAction(e.Result.Text);
        }
    }
}
