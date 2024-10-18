using Microsoft.Extensions.DependencyInjection;
using Jenny.Core;
using Jenny.front.CommandChoices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Jenny.front;

namespace Jenny_front
{
    internal class Program
    {
        private static CancellationToken CancellationToken { get; set; }

        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Configuration.AddUserSecrets<Program>();

            Installer.Install(builder.Services);

            StartListening(builder);
        }

        static void StartListening(HostApplicationBuilder builder)
        {
            using IHost host = builder.Build();

            DictationChoicesBuilder dictationChoicesBuilder = host.Services.GetService<DictationChoicesBuilder>()!;
            dictationChoicesBuilder.Clear();
            CC_Entry entry = host.Services.GetService<CC_Entry>()!;
            dictationChoicesBuilder.EntryCommand = entry;
            dictationChoicesBuilder.AddCommandChoice(entry);

            SpeechRecognitionWrapper speechWrapper = host.Services.GetService<SpeechRecognitionWrapper>()!;
            dictationChoicesBuilder.updateGrammar = speechWrapper.UpdateGrammar;
            speechWrapper.UpdateGrammar();

            VoiceSynthWrapper voiceSynthWrapper = host.Services.GetService<VoiceSynthWrapper>()!;
            var googleApiKey = builder.Configuration["Google:ApiKey"]; // secret api key
            voiceSynthWrapper.GoogleVoiceApiKey = googleApiKey!;

            Console.WriteLine("Setup complete");

            Console.ReadLine();
        }
    }
}
