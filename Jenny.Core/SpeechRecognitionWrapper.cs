using System.Speech.Recognition;

namespace Jenny.Core
{
    public class SpeechRecognitionWrapper
    {
        private readonly SpeechRecognitionEngine recognizer;
        private readonly DictationChoicesBuilder choicesBuilder;
        private readonly LogService logService;

        public SpeechRecognitionWrapper(
            DictationChoicesBuilder builder,
            LogService logService
        ) {
            this.choicesBuilder = builder;
            this.logService = logService;

            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

            builder.AddScentence("Hey", (string s) => { logService.LogWithColor("NOTHING ADDED TO SPEECH", ConsoleColor.Red); });
            recognizer.LoadGrammar(builder.BuildGrammar());

            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
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

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            logService.LogUser($"You: {e.Result.Text}");
            choicesBuilder.InvokeAction(e.Result.Text);
        }
    }
}
