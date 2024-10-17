using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Speech.Recognition;
using System.Threading.Tasks;

namespace Jenny.Core
{
    public class SpeechWrapper
    {
        private SpeechRecognitionEngine recognizer;
        private DictationChoicesBuilder choicesBuilder;

        public SpeechWrapper(DictationChoicesBuilder builder) {
            choicesBuilder = builder;

            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

            recognizer.LoadGrammar(builder.BuildGrammar());

            recognizer.SpeechRecognized +=
                new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void StopSpeechRegonition()
        {
            recognizer.Dispose();
        }

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            choicesBuilder.invokeAction(e.Result.Text);
        }
    }
}
