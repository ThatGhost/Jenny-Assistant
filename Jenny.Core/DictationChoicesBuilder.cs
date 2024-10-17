using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace Jenny.Core
{
    public class DictationChoicesBuilder
    {
        public delegate void SpeechAction();
        Dictionary<string, SpeechAction> scentencesActions = new Dictionary<string, SpeechAction>();

        public DictationChoicesBuilder() { }

        public void AddScentence(string scentence, SpeechAction action)
        {
            scentencesActions.Add(scentence, action);
        }

        public void invokeAction(string scentence)
        {
            if (!scentencesActions.ContainsKey(scentence))
            {
                Console.WriteLine("HUH??? I AINT HEAR YOU");
                return;
            }

            scentencesActions[scentence].Invoke();
        }

        public Grammar BuildGrammar()
        {
            Choices commands = new();
            commands.Add(scentencesActions.Keys.ToArray());

            // builder
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commands);

            // build
            return new Grammar(grammarBuilder);
        }
    }
}
