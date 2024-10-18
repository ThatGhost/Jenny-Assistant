using System.Speech.Recognition;

namespace Jenny.Core
{
    public class SpeechRecognitionWrapper
    {
        private SpeechRecognitionEngine recognizer;
        private DictationChoicesBuilder choicesBuilder;

        public SpeechRecognitionWrapper(DictationChoicesBuilder builder) {
            choicesBuilder = builder;

            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

            builder.AddScentence("Hey", () => { Console.WriteLine("NOTHING ADDED TO SPEECH"); });
            recognizer.LoadGrammar(builder.BuildGrammar());

            recognizer.SpeechRecognized +=
                new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

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
            Console.WriteLine($"You: {e.Result.Text}");
            choicesBuilder.InvokeAction(e.Result.Text);
        }
    }
}
