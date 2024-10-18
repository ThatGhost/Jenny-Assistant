using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Jenny.Core
{
    public class VoiceSynthWrapper
    {
        private readonly SpeechSynthesizer synthesizer;

        public VoiceSynthWrapper()
        {
            synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();

            foreach (var vinfo in synthesizer.GetInstalledVoices())
            {
                Console.WriteLine(vinfo.VoiceInfo.Name);
            }
        }

        public void Speak(string command)
        {
            synthesizer.SpeakAsync(command);
        }

        public void SpeakAndWrite(string command)
        {
            Speak(command);
            Console.WriteLine(command);
        }

        public void Stop() => synthesizer.Dispose();
        public void VolumeUp()
        {
            if(Volume < 100)
                synthesizer.Volume++;
        }

        public void VolumeDown()
        {
            if(Volume > 0)
                synthesizer.Volume--;
        }

        public int Volume { get { return synthesizer.Volume; } }
    }
}
