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
        public CommandChoice EntryCommand { get; set; }
        public CancellationToken Token { get; set; }

        public DictationChoicesBuilder() { }

        public void AddScentence(string scentence, SpeechAction action)
        {
            scentencesActions.Add(scentence, action);
        }

        public void Clear()
        {
            scentencesActions.Clear();
            addStop();
        }

        public void AddCommandChoice(CommandChoice commandChoice)
        {
            foreach (var choice in commandChoice.triggers)
            {
                scentencesActions.Add(choice.Key, choice.Value);
            }
        }

        public void InvokeAction(string scentence)
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

        private void addStop()
        {
            scentencesActions.Add("Stop", () =>
            {
                Console.WriteLine("Stop command given");
                Clear();
                AddCommandChoice(EntryCommand);
            });
        }
    }
}
