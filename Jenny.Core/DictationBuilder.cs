using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace Jenny.Core
{
    public class DictationBuilder
    {
        public delegate void SpeechAction(string o);
        public delegate void NoParamDelegate();


        private Dictionary<string, SpeechAction> dictations = new Dictionary<string, SpeechAction>();

        // for updating languages
        internal CultureInfo cultureInfo { private get; set; }

        // On stop
        public CommandPath? EntryCommand { private get; set; }
        public NoParamDelegate updateGrammerFunction;
        public NoParamDelegate onStop;
        public bool haveStop { private get; set; }
        public string stopString = "stop";

        // logging what you say
        private readonly LogService logService;

        public DictationBuilder(
            LogService logService
            ) 
        {
            this.haveStop = true;
            this.logService = logService; 
        }

        public void ClearDictations()
        {
            dictations.Clear();
            addStopCommand();
        }

        public void AddScentence(string scentence, SpeechAction action)
        {
            dictations.Add(scentence, action);
        }

        public void AddCommandPath(CommandPath commandPath)
        {
            foreach (var ath in commandPath.speechActions)
            {
                dictations.Add(ath.Key, ath.Value);
            }
        }

        public void DictateNumbersBetween(int min, int max, SpeechAction action)
        {
            for (int i = min; i <= max; i++)
            {
                dictations.Add($"{i}", action);
            }
        }

        public void AddScenentences(string[] scentences, SpeechAction action)
        {
            foreach(string s in scentences)
            {
                if (!dictations.ContainsKey(s)) dictations.Add(s, action);
            }
        }


        // Use in core
        internal void InvokeAction(string scentence)
        {
            if (!dictations.ContainsKey(scentence))
            {
                logService.LogWithColor("Unrecognised command", ConsoleColor.Red);
                return;
            }

            dictations[scentence].Invoke(scentence);
        }

        internal Grammar BuildGrammar()
        {
            Choices commands = new();
            commands.Add(dictations.Keys.ToArray());

            // builder
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = cultureInfo;
            grammarBuilder.Append(commands);

            // build
            return new Grammar(grammarBuilder);
        }

        private void addStopCommand()
        {
            if (!haveStop) return;
            dictations.Add(stopString, (string o) =>
            {
                logService.LogUser("Stop command given!");

                // start from beginning
                ClearDictations();
                AddCommandPath(EntryCommand!);

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
