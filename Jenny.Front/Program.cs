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

            DictationBuilder dictationChoicesBuilder = host.Services.GetService<DictationBuilder>()!;
            dictationChoicesBuilder.ClearDictations();
            CC_Entry entry = host.Services.GetService<CC_Entry>()!;
            dictationChoicesBuilder.EntryCommand = entry;
            dictationChoicesBuilder.AddCommandPath(entry);

            SpeechRecognizer speechWrapper = host.Services.GetService<SpeechRecognizer>()!;
            dictationChoicesBuilder.updateGrammerFunction = speechWrapper.UpdateGrammar;
            speechWrapper.UpdateGrammar();

            VoiceSynthesizer voiceSynthWrapper = host.Services.GetService<VoiceSynthesizer>()!;
            var googleApiKey = builder.Configuration["Google:ApiKey"]; // secret api key
            voiceSynthWrapper.SetTTSApiKey(googleApiKey!);

            Console.WriteLine("Setup complete");

            Console.ReadLine();
        }
    }
}
