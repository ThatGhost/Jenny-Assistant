using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace Jenny.Core
{
    public class DictationChoicesBuilder
    {
        public delegate void SpeechAction(string o);
        public delegate void NoParamDelegate();


        Dictionary<string, SpeechAction> scentencesActions = new Dictionary<string, SpeechAction>();
        public CommandChoice? EntryCommand { private get; set; }
        public NoParamDelegate updateGrammerFunction;
        public NoParamDelegate onStop;
        public CultureInfo cultureInfo { private get; set; }

        private readonly LogService logService;

        public DictationChoicesBuilder(
            LogService logService
            ) 
        {
            this.logService = logService; 
        }

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

        public void NumbersBetween(int min, int max, SpeechAction action)
        {
            for (int i = min; i <= max; i++)
            {
                scentencesActions.Add($"{i}", action);
            }
        }

        public void InvokeAction(string scentence)
        {
            if (!scentencesActions.ContainsKey(scentence))
            {
                logService.LogWithColor("Unrecognised command", ConsoleColor.Red);
                return;
            }

            scentencesActions[scentence].Invoke(scentence);
        }

        public Grammar BuildGrammar()
        {
            Choices commands = new();
            commands.Add(scentencesActions.Keys.ToArray());

            // builder
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = cultureInfo;
            grammarBuilder.Append(commands);

            // build
            return new Grammar(grammarBuilder);
        }

        private void addStop()
        {
            scentencesActions.Add("Stop", (string o) =>
            {
                logService.LogUser("Stop command given!");

                // start from beginning
                Clear();
                AddCommandChoice(EntryCommand!);

                // trigger any distructors
                if(onStop != null && onStop.GetInvocationList().Length > 0)
                {
                    onStop.Invoke();
                    foreach (var command in onStop.GetInvocationList())
                    {
                        onStop -= (command as NoParamDelegate);
                    }
                }


                // update the grammer to start from the beginning
                updateGrammerFunction.Invoke();
            });
        }
    }
}
