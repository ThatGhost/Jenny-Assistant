
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Speech.Recognition;
using System.Threading.Tasks;

using Jenny.Core;
namespace Jenny_front
{
    internal class Program
    {
        static void Main(string[] args)
        {

            DictationChoicesBuilder dictationChoicesBuilder = new DictationChoicesBuilder();
            dictationChoicesBuilder.AddScentence("Hey are you there?", onAreYouThere);
            dictationChoicesBuilder.AddScentence("You have a coffee?", onYouHaveCoffee);

            SpeechWrapper speechWrapper = new SpeechWrapper(dictationChoicesBuilder);

            Console.WriteLine("Setup complete");

            Console.ReadLine();
            speechWrapper.StopSpeechRegonition();
        }

        static void onAreYouThere()
        {
            Console.WriteLine("Yes i am here");
        }
        
        static void onYouHaveCoffee()
        {
            Console.WriteLine("We are all out, sorry");
        }
    }
}
